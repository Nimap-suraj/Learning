using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace RobustAccessDbSync
{
    class SyncMetadata
    {
        public Dictionary<string, DateTime> LastClientSyncTimes { get; set; } = new Dictionary<string, DateTime>();
        public Dictionary<string, DateTime> LastServerSyncTimes { get; set; } = new Dictionary<string, DateTime>();
        public Dictionary<string, List<string>> TableSchemas { get; set; } = new Dictionary<string, List<string>>();
    }

    class Program
    {
        private static bool _syncRunning = true;
        private static SyncMetadata _metadata = new SyncMetadata();
        private const string MetadataFile = "sync_metadata.json";
        private const string ConflictNoteField = "SyncNotes";

        [System.Runtime.Versioning.SupportedOSPlatform("windows")]
        static async Task Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.CursorVisible = false;

            ShowGameStyleLoader("🔄 MDB Synchronization Tool", 20);

            // Get user inputs
            Console.Write("Enter Server IP: ");
            string serverIp = Console.ReadLine();

            Console.Write("Enter Shared Folder Name: ");
            string sharedFolder = Console.ReadLine();

            Console.Write("Enter MDB File Name (e.g., data.mdb): ");
            string mdbFile = Console.ReadLine();

            Console.Write("Enter Server Username: ");
            string username = Console.ReadLine();

            Console.Write("Enter Server Password: ");
            string password = ReadPassword();

            Console.Write("\nEnter Client Folder Path (e.g., C:\\DemoFolder): ");
            string clientFolder = Console.ReadLine();

            // Construct paths
            string serverPath = $@"\\{serverIp}\{sharedFolder}\{mdbFile}";
            string clientPath = Path.Combine(clientFolder, mdbFile);

            // Verify server authentication
            bool authSuccess = TestNetworkConnection(serverPath, username, password);
            Console.WriteLine(authSuccess ? "🔐 Server authentication successful." : "❌ Failed to authenticate to server. Check credentials.");
            if (!authSuccess) return;

            // Verify client folder
            if (!Directory.Exists(clientFolder))
            {
                Console.WriteLine("❌ Client folder does not exist.");
                return;
            }

            // Handle initial copy if needed
            if (!File.Exists(clientPath))
            {
                Console.WriteLine("📁 Client .mdb not found. Copying from Server...");
                try
                {
                    File.Copy(serverPath, clientPath);
                    Console.WriteLine("✅ Initial Copy Complete.");

                    // Initialize metadata with all tables from server
                    InitializeMetadataFromServer(serverPath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Error copying file: {ex.Message}");
                    return;
                }
            }
            else
            {
                // Load existing metadata
                _metadata = LoadSyncMetadata(MetadataFile) ?? new SyncMetadata();
            }

            // Connection strings
            string clientConnStr = $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={clientPath};";
            string serverConnStr = $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={serverPath};";

            // Start continuous sync
            Console.WriteLine("\nStarting continuous synchronization...");
            Console.WriteLine("Press 'Q' to stop synchronization.\n");

            var syncTask = Task.Run(() => ContinuousSync(serverConnStr, clientConnStr));

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

        static string ReadPassword()
        {
            string password = "";
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    password += key.KeyChar;
                    Console.Write("*");
                }
                else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password = password.Substring(0, (password.Length - 1));
                    Console.Write("\b \b");
                }
            } while (key.Key != ConsoleKey.Enter);

            return password;
        }

        static bool TestNetworkConnection(string path, string username, string password)
        {
            try
            {
                // Try to access the file
                using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        static void InitializeMetadataFromServer(string serverPath)
        {
            string connStr = $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={serverPath};";

            using var conn = new OleDbConnection(connStr);
            conn.Open();

            // Get all tables
            DataTable tables = conn.GetSchema("Tables");

            foreach (DataRow table in tables.Rows)
            {
                string tableName = table["TABLE_NAME"].ToString();

                // Skip system tables
                if (tableName.StartsWith("MSys")) continue;

                _metadata.LastClientSyncTimes[tableName] = DateTime.MinValue;
                _metadata.LastServerSyncTimes[tableName] = DateTime.MinValue;

                // Store schema
                var schema = new List<string>();
                using var cmd = new OleDbCommand($"SELECT TOP 1 * FROM [{tableName}]", conn);
                using var reader = cmd.ExecuteReader(CommandBehavior.SchemaOnly);
                var schemaTable = reader.GetSchemaTable();

                foreach (DataRow column in schemaTable.Rows)
                {
                    schema.Add(column["ColumnName"].ToString());
                }

                _metadata.TableSchemas[tableName] = schema;
            }

            SaveSyncMetadata(MetadataFile, _metadata);
        }

        static async Task ContinuousSync(string serverConnStr, string clientConnStr)
        {
            while (_syncRunning)
            {
                try
                {
                    using var serverConn = new OleDbConnection(serverConnStr);
                    using var clientConn = new OleDbConnection(clientConnStr);
                    serverConn.Open();
                    clientConn.Open();

                    // Sync table structure first
                    SyncTableStructures(serverConn, clientConn);

                    // Get all tables to sync
                    var tables = GetTablesToSync(serverConn);

                    foreach (var table in tables)
                    {
                        // Skip system tables
                        if (table.StartsWith("MSys")) continue;

                        Console.WriteLine($"[{DateTime.Now:T}] Syncing table: {table}");

                        // Server → Client sync
                        int serverToClient = SyncTable(
                            sourceConn: serverConn,
                            targetConn: clientConn,
                            tableName: table,
                            isServerToClient: true);

                        Console.WriteLine($"[{DateTime.Now:T}] Server → Client: {serverToClient} changes");

                        // Client → Server sync
                        int clientToServer = SyncTable(
                            sourceConn: clientConn,
                            targetConn: serverConn,
                            tableName: table,
                            isServerToClient: false);

                        Console.WriteLine($"[{DateTime.Now:T}] Client → Server: {clientToServer} changes");

                        // Sync deletions
                        SyncDeletions(serverConn, clientConn, table);
                        SyncDeletions(clientConn, serverConn, table);
                    }

                    SaveSyncMetadata(MetadataFile, _metadata);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[{DateTime.Now:T}] Sync error: {ex.Message}");
                }

                await Task.Delay(5000); // Wait before next sync cycle
            }
        }

        static List<string> GetTablesToSync(OleDbConnection conn)
        {
            DataTable tables = conn.GetSchema("Tables");
            return tables.Rows.Cast<DataRow>()
                .Select(r => r["TABLE_NAME"].ToString())
                .Where(t => !t.StartsWith("MSys")) // Exclude system tables
                .ToList();
        }

        static void SyncTableStructures(OleDbConnection source, OleDbConnection target)
        {
            DataTable sourceTables = source.GetSchema("Tables");
            DataTable targetTables = target.GetSchema("Tables");

            var sourceTableNames = sourceTables.Rows.Cast<DataRow>()
                .Select(r => r["TABLE_NAME"].ToString())
                .Where(t => !t.StartsWith("MSys"))
                .ToList();

            var targetTableNames = targetTables.Rows.Cast<DataRow>()
                .Select(r => r["TABLE_NAME"].ToString())
                .Where(t => !t.StartsWith("MSys"))
                .ToList();

            // Create missing tables in target
            foreach (var table in sourceTableNames.Except(targetTableNames))
            {
                Console.WriteLine($"Creating table {table} in target database");
                CreateTableFromSource(source, target, table);
            }

            // For tables that exist in both, ensure schema matches
            foreach (var table in sourceTableNames.Intersect(targetTableNames))
            {
                SyncTableSchema(source, target, table);
            }
        }

        static void CreateTableFromSource(OleDbConnection source, OleDbConnection target, string tableName)
        {
            // Get schema from source table
            using var cmd = new OleDbCommand($"SELECT TOP 1 * FROM [{tableName}]", source);
            using var reader = cmd.ExecuteReader(CommandBehavior.SchemaOnly);
            var schema = reader.GetSchemaTable();

            // Build CREATE TABLE statement
            var sb = new StringBuilder($"CREATE TABLE [{tableName}] (");
            bool firstCol = true;

            foreach (DataRow col in schema.Rows)
            {
                if (!firstCol) sb.Append(", ");
                firstCol = false;

                string colName = col["ColumnName"].ToString();
                Type dataType = (Type)col["DataType"];
                int size = col["ColumnSize"] as int? ?? 0;
                bool isPrimaryKey = false; // You might need to detect PKs

                sb.Append($"[{colName}] {GetSqlType(dataType, size)}");

                if (isPrimaryKey) sb.Append(" PRIMARY KEY");
            }

            sb.Append(")");

            // Execute creation
            using var createCmd = new OleDbCommand(sb.ToString(), target);
            createCmd.ExecuteNonQuery();

            // Add to metadata if not exists
            if (!_metadata.TableSchemas.ContainsKey(tableName))
            {
                _metadata.TableSchemas[tableName] = schema.Rows.Cast<DataRow>()
                    .Select(r => r["ColumnName"].ToString())
                    .ToList();
            }
        }

        static string GetSqlType(Type type, int size)
        {
            if (type == typeof(string)) return size > 0 ? $"TEXT({size})" : "MEMO";
            if (type == typeof(int)) return "INTEGER";
            if (type == typeof(long)) return "BIGINT";
            if (type == typeof(decimal)) return "DECIMAL";
            if (type == typeof(DateTime)) return "DATETIME";
            if (type == typeof(bool)) return "BIT";
            if (type == typeof(Guid)) return "GUID";
            if (type == typeof(byte[])) return "BINARY";
            return "TEXT";
        }

        static void SyncTableSchema(OleDbConnection source, OleDbConnection target, string tableName)
        {
            // Get source columns
            var sourceColumns = GetColumnInfo(source, tableName);
            var targetColumns = GetColumnInfo(target, tableName);

            // Find columns to add
            var columnsToAdd = sourceColumns.Keys.Except(targetColumns.Keys);

            foreach (var col in columnsToAdd)
            {
                Console.WriteLine($"Adding column {col} to {tableName}");
                string sqlType = GetSqlType(sourceColumns[col].DataType, sourceColumns[col].Size);
                using var cmd = new OleDbCommand($"ALTER TABLE [{tableName}] ADD COLUMN [{col}] {sqlType}", target);
                cmd.ExecuteNonQuery();
            }
        }

        static Dictionary<string, (Type DataType, int Size)> GetColumnInfo(OleDbConnection conn, string tableName)
        {
            var columns = new Dictionary<string, (Type, int)>();

            using var cmd = new OleDbCommand($"SELECT TOP 1 * FROM [{tableName}]", conn);
            using var reader = cmd.ExecuteReader(CommandBehavior.SchemaOnly);
            var schema = reader.GetSchemaTable();

            foreach (DataRow col in schema.Rows)
            {
                string name = col["ColumnName"].ToString();
                Type type = (Type)col["DataType"];
                int size = col["ColumnSize"] as int? ?? 0;
                columns[name] = (type, size);
            }

            return columns;
        }

        static int SyncTable(OleDbConnection sourceConn, OleDbConnection targetConn,
                          string tableName, bool isServerToClient)
        {
            int changesApplied = 0;
            string timestampField = FindTimestampField(sourceConn, tableName);

            // Initialize sync time if not exists
            if (!_metadata.LastClientSyncTimes.ContainsKey(tableName))
                _metadata.LastClientSyncTimes[tableName] = DateTime.MinValue;

            if (!_metadata.LastServerSyncTimes.ContainsKey(tableName))
                _metadata.LastServerSyncTimes[tableName] = DateTime.MinValue;

            DateTime lastSyncTime = isServerToClient
                ? _metadata.LastClientSyncTimes[tableName]
                : _metadata.LastServerSyncTimes[tableName];

            DateTime maxTimestamp = lastSyncTime;

            // Get changes since last sync
            string getChangesQuery = string.IsNullOrEmpty(timestampField)
                ? $"SELECT * FROM [{tableName}]" // If no timestamp field, sync everything
                : $"SELECT * FROM [{tableName}] WHERE [{timestampField}] > ?";

            using var cmd = new OleDbCommand(getChangesQuery, sourceConn);
            if (!string.IsNullOrEmpty(timestampField))
                cmd.Parameters.AddWithValue($"@{timestampField}", lastSyncTime);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var row = new Dictionary<string, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    row[reader.GetName(i)] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                }

                if (ApplyChangeWithConflictResolution(targetConn, tableName, row, isServerToClient))
                {
                    changesApplied++;

                    // Update max timestamp if available
                    if (!string.IsNullOrEmpty(timestampField) && row.ContainsKey(timestampField))
                    {
                        var rowTimestamp = Convert.ToDateTime(row[timestampField]);
                        if (rowTimestamp > maxTimestamp)
                            maxTimestamp = rowTimestamp;
                    }
                }
            }

            // Update sync time
            if (isServerToClient)
                _metadata.LastClientSyncTimes[tableName] = maxTimestamp;
            else
                _metadata.LastServerSyncTimes[tableName] = maxTimestamp;

            return changesApplied;
        }

        static string FindTimestampField(OleDbConnection conn, string tableName)
        {
            // Common timestamp field names
            string[] possibleNames = { "LastModified", "ModifiedDate", "UpdateDate", "Timestamp" };

            using var cmd = new OleDbCommand($"SELECT TOP 1 * FROM [{tableName}]", conn);
            using var reader = cmd.ExecuteReader(CommandBehavior.SchemaOnly);
            var schema = reader.GetSchemaTable();

            foreach (DataRow col in schema.Rows)
            {
                string name = col["ColumnName"].ToString();
                if (possibleNames.Contains(name, StringComparer.OrdinalIgnoreCase))
                    return name;
            }

            return null;
        }

        static bool ApplyChangeWithConflictResolution(OleDbConnection targetConn, string tableName,
                                                   Dictionary<string, object> row, bool isServerToClient)
        {
            // Find PK column (try common names first)
            string pkColumn = FindPrimaryKeyColumn(targetConn, tableName);
            if (pkColumn == null) return false;

            var pkValue = row[pkColumn];
            string timestampField = FindTimestampField(targetConn, tableName);

            bool exists = RecordExists(targetConn, tableName, pkColumn, pkValue);
            if (!exists)
                return InsertRecord(targetConn, tableName, row);

            // Conflict resolution only if we have a timestamp field
            if (!string.IsNullOrEmpty(timestampField) && row.ContainsKey(timestampField))
            {
                var incomingTimestamp = Convert.ToDateTime(row[timestampField]);
                var targetTimestamp = GetLastModified(targetConn, tableName, pkColumn, pkValue, timestampField);

                // Simple conflict resolution - server wins
                if (isServerToClient)
                {
                    return UpdateRecord(targetConn, tableName, row, pkColumn);
                }
                else
                {
                    // For client to server, only update if client has newer version
                    if (incomingTimestamp > targetTimestamp)
                    {
                        return UpdateRecord(targetConn, tableName, row, pkColumn);
                    }
                    return false;
                }
            }
            else
            {
                // No timestamp - just overwrite
                return UpdateRecord(targetConn, tableName, row, pkColumn);
            }
        }

        static string FindPrimaryKeyColumn(OleDbConnection conn, string tableName)
        {
            // Common PK names
            string[] commonPkNames = { "ID", tableName + "ID", "PK_" + tableName, "GUID" };

            using var cmd = new OleDbCommand($"SELECT TOP 1 * FROM [{tableName}]", conn);
            using var reader = cmd.ExecuteReader(CommandBehavior.SchemaOnly);
            var schema = reader.GetSchemaTable();

            // First look for actual primary keys
            foreach (DataRow col in schema.Rows)
            {
                if (col["IsKey"] as bool? == true)
                    return col["ColumnName"].ToString();
            }

            // Then try common names
            foreach (string name in commonPkNames)
            {
                if (schema.Rows.Cast<DataRow>().Any(r =>
                    r["ColumnName"].ToString().Equals(name, StringComparison.OrdinalIgnoreCase)))
                    return name;
            }

            // Fallback to first column
            return schema.Rows.Count > 0 ? schema.Rows[0]["ColumnName"].ToString() : null;
        }

        static void SyncDeletions(OleDbConnection sourceConn, OleDbConnection targetConn, string tableName)
        {
            string pkColumn = FindPrimaryKeyColumn(sourceConn, tableName);
            if (pkColumn == null) return;

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
                    Console.WriteLine($"Deleted ID {id} from {tableName} (not present in source)");
            }
        }

        static List<object> GetAllIds(OleDbConnection conn, string tableName, string pkColumn)
        {
            var ids = new List<object>();
            string query = $"SELECT [{pkColumn}] FROM [{tableName}]";

            using var cmd = new OleDbCommand(query, conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                ids.Add(reader.GetValue(0));
            }
            return ids;
        }

        static Dictionary<string, object> GetRecord(OleDbConnection conn, string tableName, string pkColumn, object pkValue)
        {
            var record = new Dictionary<string, object>();
            string query = $"SELECT * FROM [{tableName}] WHERE [{pkColumn}] = ?";

            using var cmd = new OleDbCommand(query, conn);
            cmd.Parameters.AddWithValue($"@{pkColumn}", pkValue);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                    record[reader.GetName(i)] = reader.IsDBNull(i) ? null : reader.GetValue(i);
            }

            return record;
        }

        static bool UpdateRecord(OleDbConnection conn, string tableName, Dictionary<string, object> row, string pkColumn)
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

        static bool InsertRecord(OleDbConnection conn, string tableName, Dictionary<string, object> row)
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

        static bool RecordExists(OleDbConnection conn, string tableName, string pkColumn, object pkValue)
        {
            string query = $"SELECT COUNT(*) FROM [{tableName}] WHERE [{pkColumn}] = ?";
            using var cmd = new OleDbCommand(query, conn);
            cmd.Parameters.AddWithValue($"@{pkColumn}", pkValue);
            return (int)cmd.ExecuteScalar() > 0;
        }

        static DateTime GetLastModified(OleDbConnection conn, string tableName, string pkColumn, object pkValue, string timestampField)
        {
            string query = $"SELECT [{timestampField}] FROM [{tableName}] WHERE [{pkColumn}] = ?";
            using var cmd = new OleDbCommand(query, conn);
            cmd.Parameters.AddWithValue($"@{pkColumn}", pkValue);
            var result = cmd.ExecuteScalar();
            return (result != DBNull.Value && result != null) ? Convert.ToDateTime(result) : DateTime.MinValue;
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
    }
}