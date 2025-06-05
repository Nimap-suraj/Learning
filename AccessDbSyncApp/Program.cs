using System;
using System.Data.OleDb;

class Program
{
    static void Main()
    {
  
        string clientDbPath = @"C:\Users\DELL\Documents\MDB\suraj.mdb";  // Local path on your laptop
        string clientConnStr = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={clientDbPath};";

        string serverDbPath = @"\\192.168.1.93\mdbfile\rajat.mdb";  // Shared folder on friend's laptop
        string serverConnStr = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={serverDbPath};";


        using (OleDbConnection clientConn = new OleDbConnection(clientConnStr))
        using (OleDbConnection serverConn = new OleDbConnection(serverConnStr))
        {
            try
            {
                clientConn.Open();
                Console.WriteLine("Maintaining connection with local is successfull...");

                serverConn.Open();
                Console.WriteLine("Maintaining connection with server is successfull...");
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Connection error: " + ex.Message);
            }
        }

   
    }
}
