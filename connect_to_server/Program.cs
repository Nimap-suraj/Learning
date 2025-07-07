using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Principal;

class Program
{
    static void Main()
    {
        try
        {
            // Replace these values with your actual credentials
            string username = "suraj";
            string domain = "192.168.98.4"; // or "." for local machine
            string password = "1234";
            string remotePath = @"\\95.111.230.3\BatFolder";

            NetworkAccess.AccessFile(username, domain, password, remotePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}

public class NetworkAccess
{
    [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    public static extern bool LogonUser(string lpszUsername, string lpszDomain, string lpszPassword,
        int dwLogonType, int dwLogonProvider, out IntPtr phToken);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
    public extern static bool CloseHandle(IntPtr handle);

    public static void AccessFile(string username, string domain, string password, string remotePath)
    {
        IntPtr userToken = IntPtr.Zero;

        try
        {
            // LOGON32_LOGON_NEW_CREDENTIALS = 9
            // LOGON32_PROVIDER_DEFAULT = 0
            bool isSuccess = LogonUser(username, domain, password, 9, 0, out userToken);

            if (!isSuccess)
            {
                int error = Marshal.GetLastWin32Error();
                throw new UnauthorizedAccessException($"LogonUser failed. Error code: {error}");
            }

            using (WindowsIdentity identity = new WindowsIdentity(userToken))
            {
                using (WindowsImpersonationContext context = identity.Impersonate())
                {
                    Console.WriteLine($"Successfully connected to: {remotePath}");
                    Console.WriteLine("Files:");

                    // Access the remote path like a local path
                    string[] files = Directory.GetFiles(remotePath);
                    foreach (var file in files)
                    {
                        Console.WriteLine(file);
                    }
                }
            }
        }
        finally
        {
            if (userToken != IntPtr.Zero)
                CloseHandle(userToken);
        }
    }
}