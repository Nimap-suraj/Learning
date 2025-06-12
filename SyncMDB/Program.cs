using System;
using System.Data;
using System.Data.OleDb;

class Program
{
    static string clientDbPath = @"C:\Users\DELL\Documents\MDB\suraj.mdb";
    static string serverDbPath = @"\\192.168.1.67\himanshu\himanshu.mdb";
    static string clientConnStr = $"Provider=Microsoft.ACE.OLEDB.16.0;Data Source={clientDbPath};";
    static string serverConnStr = $"Provider=Microsoft.ACE.OLEDB.16.0;Data Source={serverDbPath};";

    static void Main()
    {
        try
        {
            Console.WriteLine("Starting synchronization process...");

            // Test connections first
            if (!TestConnection("Client", clientConnStr) || !TestConnection("Server", serverConnStr))
            {
                Console.WriteLine("Cannot proceed with synchronization due to connection issues.");
                return;
            }

            // Step 1: Create Draft table if it doesn't exist
            //CreateDraftTable();

            // Step 2: Identify and move conflicting records to Draft
            MoveConflictingRecordsToDraft();

            // Step 3: Pull records from server
            PullServerRecords();

            // Step 4: Push client records to server
            PushClientRecords();

            // Step 5: Restore records from Draft table
            RestoreFromDraft();

            Console.WriteLine("Synchronization completed successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during synchronization: {ex.Message}");
        }
    }

    static bool TestConnection(string name, string connectionString)
    {
        Console.WriteLine($"Testing {name} connection...");
        try
        {
            using var connection = new OleDbConnection(connectionString);
            connection.Open();
            Console.WriteLine($"{name} connection successful.");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{name} connection failed: {ex.Message}");
            return false;
        }
    }

 

    static void MoveConflictingRecordsToDraft()
    {
        Console.WriteLine("Moving conflicting records to Draft table...");

        using (OleDbConnection clientConn = new OleDbConnection(clientConnStr))
        using (OleDbConnection serverConn = new OleDbConnection(serverConnStr))
        {
            clientConn.Open();
            serverConn.Open();

            // Get all records from client and server
            DataTable clientRecords = new DataTable();
            DataTable serverRecords = new DataTable();

            new OleDbDataAdapter("SELECT ID, Data, LastModified FROM MainTable", clientConn).Fill(clientRecords);
            new OleDbDataAdapter("SELECT ID, Data, LastModified FROM MainTable", serverConn).Fill(serverRecords);

            foreach (DataRow clientRow in clientRecords.Rows)
            {
                int id = Convert.ToInt32(clientRow["ID"]);
                string clientData = clientRow["Data"].ToString();
                DateTime clientModified = Convert.ToDateTime(clientRow["LastModified"]);

                // Find matching record in server
                DataRow[] serverMatches = serverRecords.Select($"ID = {id}");
                if (serverMatches.Length > 0)
                {
                    string serverData = serverMatches[0]["Data"].ToString();
                    DateTime serverModified = Convert.ToDateTime(serverMatches[0]["LastModified"]);

                    // If data differs, move to draft
                    if (clientData != serverData)
                    {
                        Console.WriteLine($"Found conflict for ID {id}, moving to Draft...");

                        // Insert into Draft
                        string insertDraftSql = @"
                            INSERT INTO Draft (ID, Data, LastModified, OriginalTable) 
                            VALUES (?, ?, ?, ?)";
                        OleDbCommand cmd = new OleDbCommand(insertDraftSql, clientConn);
                        cmd.Parameters.AddWithValue("ID", id);
                        cmd.Parameters.AddWithValue("Data", clientData);
                        cmd.Parameters.AddWithValue("LastModified", clientModified);
                        cmd.Parameters.AddWithValue("OriginalTable", "MainTable");
                        cmd.ExecuteNonQuery();

                        // Delete from client
                        string deleteSql = $"DELETE FROM MainTable WHERE ID = {id}";
                        new OleDbCommand(deleteSql, clientConn).ExecuteNonQuery();
                    }
                }
            }
        }
    }

    static void PullServerRecords()
    {
        Console.WriteLine("Pulling records from server...");

        using (OleDbConnection clientConn = new OleDbConnection(clientConnStr))
        using (OleDbConnection serverConn = new OleDbConnection(serverConnStr))
        {
            clientConn.Open();
            serverConn.Open();

            // Get all records from server
            DataTable serverRecords = new DataTable();
            new OleDbDataAdapter("SELECT ID, Data, LastModified FROM MainTable", serverConn).Fill(serverRecords);

            foreach (DataRow serverRow in serverRecords.Rows)
            {
                int id = Convert.ToInt32(serverRow["ID"]);
                string serverData = serverRow["Data"].ToString();
                DateTime serverModified = Convert.ToDateTime(serverRow["LastModified"]);

                // Check if record exists in client
                string checkSql = $"SELECT COUNT(*) FROM MainTable WHERE ID = {id}";
                int exists = (int)new OleDbCommand(checkSql, clientConn).ExecuteScalar();

                if (exists == 0)
                {
                    // Insert new record
                    string insertSql = @"
                        INSERT INTO MainTable (ID, Data, LastModified) 
                        VALUES (?, ?, ?)";
                    OleDbCommand cmd = new OleDbCommand(insertSql, clientConn);
                    cmd.Parameters.AddWithValue("ID", id);
                    cmd.Parameters.AddWithValue("Data", serverData);
                    cmd.Parameters.AddWithValue("LastModified", serverModified);
                    cmd.ExecuteNonQuery();

                    Console.WriteLine($"Pulled new record ID {id} from server");
                }
            }
        }
    }

    static void PushClientRecords()
    {
        Console.WriteLine("Pushing client records to server...");

        using (OleDbConnection clientConn = new OleDbConnection(clientConnStr))
        using (OleDbConnection serverConn = new OleDbConnection(serverConnStr))
        {
            clientConn.Open();
            serverConn.Open();

            // Get all records from client
            DataTable clientRecords = new DataTable();
            new OleDbDataAdapter("SELECT ID, Data, LastModified FROM MainTable", clientConn).Fill(clientRecords);

            foreach (DataRow clientRow in clientRecords.Rows)
            {
                int id = Convert.ToInt32(clientRow["ID"]);
                string clientData = clientRow["Data"].ToString();
                DateTime clientModified = Convert.ToDateTime(clientRow["LastModified"]);

                // Check if record exists in server
                string checkSql = $"SELECT COUNT(*) FROM MainTable WHERE ID = {id}";
                int exists = (int)new OleDbCommand(checkSql, serverConn).ExecuteScalar();

                if (exists == 0)
                {
                    // Insert new record
                    string insertSql = @"
                        INSERT INTO MainTable (ID, Data, LastModified) 
                        VALUES (?, ?, ?)";
                    OleDbCommand cmd = new OleDbCommand(insertSql, serverConn);
                    cmd.Parameters.AddWithValue("ID", id);
                    cmd.Parameters.AddWithValue("Data", clientData);
                    cmd.Parameters.AddWithValue("LastModified", clientModified);
                    cmd.ExecuteNonQuery();

                    Console.WriteLine($"Pushed new record ID {id} to server");
                }
                else
                {
                    // Update existing record if client has newer version
                    string getServerModifiedSql = $"SELECT LastModified FROM MainTable WHERE ID = {id}";
                    DateTime serverModified = (DateTime)new OleDbCommand(getServerModifiedSql, serverConn).ExecuteScalar();

                    if (clientModified > serverModified)
                    {
                        string updateSql = @"
                            UPDATE MainTable 
                            SET Data = ?, LastModified = ? 
                            WHERE ID = ?";
                        OleDbCommand cmd = new OleDbCommand(updateSql, serverConn);
                        cmd.Parameters.AddWithValue("Data", clientData);
                        cmd.Parameters.AddWithValue("LastModified", clientModified);
                        cmd.Parameters.AddWithValue("ID", id);
                        cmd.ExecuteNonQuery();

                        Console.WriteLine($"Updated record ID {id} on server");
                    }
                }
            }
        }
    }

    static void RestoreFromDraft()
    {
        Console.WriteLine("Restoring records from Draft table...");

        using (OleDbConnection clientConn = new OleDbConnection(clientConnStr))
        {
            clientConn.Open();

            // Get all records from Draft
            DataTable draftRecords = new DataTable();
            new OleDbDataAdapter("SELECT ID, Data, LastModified, OriginalTable FROM Draft", clientConn).Fill(draftRecords);

            foreach (DataRow draftRow in draftRecords.Rows)
            {
                int id = Convert.ToInt32(draftRow["ID"]);
                string data = draftRow["Data"].ToString();
                DateTime modified = Convert.ToDateTime(draftRow["LastModified"]);
                string originalTable = draftRow["OriginalTable"].ToString();

                // Insert back to original table
                string insertSql = $@"
                    INSERT INTO {originalTable} (ID, Data, LastModified) 
                    VALUES (?, ?, ?)";
                OleDbCommand cmd = new OleDbCommand(insertSql, clientConn);
                cmd.Parameters.AddWithValue("ID", id);
                cmd.Parameters.AddWithValue("Data", data);
                cmd.Parameters.AddWithValue("LastModified", modified);
                cmd.ExecuteNonQuery();

                // Delete from Draft
                string deleteDraftSql = $"DELETE FROM Draft WHERE ID = {id}";
                new OleDbCommand(deleteDraftSql, clientConn).ExecuteNonQuery();

                Console.WriteLine($"Restored record ID {id} from Draft to {originalTable}");
            }
        }
    }
}