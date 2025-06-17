using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace CombinedFileTransferAndDbSync
{
    // Configuration for file transfer
    public class FileTransferConfig
    {
        public string ServerIP { get; set; } = "95.111.230.3";
        public string ShareName { get; set; } = "BatFolder";
        public string Username { get; set; } = "administrator";
        public string Password { get; set; } = "N1m@p2025$Server";
        public string DriveLetter { get; set; } = "X:";
    }

    // Metadata for database synchronization
    public class SyncMetadata
    {
        public DateTime LastClientSyncTime { get; set; }
        public DateTime LastServerSyncTime { get; set; }
    }

    class Program
    {
        // File transfer configuration
        private static FileTransferConfig _fileConfig = new FileTransferConfig();

        // Database sync control
        private static bool _syncRunning = true;
        private static DateTime _lastClientSyncTime;
        private static DateTime _lastServerSyncTime;
        private const string ConflictSuffix = "_CONFLICT_RESOLVED";

        static async Task Main()
        {
            Console.Title = "Combined File Transfer and Database Sync Tool";
            Console.CursorVisible = false;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Main Menu");
                Console.WriteLine("---------");
                Console.WriteLine("1. File Transfer Utility");
                Console.WriteLine("2. Database Synchronization");
                Console.WriteLine("3. Exit");
                Console.Write("\nSelect an option: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await RunFileTransfer();
                        break;
                    case "2":
                        await RunDatabaseSync();
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Press any key to continue...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        #region File Transfer Methods
        static async Task RunFileTransfer()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("File Transfer Utility");
                Console.WriteLine("---------------------");
                Console.WriteLine($"Current configuration:");
                Console.WriteLine($"- Server: \\\\{_fileConfig.ServerIP}\\{_fileConfig.ShareName}");
                Console.WriteLine($"- Username: {_fileConfig.Username}");
                Console.WriteLine($"- Drive letter: {_fileConfig.DriveLetter}");
                Console.WriteLine("\n1. Download file from server");
                Console.WriteLine("2. Upload file to server");
                Console.WriteLine("3. Configure settings");
                Console.WriteLine("4. Return to main menu");
                Console.Write("\nSelect an option: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await TransferFileFromServer();
                        break;
                    case "2":
                        await PushFileToServer();
                        break;
                    case "3":
                        ConfigureFileTransferSettings();
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Press any key to continue...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static async Task TransferFileFromServer()
        {
            Console.Clear();
            Console.WriteLine("Download File from Server");
            Console.WriteLine("------------------------");

            Console.Write("Enter the file name on the server (e.g., C# Notes.txt): ");
            string fileName = Console.ReadLine();

            Console.Write("Enter destination folder (e.g., C:\\DemoFiles): ");
            string destFolder = Console.ReadLine();

            if (!Directory.Exists(destFolder))
            {
                Console.WriteLine($"ERROR: Destination folder does not exist: {destFolder}");
                Console.ReadKey();
                return;
            }

            string localDestPath = Path.Combine(destFolder, Path.GetFileName(fileName));

            if (!MountNetworkDrive())
            {
                Console.ReadKey();
                return;
            }

            string serverFilePath = Path.Combine(_fileConfig.DriveLetter, fileName);

            if (!File.Exists(serverFilePath))
            {
                Console.WriteLine($"ERROR: File does not exist on server: {fileName}");
                UnmountNetworkDrive();
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Copying file from server...");
            try
            {
                File.Copy(serverFilePath, localDestPath, true);
                Console.WriteLine($"File successfully downloaded to: {localDestPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: File download failed. {ex.Message}");
            }

            UnmountNetworkDrive();
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        static async Task PushFileToServer()
        {
            Console.Clear();
            Console.WriteLine("Upload File to Server");
            Console.WriteLine("---------------------");

            Console.Write("Enter the local file path to upload (e.g., C:\\Files\\notes.txt): ");
            string localFilePath = Console.ReadLine();

            if (!File.Exists(localFilePath))
            {
                Console.WriteLine($"ERROR: File does not exist: {localFilePath}");
                Console.ReadKey();
                return;
            }

            Console.Write("Enter destination folder on server (e.g., Uploads): ");
            string serverFolder = Console.ReadLine();

            if (!MountNetworkDrive())
            {
                Console.ReadKey();
                return;
            }

            string serverDestPath = Path.Combine(_fileConfig.DriveLetter, serverFolder, Path.GetFileName(localFilePath));

            try
            {
                // Ensure server directory exists
                Directory.CreateDirectory(Path.GetDirectoryName(serverDestPath));

                Console.WriteLine("Uploading file to server...");
                File.Copy(localFilePath, serverDestPath, true);
                Console.WriteLine($"File successfully uploaded to: {serverDestPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: File upload failed. {ex.Message}");
            }

            UnmountNetworkDrive();
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        static bool MountNetworkDrive()
        {
            RunCommand($"net use {_fileConfig.DriveLetter} /delete", false); // Clean up existing

            Console.WriteLine("Mounting shared folder...");
            var connectCmd = $"net use {_fileConfig.DriveLetter} \\\\{_fileConfig.ServerIP}\\{_fileConfig.ShareName} /user:{_fileConfig.Username} {_fileConfig.Password} /persistent:no";
            if (!RunCommand(connectCmd))
            {
                Console.WriteLine("ERROR: Failed to connect to shared folder.");
                return false;
            }
            return true;
        }

        static void UnmountNetworkDrive()
        {
            RunCommand($"net use {_fileConfig.DriveLetter} /delete", false);
        }

        static void ConfigureFileTransferSettings()
        {
            Console.Clear();
            Console.WriteLine("Configure File Transfer Settings");
            Console.WriteLine("--------------------------------");

            Console.Write($"Server IP [{_fileConfig.ServerIP}]: ");
            var input = Console.ReadLine();
            if (!string.IsNullOrEmpty(input)) _fileConfig.ServerIP = input;

            Console.Write($"Share Name [{_fileConfig.ShareName}]: ");
            input = Console.ReadLine();
            if (!string.IsNullOrEmpty(input)) _fileConfig.ShareName = input;

            Console.Write($"Username [{_fileConfig.Username}]: ");
            input = Console.ReadLine();
            if (!string.IsNullOrEmpty(input)) _fileConfig.Username = input;

            Console.Write($"Password [{_fileConfig.Password}]: ");
            input = Console.ReadLine();
            if (!string.IsNullOrEmpty(input)) _fileConfig.Password = input;

            Console.Write($"Drive Letter [{_fileConfig.DriveLetter}]: ");
            input = Console.ReadLine();
            if (!string.IsNullOrEmpty(input)) _fileConfig.DriveLetter = input;

            Console.WriteLine("\nSettings updated. Press any key to continue...");
            Console.ReadKey();
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
        #endregion

        #region Database Sync Methods
        static async Task RunDatabaseSync()
        {
            Console.Clear();
            Console.CursorVisible = false;
            ShowGameStyleLoader("Initializing Database Synchronization Tool", 20);

            Console.WriteLine("\nDatabase Synchronization Tool");
            Console.WriteLine("-----------------------------");

            // Get user inputs
            Console.Write(@"Enter client database path (e.g., C:\path\client.mdb): ");
            string clientDbPath = Console.ReadLine();
            Console.WriteLine();

            Console.Write(@"Enter server database path (e.g., \\server\path\server.mdb): ");
            string serverDbPath = Console.ReadLine();
            Console.WriteLine();

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

            Console.Write("Enter table name to synchronize (default: Employee): ");
            string tableName = Console.ReadLine();
            if (string.IsNullOrEmpty(tableName)) tableName = "Employee";
            Console.WriteLine();

            Console.Write("Enter primary key column name (default: ID): ");
            string pkColumn = Console.ReadLine();
            if (string.IsNullOrEmpty(pkColumn)) pkColumn = "ID";
            Console.WriteLine();

            Console.WriteLine("\nSync Options:");
            Console.WriteLine("1. Continuous bidirectional sync (default)");
            Console.WriteLine("2. Push client changes to server only");
            Console.WriteLine("3. Pull server changes to client only");
            Console.Write("\nSelect sync mode: ");
            string syncMode = Console.ReadLine();

            string syncMetaFile = "sync_metadata.json";

            ShowGameStyleLoader("Verifying database files", 10);
            Console.WriteLine();

            if (!VerifyDatabaseFiles(clientDbPath, serverDbPath))
            {
                Console.WriteLine("\nPress any key to exit...");
                Console.ReadKey();
                return;
            }

            string clientConnStr = $"Provider=Microsoft.ACE.OLEDB.16.0;Data Source={clientDbPath};";
            string serverConnStr = $"Provider=Microsoft.ACE.OLEDB.16.0;Data Source={serverDbPath};";

            ShowGameStyleLoader("Testing database connections", 20);
            if (!TestConnection("Client DB", clientConnStr) || !TestConnection("Server DB", serverConnStr))
            {
                Console.WriteLine("\nPress any key to exit...");
                Console.ReadKey();
                return;
            }

            // Create table if it doesn't exist in both databases
            ShowGameStyleLoader("Ensuring table structure exists", 30);
            Console.WriteLine();

            EnsureTableExists(clientConnStr, tableName, pkColumn, "client");
            EnsureTableExists(serverConnStr, tableName, pkColumn, "server");

            // Load last sync times from file
            ShowGameStyleLoader("Loading synchronization metadata", 10);
            Console.WriteLine();

            var metadata = LoadSyncMetadata(syncMetaFile) ?? new SyncMetadata
            {
                LastClientSyncTime = GetMaxLastModified(serverConnStr, tableName),
                LastServerSyncTime = GetMaxLastModified(clientConnStr, tableName)
            };

            _lastClientSyncTime = metadata.LastClientSyncTime;
            _lastServerSyncTime = metadata.LastServerSyncTime;

            Console.WriteLine("\nStarting synchronization...");
            Console.WriteLine("Press 'Q' then Enter to stop synchronization.\n");

            var syncTask = Task.Run(() => ContinuousSync(serverConnStr, clientConnStr, tableName, pkColumn, syncMetaFile, syncMode));

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
            Console.WriteLine("\nSynchronization stopped. Press any key to continue.");
            Console.CursorVisible = true;
            Console.ReadKey();
        }

        static async Task<bool> PullDatabaseFromServer(string serverPath, string clientPath)
        {
            try
            {
                Console.WriteLine($"Attempting to copy from {serverPath} to {clientPath}");

                // Ensure target directory exists
                Directory.CreateDirectory(Path.GetDirectoryName(clientPath));

                // Use File.Copy for simple copy operation
                File.Copy(serverPath, clientPath, false);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error copying database: {ex.Message}");
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

        static void EnsureTableExists(string connectionString, string tableName, string pkColumn, string system)
        {
            try
            {
                using var conn = new OleDbConnection(connectionString);
                conn.Open();

                DataTable tables = conn.GetSchema("Tables");
                bool tableExists = false;

                foreach (DataRow row in tables.Rows)
                {
                    if (row["TABLE_NAME"].ToString().Equals(tableName, StringComparison.OrdinalIgnoreCase))
                    {
                        tableExists = true;
                        break;
                    }
                }

                if (!tableExists)
                {
                    Console.WriteLine($"Table '{tableName}' not found. Creating...");

                    using var cmd = new OleDbCommand($@"
                    CREATE TABLE [{tableName}] (
                        [{pkColumn}] GUID PRIMARY KEY,
                        [Name] TEXT(255),
                        [LastModified] DATETIME DEFAULT Now(),
                        [Notes] MEMO
                    )", conn);

                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Table created successfully.");
                }
                else
                {
                    Console.WriteLine($"Table '{tableName}' already exists in {system}.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError ensuring table exists: {ex.Message}");
            }
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

        static DateTime GetMaxLastModified(string connectionString, string tableName)
        {
            try
            {
                using var conn = new OleDbConnection(connectionString);
                conn.Open();
                using var cmd = new OleDbCommand($"SELECT MAX(LastModified) FROM [{tableName}]", conn);
                var result = cmd.ExecuteScalar();
                if (result != DBNull.Value && result != null)
                    return Convert.ToDateTime(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError fetching max LastModified: {ex.Message}");
            }
            return DateTime.MinValue;
        }

        static async Task ContinuousSync(string serverConnStr, string clientConnStr, string tableName, string pkColumn, string syncMetaFile, string syncMode)
        {
            while (_syncRunning)
            {
                try
                {
                    using var serverConn = new OleDbConnection(serverConnStr);
                    using var clientConn = new OleDbConnection(clientConnStr);
                    serverConn.Open();
                    clientConn.Open();

                    // Handle different sync modes
                    switch (syncMode)
                    {
                        case "2": // Push only (client → server)
                            Console.WriteLine($"[{DateTime.Now:T}] Pushing Client → Server...");
                            int clientToServerChanges = SyncDirection(
                                sourceConn: clientConn,
                                targetConn: serverConn,
                                tableName: tableName,
                                lastSyncTime: ref _lastClientSyncTime,
                                isServerToClient: false,
                                pkColumn: pkColumn);
                            Console.WriteLine($"[{DateTime.Now:T}] Client → Server: {clientToServerChanges} changes pushed");
                            break;

                        case "3": // Pull only (server → client)
                            Console.WriteLine($"[{DateTime.Now:T}] Pulling Server → Client...");
                            int serverToClientChanges = SyncDirection(
                                sourceConn: serverConn,
                                targetConn: clientConn,
                                tableName: tableName,
                                lastSyncTime: ref _lastServerSyncTime,
                                isServerToClient: true,
                                pkColumn: pkColumn);
                            Console.WriteLine($"[{DateTime.Now:T}] Server → Client: {serverToClientChanges} changes pulled");
                            break;

                        default: // Bidirectional sync
                            Console.WriteLine($"[{DateTime.Now:T}] Syncing Server → Client...");
                            int serverToClientChanges1 = SyncDirection(
                                sourceConn: serverConn,
                                targetConn: clientConn,
                                tableName: tableName,
                                lastSyncTime: ref _lastServerSyncTime,
                                isServerToClient: true,
                                pkColumn: pkColumn);
                            Console.WriteLine($"[{DateTime.Now:T}] Server → Client: {serverToClientChanges1} changes applied");

                            Console.WriteLine($"[{DateTime.Now:T}] Syncing Client → Server...");
                            int clientToServerChanges1 = SyncDirection(
                                sourceConn: clientConn,
                                targetConn: serverConn,
                                tableName: tableName,
                                lastSyncTime: ref _lastClientSyncTime,
                                isServerToClient: false,
                                pkColumn: pkColumn);
                            Console.WriteLine($"[{DateTime.Now:T}] Client → Server: {clientToServerChanges1} changes applied");
                            break;
                    }

                    // Save updated sync times
                    SaveSyncMetadata(syncMetaFile, new SyncMetadata
                    {
                        LastClientSyncTime = _lastClientSyncTime,
                        LastServerSyncTime = _lastServerSyncTime
                    });

                    // Wait 10 seconds before running deletion sync (only in bidirectional mode)
                    if (syncMode != "2" && syncMode != "3")
                    {
                        await Task.Delay(10000);

                        // Deletion sync: Remove missing IDs from both sides
                        Console.WriteLine($"[{DateTime.Now:T}] Checking for deletions...");
                        SyncDeletionsByComparison(serverConn, clientConn, tableName, pkColumn); // server → client
                        SyncDeletionsByComparison(clientConn, serverConn, tableName, pkColumn); // client → server
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[{DateTime.Now:T}] Sync error: {ex.Message}");
                }

                await Task.Delay(5000); // Wait before next sync cycle
            }
        }

        static void SyncDeletionsByComparison(OleDbConnection sourceConn, OleDbConnection targetConn, string tableName, string pkColumn)
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

        static int SyncDirection(OleDbConnection sourceConn, OleDbConnection targetConn,
                          string tableName, ref DateTime lastSyncTime,
                          bool isServerToClient, string pkColumn)
        {
            int changesApplied = 0;
            DateTime maxTimestamp = lastSyncTime;

            string getChangesQuery = $@"
        SELECT * FROM [{tableName}]
        WHERE LastModified > ?
        ORDER BY LastModified";

            using var cmd = new OleDbCommand(getChangesQuery, sourceConn);
            cmd.Parameters.AddWithValue("@LastModified", lastSyncTime);

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
                    var rowTimestamp = Convert.ToDateTime(row["LastModified"]);
                    if (rowTimestamp > maxTimestamp)
                        maxTimestamp = rowTimestamp;
                }
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
            var pkValue = row[pkColumn];
            var incomingLastModified = Convert.ToDateTime(row["LastModified"]);

            bool exists = RecordExists(targetConn, tableName, pkColumn, pkValue);
            if (!exists)
                return InsertRecord(targetConn, tableName, row);

            var targetLastModified = GetLastModified(targetConn, tableName, pkColumn, pkValue);
            var targetRecord = GetRecord(targetConn, tableName, pkColumn, pkValue);

            // Simple conflict resolution - server wins
            if (isServerToClient)
            {
                bool dataIsDifferent = !row["Name"].Equals(targetRecord["Name"]);
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

        static List<Guid> GetAllIds(OleDbConnection conn, string tableName, string pkColumn)
        {
            var ids = new List<Guid>();
            string query = $"SELECT [{pkColumn}] FROM [{tableName}]";

            using var cmd = new OleDbCommand(query, conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                ids.Add(reader.GetGuid(0));
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

        static DateTime GetLastModified(OleDbConnection conn, string tableName, string pkColumn, object pkValue)
        {
            string query = $"SELECT LastModified FROM [{tableName}] WHERE [{pkColumn}] = ?";
            using var cmd = new OleDbCommand(query, conn);
            cmd.Parameters.AddWithValue($"@{pkColumn}", pkValue);
            var result = cmd.ExecuteScalar();
            return (result != DBNull.Value && result != null) ? Convert.ToDateTime(result) : DateTime.MinValue;
        }
        #endregion
    }
}