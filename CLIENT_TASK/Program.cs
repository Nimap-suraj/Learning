using System.Text.Json;
using System.Threading;
using System.Data.OleDb;
using System.Data;
using System.Net.NetworkInformation;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace RobustAccessDbSync
{
    class SyncMetadata
    {
        public DateTime LastClientSyncTime { get; set; }
        public DateTime LastServerSyncTime { get; set; }
    }

    class Program
    {
        static string DRIVE_LETTER = "X:";
        private static bool _syncRunning = true;
        private static DateTime _lastClientSyncTime;
        private static DateTime _lastServerSyncTime;
        private const string ConflictSuffix = "_CONFLICT_RESOLVED";
        static string SERVER_IP = "95.111.230.3";
        static string SHARE_NAME = "BatFolder";
        static string USERNAME = "administrator";
        static string PASSWORD = "passwword";

        [System.Runtime.Versioning.SupportedOSPlatform("windows")]
        static async Task Main()
        {
            // Hides the blinking cursor in the console
            Console.CursorVisible = false;
            ShowGameStyleLoader("Initializing Database Synchronization Tool", 20);

            Console.WriteLine("\nDatabase Synchronization Tool");
            Console.WriteLine("-----------------------------");

            // Get user inputs
            Console.Write("Enter client database path (e.g., C:\\path\\client.mdb): ");
            string clientDbPath = Console.ReadLine();
            Console.WriteLine();

            Console.Write("Enter server database path (e.g., \\\\server\\path\\server.mdb): ");
            string serverDbPath = Console.ReadLine();
            Console.WriteLine();
            if (!HasMdbExtension(clientDbPath))
            {
                while (true)
                {
                    //Console.Write("Enter the file name on the server (e.g., C# Notes.txt): ");
                    // string fileName = serverDbPath;

                    Console.Write("Enter destination folder (e.g., C:\\DemoFiles): ");
                    string destFolder = clientDbPath;

                    if (!Directory.Exists(destFolder))
                    {
                        Console.WriteLine($"ERROR: Destination folder does not exist: {destFolder}");
                        Console.ReadKey();
                        continue;
                    }

                    // Combine folder + filename to get final local path
                    clientDbPath = Path.Combine(destFolder, Path.GetFileName(serverDbPath));


                    RunCommand($"net use {DRIVE_LETTER} /delete", false); // Clean up existing

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
                        break;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"ERROR: File copy failed. {ex.Message}");
                    }

                    RunCommand($"net use {DRIVE_LETTER} /delete", false);

                    Console.Write("Do you want to continue? Type q to quit, any other key to continue: ");
                    var input = Console.ReadLine();
                    //if (input?.ToLower() == "q")
                    //    break;
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

            }


            // Check if client DB exists, if not pull from server
            if (!File.Exists(clientDbPath))
            {
                Console.WriteLine("Client database not found. Attempting to pull from server...");
                if (await PullDatabaseFromServer(serverDbPath, clientDbPath))
                {
                    Console.WriteLine("Successfully pulled database from server to client.");
                }
                else
                {
                    Console.WriteLine("\nPress any key to exit...");
                    Console.ReadKey();
                    return;
                }
            }

            string syncMetaFile = "sync_metadata.json";

            ShowGameStyleLoader("Verifying database files", 10);
            Console.WriteLine();

            //Prevents errors due to missing or corrupted files and check both Databases arr valid or not.
            if (!VerifyDatabaseFiles(clientDbPath, serverDbPath))
            {
                Console.WriteLine("\nPress any key to exit...");
                Console.ReadKey();
                return;
            }

            //string clientConnStr = $"Provider=Microsoft.ACE.OLEDB.16.0;Data Source={clientDbPath};";
            //string serverConnStr = $"Provider=Microsoft.ACE.OLEDB.16.0;Data Source={serverDbPath};";

            string clientConnStr = $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={clientDbPath};";
            string serverConnStr = $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={serverDbPath};";


            ShowGameStyleLoader("Testing database connections", 20);
            if (!TestConnection("Client DB", clientConnStr) || !TestConnection("Server DB", serverConnStr))
            {
                Console.WriteLine("\nPress any key to exit...");
                Console.ReadKey();
                return;
            }

            // Load last sync times from file
            ShowGameStyleLoader("Loading synchronization metadata", 10);
            Console.WriteLine();

            var metadata = LoadSyncMetadata(syncMetaFile) ?? new SyncMetadata
            {
                LastClientSyncTime = DateTime.MinValue,
                LastServerSyncTime = DateTime.MinValue
            };

            _lastClientSyncTime = metadata.LastClientSyncTime;
            _lastServerSyncTime = metadata.LastServerSyncTime;

            Console.WriteLine("\nStarting continuous synchronization...");
            Console.WriteLine("Press 'Q' then Enter to stop synchronization.\n");

            var syncTask = Task.Run(() => ContinuousSync(serverConnStr, clientConnStr, syncMetaFile));

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
        public static bool HasMdbExtension(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return false;

            string extension = Path.GetExtension(path);
            return extension.Equals(".mdb", StringComparison.OrdinalIgnoreCase);
        }
        static void CreateTableFromSource(OleDbConnection sourceConn, OleDbConnection targetConn, string tableName)
        {
            try
            {
                // Get table schema from source
                DataTable schema = sourceConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables,
                    new object[] { null, null, tableName, "TABLE" });

                if (schema.Rows.Count == 0) return;

                // Get columns information
                DataTable columns = sourceConn.GetOleDbSchemaTable(OleDbSchemaGuid.Columns,
                    new object[] { null, null, tableName, null });

                // Get primary key information
                DataTable primaryKeys = sourceConn.GetOleDbSchemaTable(OleDbSchemaGuid.Primary_Keys,
                    new object[] { null, null, tableName });

                // Build CREATE TABLE statement
                var createTableSql = new StringBuilder($"CREATE TABLE [{tableName}] (");

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

                // Add LastModified column if it doesn't exist
                if (columns.Select("COLUMN_NAME = 'Serverzeit'").Length == 0)
                {
                    createTableSql.Append("[Serverzeit] DATETIME DEFAULT Now(), ");
                }

                // Remove trailing comma and close statement
                createTableSql.Length -= 2;
                createTableSql.Append(")");

                // Execute creation
                using var cmd = new OleDbCommand(createTableSql.ToString(), targetConn);
                cmd.ExecuteNonQuery();

                Console.WriteLine($"Created table {tableName} in {targetConn} database");
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

        static int GetRecordCount(string connectionString, string tableName)
        {
            try
            {
                using var conn = new OleDbConnection(connectionString);
                conn.Open();
                using var cmd = new OleDbCommand($"SELECT COUNT(*) FROM [{tableName}]", conn);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch
            {
                return -1; 
            }
        }

        static string GetPrimaryKeyColumn(string connectionString, string tableName)
        {
            try
            {
                using var conn = new OleDbConnection(connectionString);
                conn.Open();

                // Get primary keys
                DataTable schema = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Primary_Keys,
                    new object[] { null, null, tableName });

                if (schema != null && schema.Rows.Count > 0)
                {
                    // Sort by ORDINAL field to maintain key column order
                    var primaryKeyColumns = schema.AsEnumerable()
                        .OrderBy(row => row["ORDINAL"])
                        .Select(row => row["COLUMN_NAME"].ToString())
                        .FirstOrDefault();

                    // If you only want the first PK column, return primaryKeyColumns[0]
                    // Or return all as comma-separated string or list
                    return primaryKeyColumns;
                }

                if (schema.Rows.Count > 0)
                {
                    return schema.Rows[0]["COLUMN_NAME"].ToString();
                }

                // Fallback: Try to find common primary key column names
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

                return null; // No primary key or matching column found
            }
            catch
            {
                return null;
            }
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
                string fileName = Path.GetFileName(serverPath); // Extract the filename from server path

                // Verify connectivity
                if (!PingHost("127.0.0.1") || !PingHost(serverIP))
                {
                    Console.WriteLine("ERROR: Network connectivity issues");
                    return false;
                }

                // Ensure destination directory exists
                bool isClientPathDirectory = Directory.Exists(clientPath) ||
                                           (clientPath.EndsWith("\\") ||
                                            clientPath.EndsWith("/"));

                string finalClientPath;
                if (isClientPathDirectory)
                {
                    // If clientPath is a directory, combine it with the filename
                    Directory.CreateDirectory(clientPath); // Ensure directory exists
                    finalClientPath = Path.Combine(clientPath, fileName);
                }
                else
                {
                    // If clientPath includes a filename, use it directly
                    string directory = Path.GetDirectoryName(clientPath);
                    if (!string.IsNullOrEmpty(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }
                    finalClientPath = clientPath;
                }

                // Clean up any existing mapping
                await RunCommand($"net use {DRIVE_LETTER} /delete", false);

                // Mount the shared folder
                Console.WriteLine("Mounting server share...");
                string connectCmd = $"net use {DRIVE_LETTER} \\\\{serverIP}\\{shareName} /user:{USERNAME} {PASSWORD} /persistent:no";
                if (!await RunCommand(connectCmd))
                {
                    Console.WriteLine("ERROR: Failed to connect to shared folder");
                    return false;
                }

                // Verify source file exists
                string serverFile = $"{DRIVE_LETTER}\\{serverFilePath}";
                if (!File.Exists(serverFile))
                {
                    Console.WriteLine($"ERROR: File does not exist on server: {serverFilePath}");
                    await RunCommand($"net use {DRIVE_LETTER} /delete", false);
                    return false;
                }

                // Perform the copy
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
                    await RunCommand($"net use {DRIVE_LETTER} /delete", false);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error pulling database from server: {ex.Message}");
                return false;
            }
        }

     
        static async Task<bool> RunCommand(string command, bool showOutput = true)
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
                Console.ForegroundColor = ConsoleColor.Green;
                int delay = message.Contains("connection") ? 50 :
                           message.Contains("metadata") ? 30 :
                           message.Contains("structure") ? 70 : 20;
                Thread.Sleep(delay);
            }
            Console.WriteLine();
        }

        static SyncMetadata LoadSyncMetadata(string path)
        {
            if (!File.Exists(path)) return null;
            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<SyncMetadata>(json);
        }

        static void SaveSyncMetadata(string path, SyncMetadata metadata)
        {
            var json = JsonSerializer.Serialize(metadata, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json);
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

        //static DateTime GetMaxLastModified(string connectionString, string tableName)
        //{
        //    try
        //    {
        //        using var conn = new OleDbConnection(connectionString);
        //        conn.Open();
        //        using var cmd = new OleDbCommand($"SELECT MAX(Serverzeit) FROM [{tableName}]", conn);
        //        var result = cmd.ExecuteScalar();
        //        if (result != DBNull.Value && result != null)
        //            return Convert.ToDateTime(result);
        //    }
        //    catch
        //    {
        //        return DateTime.MinValue;
        //    }
        //    return DateTime.MinValue;
        //}

        static async Task ContinuousSync(string serverConnStr, string clientConnStr, string syncMetaFile)
        {
            // Get all tables from both databases
            var clientTables = GetAllTableNames(clientConnStr);
            var serverTables = GetAllTableNames(serverConnStr);

         

            // Get all unique tables (union of both)
            var allTables = clientTables.Union(serverTables, StringComparer.OrdinalIgnoreCase).ToList();

            while (_syncRunning)
            {
                foreach (var tableName in allTables)
                {
                    try
                    {
                        using var serverConn = new OleDbConnection(serverConnStr);
                        using var clientConn = new OleDbConnection(clientConnStr);
                        serverConn.Open();
                        clientConn.Open();

                        // Check if table exists in both databases
                        bool existsInClient = TableExists(clientConn, tableName);
                        bool existsInServer = TableExists(serverConn, tableName);

                        // Create table in client if it only exists in server
                        if (!existsInClient && existsInServer)
                        {
                            Console.WriteLine($"[{DateTime.Now:T}] Creating table {tableName} in client database");
                            CreateTableFromSource(serverConn, clientConn, tableName);
                            existsInClient = true;
                        }
                        // Create table in server if it only exists in client
                        else if (existsInClient && !existsInServer)
                        {
                            Console.WriteLine($"[{DateTime.Now:T}] Creating table {tableName} in server database");
                            CreateTableFromSource(clientConn, serverConn, tableName);
                            existsInServer = true;
                        }

                        //// Skip if table still doesn't exist in both
                        //if (!existsInClient || !existsInServer)
                        //{
                        //    Console.WriteLine($"[{DateTime.Now:T}] Skipping table {tableName} - creation failed");
                        //    continue;
                        //}

                        // Get primary key for this table
                        string pkColumn = GetPrimaryKeyColumn(clientConnStr, tableName) ??
                                         GetPrimaryKeyColumn(serverConnStr, tableName) ??
                                         "GUID"; // fallback

                        // First check if table has LastModified column
                        if (!TableHasColumn(serverConn, tableName, "Serverzeit"))
                        {
                            AddLastModifiedColumn(serverConn, tableName);
                        }
                        if (!TableHasColumn(clientConn, tableName, "Serverzeit"))
                        {
                            AddLastModifiedColumn(clientConn, tableName);
                        }

                        Console.WriteLine($"[{DateTime.Now:T}] Syncing table: {tableName}");

                        // Server → Client
                        int serverToClientChanges = SyncDirection(
                            sourceConn: serverConn,
                            targetConn: clientConn,
                            tableName: tableName,
                            lastSyncTime: ref _lastServerSyncTime,
                            isServerToClient: true,
                            pkColumn: pkColumn);

                        // Client → Server
                        int clientToServerChanges = SyncDirection(
                            sourceConn: clientConn,
                            targetConn: serverConn,
                            tableName: tableName,
                            lastSyncTime: ref _lastClientSyncTime,
                            isServerToClient: false,
                            pkColumn: pkColumn);

                        Console.WriteLine($"[{DateTime.Now:T}] {tableName} sync complete " +
                                        $"(Server→Client: {serverToClientChanges}, " +
                                        $"Client→Server: {clientToServerChanges})");

                        // Deletion sync
                        SyncDeletionsByComparison(serverConn, clientConn, tableName, pkColumn);
                        SyncDeletionsByComparison(clientConn, serverConn, tableName, pkColumn);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[{DateTime.Now:T}] Error syncing table {tableName}: {ex.Message}");
                    }
                }   

                // Save updated sync times
                SaveSyncMetadata(syncMetaFile, new SyncMetadata
                {
                    LastClientSyncTime = _lastClientSyncTime,
                    LastServerSyncTime = _lastServerSyncTime
                });

                await Task.Delay(5000); // Wait before next sync cycle
            }
        }
        static bool TableHasColumn(OleDbConnection conn, string tableName, string columnName)
        {
            try
            {
                DataTable schema = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Columns,
                    new object[] { null, null, tableName, null });

                foreach (DataRow row in schema.Rows)
                {
                    if (row["COLUMN_NAME"].ToString().Equals(columnName, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
                return false;
            }
            catch
            {
                return false;
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

        static void AddLastModifiedColumn(OleDbConnection conn, string tableName)
        {
            try
            {
                using var cmd = new OleDbCommand(
                    $"ALTER TABLE [{tableName}] ADD COLUMN [Serverzeit] DATETIME DEFAULT Now()", conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding Serverzeit column to {tableName}: {ex.Message}");
            }
        }

        static void SyncDeletionsByComparison(OleDbConnection sourceConn, OleDbConnection targetConn, string tableName, string pkColumn)
        {
            try
            {
                var sourceIds = GetAllIds(sourceConn, tableName, pkColumn);
                var targetIds = GetAllIds(targetConn, tableName, pkColumn);

                var idsToDelete = targetIds.Except(sourceIds);

                foreach (var id in idsToDelete)
                {
                    string deleteQuery = $"DELETE FROM [{tableName}] WHERE [{pkColumn}] = ?";
                    using var cmd = new OleDbCommand(deleteQuery, targetConn);
                    cmd.Parameters.AddWithValue($"@{pkColumn}", id);
                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                        Console.WriteLine($"Deleted ID {id} from target (not present in source)");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error syncing deletions for {tableName}: {ex.Message}");
            }
        }

        static int SyncDirection(OleDbConnection sourceConn, OleDbConnection targetConn,
                          string tableName, ref DateTime lastSyncTime,
                          bool isServerToClient, string pkColumn)
        {
            int changesApplied = 0;
            DateTime maxTimestamp = lastSyncTime;

            try
            {
                string getChangesQuery = $@"
                SELECT * FROM [{tableName}]
                WHERE Serverzeit > ?
                ORDER BY Serverzeit";

                using var cmd = new OleDbCommand(getChangesQuery, sourceConn);
                cmd.Parameters.AddWithValue("@Serverzeit", lastSyncTime);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var row = new Dictionary<string, object>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        row[reader.GetName(i)] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                    }

                    if (ApplyChangeWithConflictResolution(targetConn, tableName, row, isServerToClient, pkColumn))
                    {
                        changesApplied++;
                        var rowTimestamp = Convert.ToDateTime(row["Serverzeit"]);
                        if (rowTimestamp > maxTimestamp)
                            maxTimestamp = rowTimestamp;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in sync direction for {tableName}: {ex.Message}");
            }

            lastSyncTime = maxTimestamp;
            return changesApplied;
        }

        static bool ApplyChangeWithConflictResolution(OleDbConnection targetConn,
                                                    string tableName,
                                                    Dictionary<string, object> row,
                                                    bool isServerToClient,
                                                    string pkColumn)
        {
            try
            {
                var pkValue = row[pkColumn];
                var incomingLastModified = Convert.ToDateTime(row["Serverzeit"]);

                bool exists = RecordExists(targetConn, tableName, pkColumn, pkValue);
                //Console.WriteLine("present or not yes/No",exists);
                if (!exists)
                    return InsertRecord(targetConn, tableName, row);

                var targetLastModified = GetLastModified(targetConn, tableName, pkColumn, pkValue);
                //Console.WriteLine("targetLastModified"+ targetLastModified);

                //Console.WriteLine(pkColumn);
                //Console.WriteLine(pkValue);
                //Console.WriteLine(tableName);
                //Console.WriteLine(targetConn);
                var targetRecord = GetRecord(targetConn, tableName, pkColumn, pkValue);
                //Console.WriteLine("targetRecord" + targetRecord);

                // Simple conflict resolution - server wins
                if (isServerToClient)
                {
                    bool dataIsDifferent = !row[$"{pkColumn}"].Equals(targetRecord[$"{pkColumn}"]);
                    if (dataIsDifferent)
                    {
                        Console.WriteLine($"Overwriting client data for ID {pkValue} with server version");
                    }
                    return UpdateRecord(targetConn, tableName, row, pkColumn);
                }
                else
                {
                    // For client to server, only update if client has newer version
                    if (incomingLastModified > targetLastModified)
                    {
                        return UpdateRecord(targetConn, tableName, row, pkColumn);
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error applying change to {tableName}: {ex.Message}");
                return false;
            }
        }

        static List<Guid> GetAllIds(OleDbConnection conn, string tableName, string pkColumn)
        {
            var ids = new List<Guid>();
            try
            {
                string query = $"SELECT [{pkColumn}] FROM [{tableName}]";

                using var cmd = new OleDbCommand(query, conn);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ids.Add(reader.GetGuid(0));
                }
            }
            catch
            {
                // Return empty list if error occurs
            }
            return ids;
        }

        static Dictionary<string, object> GetRecord(OleDbConnection conn, string tableName, string pkColumn, object pkValue)
        {
            Console.WriteLine("getRecord method started");
            var record = new Dictionary<string, object>();
            try
            {
                string query = $"SELECT * FROM [{tableName}] WHERE [{pkColumn}] = ?";
                //Console.WriteLine(query);
                using var cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue($"@{pkColumn}", pkValue);

                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        //Console.WriteLine(reader.FieldCount);
                        //Console.WriteLine(reader.IsDBNull(i) ? null : reader.GetValue(i));
                        record[reader.GetName(i)] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                    }
                }
            }
            catch
            {
                // Return empty dictionary if error occurs
            }
            //foreach (KeyValuePair<string, object> item in record)
            //{
            //    Console.WriteLine($"{item.Key}   {item.Value}");
            //}
            return record;
        }

        static bool UpdateRecord(OleDbConnection conn, string tableName, Dictionary<string, object> row, string pkColumn)
        {
            try
            {
                Console.WriteLine("enter update record");
                var columns = row.Keys.Where(k => k != pkColumn).ToList();
                var updateSet = string.Join(", ", columns.Select(c => $"[{c}] = ?"));
                string updateQuery = $@"UPDATE [{tableName}] SET {updateSet} WHERE [{pkColumn}] = ?";

                using var cmd = new OleDbCommand(updateQuery, conn);
                //foreach (var item in row)
                //{
                //    Console.WriteLine($"Key: {item.Key} Value: {item.Value}");
                //}
                foreach (var col in columns)
                {
                    //Console.WriteLine($"coumun: {col}");
                    cmd.Parameters.AddWithValue($"@{col}", row[col] ?? DBNull.Value);
                }
                cmd.Parameters.AddWithValue($"@{pkColumn}", row[pkColumn]);

                return cmd.ExecuteNonQuery() > 0;
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
    }
}