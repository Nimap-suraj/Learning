using System;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Threading;

class Program
{
    static void Main()
    {
        Console.CursorVisible = false;

        // Show loader at the start
        ShowGameStyleLoader("Checking network connectivity", 20);

        Console.Clear(); // Clear loader
        Console.CursorVisible = true;

        string serverIp = "95.111.230.3";
        string shareName = "BatFolder";
        string driveLetter = "X:";
        string username = "administrator";
        string password = "N1m@p2025$Server";

        ShowGameStyleLoader("Checking connection with Localhost...", 5);
        if (PingHost("192.168.137.1"))
        {
            Console.WriteLine("Maintaining connection with LocalMachine is successful.\n");
        }
        else
        {
            Console.WriteLine("ERROR: Localhost is not responding.");
            Console.ReadKey();
            return;
        }
        ShowGameStyleLoader("Checking connection with Server...", 20);
        if (PingHost(serverIp))
        {
            Console.WriteLine($"Maintaining connection with Server ({serverIp}) is successful.\n");
        }
        else
        {
            Console.WriteLine($"ERROR: Cannot reach server {serverIp}.");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("All connection checks completed successfully.");


        Console.WriteLine("Mounting shared folder...");
        UnmountDrive(driveLetter); // Remove if already mapped
        bool mounted = MountNetworkDrive(driveLetter, serverIp, shareName, username, password);
        if (!mounted)
        {
            Console.WriteLine("ERROR: Could not mount the shared folder.");
            Console.ReadKey();
            return;
        }

        Console.WriteLine($"Shared folder mounted to {driveLetter}\n");

           }

    static bool MountNetworkDrive(string driveLetter, string serverIp, string shareName, string username, string password)
    {
        // Remove trailing backslash if present
        driveLetter = driveLetter.TrimEnd('\\');

        string command = $"net use {driveLetter} \\\\{serverIp}\\{shareName} {password} /user:{username} /persistent:no";

        return ExecuteCommand(command);
    }

    static void UnmountDrive(string driveLetter)
    {
        string command = $"net use {driveLetter} /delete /y";
        ExecuteCommand(command, ignoreErrors: true); // Updated
    }


    static bool ExecuteCommand(string command, bool ignoreErrors = false)
    {
        try
        {
            ProcessStartInfo psi = new ProcessStartInfo("cmd.exe", "/c " + command)
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                UseShellExecute = false
            };

            using (Process process = Process.Start(psi))
            {
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                if (process.ExitCode != 0)
                {
                    if (!ignoreErrors)
                    {
                        Console.WriteLine("Command failed:");
                        Console.WriteLine(error);
                        return false;
                    }
                }
            }

            return true;
        }
        catch (Exception ex)
        {
            if (!ignoreErrors)
                Console.WriteLine("Exception during command execution: " + ex.Message);
            return false;
        }
    }

    static bool PingHost(string host)
    {
        try
        {
            using (Ping ping = new Ping())
            {
                PingReply reply = ping.Send(host, 2000); // 2 second timeout
                return reply.Status == IPStatus.Success;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Exception pinging {host}: {ex.Message}");
            return false;
        }
    }

    static void ShowGameStyleLoader(string message, int totalSteps)
    {
        Console.WriteLine(message);
        int progressBarWidth = 50;

        for (int i = 0; i <= totalSteps; i++)
        {
            double percentage = (double)i / totalSteps;
            int filledBars = (int)(percentage * progressBarWidth);
            string bar = new string('█', filledBars).PadRight(progressBarWidth, '-');

            Console.Write($"\r[{bar}] {percentage * 100:0}%");

            Thread.Sleep(20); // Optional: replace with real logic if needed
        }

        Console.WriteLine(); // Move to the next line after loader
    }
}