using Microsoft.Data.SqlClient;
using System;
using System.Data.SqlClient;

class Program
{
    static void Main()
    {
        // Replace these with your actual connection strings
        //string localConnectionString = "Server=DESKTOP-TBOGTIK\\SQLEXPRESS;Database=Learn;Trusted_Connection=True;TrustServerCertificate=True";
        //string serverConnectionString = "Server=my.server.com;Database=MyServerDB;User Id=myuser;Password=mypassword;";
        // Temporary debug string (do NOT use in production)
        string localConnectionString = "Server=DESKTOP-TBOGTIK\\SQLEXPRESS;Database=HospitalSystem;User Id=sa;Password=root;Trusted_Connection=True;TrustServerCertificate=True;";
        TestConnection("Local Database", localConnectionString);
        //TestConnection("Server Database", serverConnectionString);

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }

    static void TestConnection(string name, string connectionString)
    {
        Console.WriteLine($"\nTesting {name} connection...");

        try
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                Console.WriteLine($"SUCCESS: Connected to {name}");
                Console.WriteLine($"  Server: {connection.DataSource}");
                Console.WriteLine($"  Database: {connection.Database}");
                Console.WriteLine($"  Version: {connection.ServerVersion}");
                connection.Close();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"FAILED: Could not connect to {name}");
            Console.WriteLine($"  Error: {ex.Message}");

            if (ex is SqlException sqlEx)
            {
                Console.WriteLine($"  SQL Error #{sqlEx.Number}");
            }
        }
    }
}