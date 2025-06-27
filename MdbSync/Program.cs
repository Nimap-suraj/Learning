using System.Text.Json;
using System.Threading;
using System.Data.OleDb;
using System.Data;
using System.Net.NetworkInformation;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Collections.Concurrent;
using System.Net.Sockets;

namespace RobustAccessDbSync
{
    class SyncMetadata
    {
        public Dictionary<string, DateTime> TableLastSync { get; set; } = new Dictionary<string, DateTime>();
        public List<QueuedChange> QueuedChanges { get; set; } = new List<QueuedChange>();
    }

    class QueuedChange
    {
        public string TableName { get; set; }
        public string PkColumn { get; set; }
        public Dictionary<string, object> RowData { get; set; }
        public bool IsDelete { get; set; }
        public DateTime ChangeTime { get; set; }
    }

    class Program
    {
        static string DRIVE_LETTER = "X:";
        private static bool _syncRunning = true;
        private const string ConflictSuffix = "_CONFLICT_RESOLVED";

        static string SERVER_IP;
        static string SHARE_NAME;
        static string USERNAME;
        static string PASSWORD;
        private static bool _isOnline = true;
        private static DateTime _lastOnlineTime = DateTime.MinValue;

        [System.Runtime.Versioning.SupportedOSPlatform("windows")]
        static async Task Main()
        {
            Console.CursorVisible = false;
            ShowGameStyleLoader("Initializing Database Synchronization Tool", 20);

            Console.WriteLine("\nDatabase Synchronization Tool");
            Console.WriteLine("-----------------------------");

            Console.Write("Enter SERVER IP: ");
            SERVER_IP = Console.ReadLine();

            Console.Write("Enter SHARE NAME: ");
            SHARE_NAME = Console.ReadLine();

            Console.Write("Enter USERNAME: ");
            USERNAME = Console.ReadLine();

            Console.Write("Enter PASSWORD: ");
            PASSWORD = Console.ReadLine();
            Console.WriteLine();

            Console.Write("Enter client database path (e.g., C:\\path\\client.mdb): ");
            string clientDbPath = Console.ReadLine();
            Console.WriteLine();

            Console.Write("Enter server database filename (e.g., TEST.mdb): ");
            string serverFileName = Console.ReadLine();
            string serverDbPath = $@"\\{SERVER_IP}\{SHARE_NAME}\{serverFileName}";
            Console.WriteLine($"Resolved Server DB Path: {serverDbPath}");

            bool isNewClientDb = false;
            if (!HasMdbExtension(clientDbPath))
            {
                while (true)
                {
                    Console.Write("Enter destination folder (e.g., C:\\DemoFiles): ");
                    string destFolder = clientDbPath;

                    if (!Directory.Exists(destFolder))
                    {
                        Console.WriteLine($"ERROR: Destination folder does not exist: {destFolder}");
                        Console.ReadKey();
                        continue;
                    }

                    clientDbPath = Path.Combine(destFolder, Path.GetFileName(serverDbPath));

                    RunCommand($"net use {DRIVE_LETTER} /delete", false);

                    Console.WriteLine("Mounting shared folder...");
                    var connectCmd = $"net use {DRIVE_LETTER} \\\\{SERVER_IP}\\{SHARE_NAME} /user:{USERNAME} {PASSWORD} /persistent:no";
                    if (!RunCommand(connectCmd))
                    {
                        Console.WriteLine("ERROR: Failed to connect to shared folder.");
                        Console.ReadKey();
                        continue;
                    }

                    string serverFilePath = Path.Combine(DRIVE_LETTER, Path.GetFileName(serverDbPath));

                    if (!File.Exists(serverFilePath))
                    {
                        Console.WriteLine($"ERROR: File does not exist on server: {Path.GetFileName(serverDbPath)}");
                        RunCommand($"net use {DRIVE_LETTER} /delete", false);
                        Console.ReadKey();
                        continue;
                    }

                    Console.WriteLine("Copying file from server...");
                    try
                    {
                        File.Copy(serverFilePath, clientDbPath, true);
                        Console.WriteLine($"File successfully copied to: {clientDbPath}");
                        isNewClientDb = true;
                        break;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"ERROR: File copy failed. {ex.Message}");
                    }

                    RunCommand($"net use {DRIVE_LETTER} /delete", false);
                }
            }

            string syncMetaFile = "sync_metadata.json";
            SyncMetadata metadata = null;

            // Check if client DB exists
            if (!File.Exists(clientDbPath))
            {
                Console.WriteLine("Client database not found. Attempting to pull from server...");
                if (await PullDatabaseFromServer(serverDbPath, clientDbPath))
                {
                    Console.WriteLine("Successfully pulled database from server to client.");
                    isNewClientDb = true;
                }
                else
                {
                    Console.WriteLine("\nPress any key to exit...");
                    Console.ReadKey();
                    return;
                }
            }

            ShowGameStyleLoader("Verifying database files", 10);
            Console.WriteLine();

            if (!VerifyDatabaseFiles(clientDbPath, serverDbPath))
            {
                Console.WriteLine("\nPress any key to exit...");
                Console.ReadKey();
                return;
            }

            string clientConnStr = $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={clientDbPath};";
            string serverConnStr = $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={serverDbPath};";

            ShowGameStyleLoader("Testing database connections", 20);
            if (!TestConnection("Client DB", clientConnStr) || !TestConnection("Server DB", serverConnStr))
            {
                Console.WriteLine("\nPress any key to exit...");
                Console.ReadKey();
                return;
            }

            ShowGameStyleLoader("Loading synchronization metadata", 10);
            Console.WriteLine();

            metadata = LoadSyncMetadata(syncMetaFile) ?? new SyncMetadata();

            // Initialize metadata with appropriate timestamps
            InitializeMetadata(metadata, clientConnStr, serverConnStr, isNewClientDb);

            Console.WriteLine("\nStarting optimized synchronization...");
            Console.WriteLine("Press 'Q' then Enter to stop synchronization.\n");

            var syncTask = Task.Run(() => ContinuousSync(serverConnStr, clientConnStr, syncMetaFile, metadata));

            while (_syncRunning)
            {
                if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Q)
                {
                    _syncRunning = false;
                    Console.WriteLine("Stopping synchronization...");
                }
                await Task.Delay(100);
            }

            await syncTask;
            Console.WriteLine("\nSynchronization stopped. Press any key to exit.");
            Console.CursorVisible = true;
            Console.ReadKey();
        }

        static void InitializeMetadata(SyncMetadata metadata, string clientConnStr, string serverConnStr, bool isNewClientDb)
        {
            var allTables = GetAllTableNames(clientConnStr)
                .Union(GetAllTableNames(serverConnStr))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            foreach (var table in allTables)
            {
                if (!metadata.TableLastSync.ContainsKey(table))
                {
                    // For newly created client DBs, use current time to skip initial identical records
                    if (isNewClientDb)
                    {
                        metadata.TableLastSync[table] = DateTime.UtcNow;
                        Console.WriteLine($"Initialized new table '{table}' with current timestamp");
                    }
                    else
                    {
                        // For existing DBs, use max Serverzeit from table
                        try
                        {
                            using var conn = new OleDbConnection(clientConnStr);
                            conn.Open();
                            using var cmd = new OleDbCommand($"SELECT MAX(Serverzeit) FROM [{table}]", conn);
                            var result = cmd.ExecuteScalar();

                            if (result != DBNull.Value && result != null)
                            {
                                metadata.TableLastSync[table] = (DateTime)result;
                                Console.WriteLine($"Initialized table '{table}' with max Serverzeit: {(DateTime)result:yyyy-MM-dd HH:mm:ss}");
                            }
                            else
                            {
                                metadata.TableLastSync[table] = DateTime.MinValue;
                                Console.WriteLine($"Initialized table '{table}' with MinValue (no records)");
                            }
                        }
                        catch
                        {
                            metadata.TableLastSync[table] = DateTime.MinValue;
                            Console.WriteLine($"Initialized table '{table}' with MinValue (error)");
                        }
                    }
                }
            }
        }

        static bool RunCommand(string command, bool showOutput = true)
        {
            try
            {
                ProcessStartInfo procInfo = new ProcessStartInfo("cmd.exe", "/c " + command)
                {
                    RedirectStandardOutput = !showOutput,
                    RedirectStandardError = !showOutput,
                    UseShellExecute = false,
                    CreateNoWindow = !showOutput
                };

                using (Process proc = Process.Start(procInfo))
                {
                    proc.WaitForExit();
                    return proc.ExitCode == 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Command execution failed: " + ex.Message);
                return false;
            }
        }

        public static bool HasMdbExtension(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return false;

            string extension = Path.GetExtension(path);
            return extension.Equals(".mdb", StringComparison.OrdinalIgnoreCase);
        }

        static async Task<bool> PullDatabaseFromServer(string serverPath, string clientPath)
        {
            try
            {
                var serverParts = serverPath.Split(new[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
                if (serverParts.Length < 2)
                {
                    Console.WriteLine("Invalid server path format. Expected format: \\\\server\\share\\path\\file.mdb");
                    return false;
                }

                string serverIP = serverParts[0];
                string shareName = serverParts[1];
                string serverFilePath = string.Join("\\", serverParts.Skip(2));
                string fileName = Path.GetFileName(serverPath);

                if (!PingHost("127.0.0.1") || !PingHost(serverIP))
                {
                    Console.WriteLine("ERROR: Network connectivity issues");
                    return false;
                }

                bool isClientPathDirectory = Directory.Exists(clientPath) ||
                                           (clientPath.EndsWith("\\") ||
                                            clientPath.EndsWith("/"));

                string finalClientPath;
                if (isClientPathDirectory)
                {
                    Directory.CreateDirectory(clientPath);
                    finalClientPath = Path.Combine(clientPath, fileName);
                }
                else
                {
                    string directory = Path.GetDirectoryName(clientPath);
                    if (!string.IsNullOrEmpty(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }
                    finalClientPath = clientPath;
                }

                RunCommand($"net use {DRIVE_LETTER} /delete", false);

                Console.WriteLine("Mounting server share...");
                string connectCmd = $"net use {DRIVE_LETTER} \\\\{serverIP}\\{shareName} /user:{USERNAME} {PASSWORD} /persistent:no";
                if (!RunCommand(connectCmd))
                {
                    Console.WriteLine("ERROR: Failed to connect to shared folder");
                    return false;
                }

                string serverFile = $"{DRIVE_LETTER}\\{serverFilePath}";
                if (!File.Exists(serverFile))
                {
                    Console.WriteLine($"ERROR: File does not exist on server: {serverFilePath}");
                    RunCommand($"net use {DRIVE_LETTER} /delete", false);
                    return false;
                }

                Console.WriteLine($"Copying file from server to {finalClientPath}...");
                try
                {
                    File.Copy(serverFile, finalClientPath, true);
                    Console.WriteLine("File successfully copied");
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ERROR: File copy failed: {ex.Message}");
                    return false;
                }
                finally
                {
                    RunCommand($"net use {DRIVE_LETTER} /delete", false);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error pulling database from server: {ex.Message}");
                return false;
            }
        }

        static bool CheckNetworkConnection(string ip)
        {
            try
            {
                using (var tcpClient = new TcpClient())
                {
                    var result = tcpClient.BeginConnect(ip, 445, null, null);
                    var success = result.AsyncWaitHandle.WaitOne(2000);
                    if (tcpClient.Connected) tcpClient.EndConnect(result);
                    return success;
                }
            }
            catch
            {
                return false;
            }
        }

        static async Task ContinuousSync(
            string serverConnStr,
            string clientConnStr,
            string syncMetaFile,
            SyncMetadata metadata)
        {
            var clientTables = GetAllTableNames(clientConnStr);
            var serverTables = GetAllTableNames(serverConnStr);
            var allTables = clientTables.Union(serverTables, StringComparer.OrdinalIgnoreCase).ToList();

            while (_syncRunning)
            {
                try
                {
                    // Check network status every 5 seconds
                    _isOnline = CheckNetworkConnection(SERVER_IP);

                    if (_isOnline)
                    {
                        if (_lastOnlineTime == DateTime.MinValue)
                        {
                            Console.WriteLine($"[{DateTime.Now:T}] Connection restored");
                        }
                        _lastOnlineTime = DateTime.Now;

                        // Process queued offline changes first
                        if (metadata.QueuedChanges.Count > 0)
                        {
                            Console.WriteLine($"[{DateTime.Now:T}] Processing {metadata.QueuedChanges.Count} queued changes");
                            await ProcessQueuedChanges(metadata, clientConnStr, serverConnStr);
                            SaveSyncMetadata(syncMetaFile, metadata);
                        }

                        // Sync each table
                        foreach (var tableName in allTables)
                        {
                            try
                            {
                                DateTime lastSync = metadata.TableLastSync.ContainsKey(tableName)
                                    ? metadata.TableLastSync[tableName]
                                    : DateTime.MinValue;

                                Console.WriteLine($"[{DateTime.Now:T}] Syncing {tableName} since {lastSync:yyyy-MM-dd HH:mm:ss}");

                                // Server -> Client
                                int serverToClient = await SyncDirection(
                                    sourceConnStr: serverConnStr,
                                    targetConnStr: clientConnStr,
                                    tableName: tableName,
                                    lastSync: lastSync,
                                    isServerToClient: true,
                                    metadata: metadata
                                );

                                // Client -> Server
                                int clientToServer = await SyncDirection(
                                    sourceConnStr: clientConnStr,
                                    targetConnStr: serverConnStr,
                                    tableName: tableName,
                                    lastSync: lastSync,
                                    isServerToClient: false,
                                    metadata: metadata
                                );

                                if (serverToClient > 0 || clientToServer > 0)
                                {
                                    Console.WriteLine($"[{DateTime.Now:T}] {tableName} sync: " +
                                                    $"Server→Client: {serverToClient}, " +
                                                    $"Client→Server: {clientToServer}");

                                    // Update sync time only if changes were processed
                                    metadata.TableLastSync[tableName] = DateTime.UtcNow;
                                }
                                else
                                {
                                    Console.WriteLine($"[{DateTime.Now:T}] No changes for {tableName}");
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"[{DateTime.Now:T}] Error syncing table {tableName}: {ex.Message}");
                            }
                        }
                    }
                    else
                    {
                        if (_lastOnlineTime != DateTime.MinValue)
                        {
                            Console.WriteLine($"[{DateTime.Now:T}] Connection lost - entering offline mode");
                            _lastOnlineTime = DateTime.MinValue;
                        }

                        // Queue client changes made while offline
                        await QueueLocalChanges(metadata, clientConnStr);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[{DateTime.Now:T}] Sync cycle error: {ex.Message}");
                }
                finally
                {
                    SaveSyncMetadata(syncMetaFile, metadata);
                    await Task.Delay(5000);
                }
            }
        }

        static async Task ProcessQueuedChanges(
            SyncMetadata metadata,
            string clientConnStr,
            string serverConnStr)
        {
            var processedChanges = new List<QueuedChange>();

            foreach (var change in metadata.QueuedChanges)
            {
                try
                {
                    if (change.IsDelete)
                    {
                        using (var conn = new OleDbConnection(serverConnStr))
                        {
                            conn.Open();
                            DeleteRecord(conn, change.TableName, change.PkColumn, change.RowData[change.PkColumn]);
                        }
                    }
                    else
                    {
                        using (var conn = new OleDbConnection(serverConnStr))
                        {
                            conn.Open();
                            ApplyChangeWithConflictResolution(
                                conn,
                                change.TableName,
                                change.RowData,
                                isServerToClient: false,
                                change.PkColumn
                            );
                        }
                    }

                    processedChanges.Add(change);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[{DateTime.Now:T}] Failed to apply queued change: {ex.Message}");
                }
            }

            // Remove processed changes
            foreach (var change in processedChanges)
            {
                metadata.QueuedChanges.Remove(change);
            }
        }

        static async Task QueueLocalChanges(SyncMetadata metadata, string clientConnStr)
        {
            foreach (var tableName in GetAllTableNames(clientConnStr))
            {
                try
                {
                    DateTime lastSync = metadata.TableLastSync.ContainsKey(tableName)
                        ? metadata.TableLastSync[tableName]
                        : DateTime.MinValue;

                    string pkColumn = GetPrimaryKeyColumn(clientConnStr, tableName);
                    if (string.IsNullOrEmpty(pkColumn)) continue;

                    using (var conn = new OleDbConnection(clientConnStr))
                    {
                        conn.Open();

                        // Get modified records
                        string query = $@"SELECT * FROM [{tableName}] 
                                        WHERE Serverzeit > @lastSync";

                        using (var cmd = new OleDbCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@lastSync", lastSync);

                            using (var reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    var row = new Dictionary<string, object>();
                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        row[reader.GetName(i)] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                                    }

                                    metadata.QueuedChanges.Add(new QueuedChange
                                    {
                                        TableName = tableName,
                                        PkColumn = pkColumn,
                                        RowData = row,
                                        IsDelete = false,
                                        ChangeTime = DateTime.UtcNow
                                    });
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[{DateTime.Now:T}] Error queuing changes for {tableName}: {ex.Message}");
                }
            }
        }

        static async Task<int> SyncDirection(
            string sourceConnStr,
            string targetConnStr,
            string tableName,
            DateTime lastSync,
            bool isServerToClient,
            SyncMetadata metadata)
        {
            int changesApplied = 0;
            DateTime maxTimestamp = lastSync;

            try
            {
                using (var sourceConn = new OleDbConnection(sourceConnStr))
                {
                    sourceConn.Open();

                    if (!TableExists(sourceConn, tableName)) return 0;

                    string pkColumn = GetPrimaryKeyColumn(sourceConnStr, tableName);
                    if (string.IsNullOrEmpty(pkColumn)) return 0;

                    string query = $@"SELECT * FROM [{tableName}] 
                                    WHERE Serverzeit > @lastSync
                                    ORDER BY Serverzeit";

                    using (var cmd = new OleDbCommand(query, sourceConn))
                    {
                        cmd.Parameters.AddWithValue("@lastSync", lastSync);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var row = new Dictionary<string, object>();
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    row[reader.GetName(i)] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                                }

                                using (var targetConn = new OleDbConnection(targetConnStr))
                                {
                                    targetConn.Open();

                                    if (ApplyChangeWithConflictResolution(
                                        targetConn,
                                        tableName,
                                        row,
                                        isServerToClient,
                                        pkColumn))
                                    {
                                        changesApplied++;
                                        var rowTimestamp = Convert.ToDateTime(row["Serverzeit"]);
                                        if (rowTimestamp > maxTimestamp)
                                            maxTimestamp = rowTimestamp;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{DateTime.Now:T}] Error syncing {tableName}: {ex.Message}");
            }

            // Update last sync time if we processed changes
            if (changesApplied > 0 && maxTimestamp > lastSync)
            {
                metadata.TableLastSync[tableName] = maxTimestamp;
            }

            return changesApplied;
        }

        static bool ApplyChangeWithConflictResolution(
            OleDbConnection targetConn,
            string tableName,
            Dictionary<string, object> row,
            bool isServerToClient,
            string pkColumn)
        {
            try
            {
                var pkValue = row[pkColumn];

                if (!TableExists(targetConn, tableName))
                {
                    CreateTableFromSource(targetConn, row, tableName);
                }

                bool exists = RecordExists(targetConn, tableName, pkColumn, pkValue);

                if (!exists)
                {
                    return InsertRecord(targetConn, tableName, row);
                }

                var targetLastModified = GetLastModified(targetConn, tableName, pkColumn, pkValue);
                var incomingLastModified = Convert.ToDateTime(row["Serverzeit"]);

                // Server changes always win in conflicts
                if (isServerToClient || incomingLastModified > targetLastModified)
                {
                    return UpdateRecord(targetConn, tableName, row, pkColumn);
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error applying change to {tableName}: {ex.Message}");
                return false;
            }
        }

        static void CreateTableFromSource(OleDbConnection targetConn, Dictionary<string, object> sampleRow, string tableName)
        {
            try
            {
                var createTableSql = new StringBuilder($"CREATE TABLE [{tableName}] (");
                DataTable columns = targetConn.GetOleDbSchemaTable(OleDbSchemaGuid.Columns,
                   new object[] { null, null, tableName, null });

                // Get primary key information
                DataTable primaryKeys = targetConn.GetOleDbSchemaTable(OleDbSchemaGuid.Primary_Keys,
                    new object[] { null, null, tableName });

                //foreach (var kvp in sampleRow)
                //{
                //    string columnName = kvp.Key;
                //    string dataType = "TEXT(255)"; // Default type

                //    if (kvp.Value is int) dataType = "INTEGER";
                //    else if (kvp.Value is double || kvp.Value is float) dataType = "DOUBLE";
                //    else if (kvp.Value is DateTime) dataType = "DATETIME";
                //    else if (kvp.Value is bool) dataType = "BIT";
                //    else if (kvp.Value is Guid) dataType = "GUID";

                //    createTableSql.Append($"[{columnName}] {dataType}, ");
                //}

                foreach (DataRow column in columns.Rows)
                {
                    string columnName = column["COLUMN_NAME"].ToString();
                    string dataType = GetSqlDataType(column);
                    bool isPrimaryKey = primaryKeys.Select($"COLUMN_NAME = '{columnName}'").Length > 0;

                    createTableSql.Append($"[{columnName}] {dataType}");

                    if (isPrimaryKey)
                        createTableSql.Append(" PRIMARY KEY");

                    createTableSql.Append(", ");
                }

                createTableSql.Append("[Serverzeit] DATETIME DEFAULT Now())");

                using var cmd = new OleDbCommand(createTableSql.ToString(), targetConn);
                cmd.ExecuteNonQuery();
                Console.WriteLine($"Created table {tableName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating table {tableName}: {ex.Message}");
            }
        }
        static string GetSqlDataType(DataRow column)
        {
            int oleDbType = (int)column["DATA_TYPE"];
            int size = column["CHARACTER_MAXIMUM_LENGTH"] is DBNull ? 0 : Convert.ToInt32(column["CHARACTER_MAXIMUM_LENGTH"]);

            switch (oleDbType)
            {
                case 130: // Text
                    return size > 0 ? $"TEXT({size})" : "TEXT(255)";
                case 3: // Integer
                    return "INTEGER";
                case 5: // Double
                    return "DOUBLE";
                case 7: // DateTime
                    return "DATETIME";
                case 11: // Boolean
                    return "BIT";
                case 72: // GUID
                    return "GUID";
                case 203: // Memo
                    return "MEMO";
                default:
                    return "TEXT(255)";
            }
        }

        static SyncMetadata LoadSyncMetadata(string path)
        {
            if (!File.Exists(path)) return new SyncMetadata();

            try
            {
                var json = File.ReadAllText(path);
                return JsonSerializer.Deserialize<SyncMetadata>(json);
            }
            catch
            {
                return new SyncMetadata();
            }
        }

        static void SaveSyncMetadata(string path, SyncMetadata metadata)
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                var json = JsonSerializer.Serialize(metadata, options);
                File.WriteAllText(path, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving metadata: {ex.Message}");
            }
        }

        // ========== HELPER METHODS ========== //

        static bool PingHost(string nameOrAddress)
        {
            try
            {
                using (var ping = new Ping())
                {
                    var reply = ping.Send(nameOrAddress, 2000);
                    return reply.Status == IPStatus.Success;
                }
            }
            catch
            {
                return false;
            }
        }

        static void ShowGameStyleLoader(string message, int totalSteps)
        {
            Console.Write(message + " ");
            int progressBarWidth = 30;

            for (int i = 0; i <= totalSteps; i++)
            {
                double percentage = (double)i / totalSteps;
                int filledBars = (int)(percentage * progressBarWidth);
                string bar = new string('█', filledBars).PadRight(progressBarWidth, '-');

                Console.Write($"\r{message} [{bar}] {percentage * 100:0}%");
                Thread.Sleep(20);
            }
            Console.WriteLine();
        }

        static bool VerifyDatabaseFiles(string clientPath, string serverPath)
        {
            if (!File.Exists(clientPath))
            {
                Console.WriteLine($"\nClient database not found at: {clientPath}");
                return false;
            }

            if (!File.Exists(serverPath))
            {
                Console.WriteLine($"\nServer database not found at: {serverPath}");
                return false;
            }

            return true;
        }

        static bool TestConnection(string name, string connectionString)
        {
            try
            {
                using var connection = new OleDbConnection(connectionString);
                connection.Open();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n{name} connection failed: {ex.Message}");
                return false;
            }
        }

        static List<string> GetAllTableNames(string connectionString)
        {
            var tables = new List<string>();
            try
            {
                using var conn = new OleDbConnection(connectionString);
                conn.Open();
                DataTable schemaTables = conn.GetSchema("Tables");

                foreach (DataRow row in schemaTables.Rows)
                {
                    string tableName = row["TABLE_NAME"].ToString();
                    string tableType = row["TABLE_TYPE"].ToString();

                    if (tableType == "TABLE" && !tableName.StartsWith("MSys")
                        && !tableName.StartsWith("~TMP") && !tableName.StartsWith("_"))
                    {
                        tables.Add(tableName);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting table names: {ex.Message}");
            }
            return tables;
        }

        static string GetPrimaryKeyColumn(string connectionString, string tableName)
        {
            try
            {
                using var conn = new OleDbConnection(connectionString);
                conn.Open();

                DataTable schema = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Primary_Keys,
                    new object[] { null, null, tableName });

                if (schema.Rows.Count > 0)
                {
                    return schema.Rows[0]["COLUMN_NAME"].ToString();
                }

                DataTable columns = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Columns,
                    new object[] { null, null, tableName, null });

                foreach (DataRow row in columns.Rows)
                {
                    string col = row["COLUMN_NAME"].ToString();

                    if (col.Equals("ID", StringComparison.OrdinalIgnoreCase) ||
                        col.Equals("GUID", StringComparison.OrdinalIgnoreCase))
                    {
                        return col;
                    }
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        static bool TableExists(OleDbConnection conn, string tableName)
        {
            try
            {
                DataTable schema = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables,
                    new object[] { null, null, tableName, "TABLE" });
                return schema.Rows.Count > 0;
            }
            catch
            {
                return false;
            }
        }

        static bool RecordExists(OleDbConnection conn, string tableName, string pkColumn, object pkValue)
        {
            try
            {
                string query = $"SELECT COUNT(*) FROM [{tableName}] WHERE [{pkColumn}] = ?";
                using var cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue($"@{pkColumn}", pkValue);
                return (int)cmd.ExecuteScalar() > 0;
            }
            catch
            {
                return false;
            }
        }

        static bool InsertRecord(OleDbConnection conn, string tableName, Dictionary<string, object> row)
        {
            try
            {
                var columns = row.Keys.ToList();
                var columnList = string.Join(", ", columns.Select(c => $"[{c}]"));
                var valuePlaceholders = string.Join(", ", columns.Select(_ => "?"));

                string insertQuery = $@"INSERT INTO [{tableName}] ({columnList}) VALUES ({valuePlaceholders})";

                using var cmd = new OleDbCommand(insertQuery, conn);
                foreach (var col in columns)
                    cmd.Parameters.AddWithValue($"@{col}", row[col] ?? DBNull.Value);

                return cmd.ExecuteNonQuery() > 0;
            }
            catch
            {
                return false;
            }
        }

        static bool UpdateRecord(OleDbConnection conn, string tableName, Dictionary<string, object> row, string pkColumn)
        {
            try
            {
                var columns = row.Keys.Where(k => k != pkColumn).ToList();
                var updateSet = string.Join(", ", columns.Select(c => $"[{c}] = ?"));
                string updateQuery = $@"UPDATE [{tableName}] SET {updateSet} WHERE [{pkColumn}] = ?";

                using var cmd = new OleDbCommand(updateQuery, conn);
                foreach (var col in columns)
                    cmd.Parameters.AddWithValue($"@{col}", row[col] ?? DBNull.Value);
                cmd.Parameters.AddWithValue($"@{pkColumn}", row[pkColumn]);

                return cmd.ExecuteNonQuery() > 0;
            }
            catch
            {
                return false;
            }
        }

        static DateTime GetLastModified(OleDbConnection conn, string tableName, string pkColumn, object pkValue)
        {
            try
            {
                string query = $"SELECT Serverzeit FROM [{tableName}] WHERE [{pkColumn}] = ?";
                using var cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue($"@{pkColumn}", pkValue);
                var result = cmd.ExecuteScalar();
                return (result != DBNull.Value && result != null) ? Convert.ToDateTime(result) : DateTime.MinValue;
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        static void DeleteRecord(OleDbConnection conn, string tableName, string pkColumn, object pkValue)
        {
            try
            {
                string query = $"DELETE FROM [{tableName}] WHERE [{pkColumn}] = ?";
                using var cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue($"@{pkColumn}", pkValue);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting record: {ex.Message}");
            }
        }
    }
}