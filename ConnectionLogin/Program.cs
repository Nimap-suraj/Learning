internal class Program
{
    private static void Main(string[] args)
    {
        string configPath = "data.txt";
        string UserName = string.Empty;
        string Password = string.Empty;
        // agar exit karta hain tabhi run hoga and set kar dega
        if (File.Exists(configPath))
        {
            string[] savedCredential = File.ReadAllLines(configPath);
            if (savedCredential.Length >= 2)
            {
                UserName = savedCredential[0];
                Password = savedCredential[1];
                Console.WriteLine($"Logged in As {UserName}");
                Console.WriteLine("Want To use Different Credentials(y/n) ?");
                if (Console.ReadLine().ToLower() == "y")
                {
                    UserName = "";
                    Password = "";
                }
                else
                {
                    Console.WriteLine("another code is running start");
                }
            }
        }
        bool LoginSuccess = false;
        while (!LoginSuccess)
        {
            Console.Write("Username: ");
            UserName = Console.ReadLine();
            Console.Write("Password: ");
            Password = Console.ReadLine();

            LoginSuccess = CheckLogin(UserName, Password);
            if (LoginSuccess)
            {
                Console.WriteLine("Login Successful");

                Console.WriteLine("Remember credentials (y/n)?");
                if (Console.ReadLine().ToLower() == "y")
                {
                    File.WriteAllText(configPath, $"{UserName}\n{Password}");
                    Console.WriteLine("Credentials Saved!");
                }
            }
            else
            {
                Console.WriteLine("Login Failed. Please try again.");
                Console.Beep();
            }
        }

    }
    static bool CheckLogin(string UserName, string Password)
    {
        return (UserName == "admin" && Password == "admin");
    }
}