using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwaitDemo
{
    public interface IAccount
    {
         void PrintDetails(); 
        // without body 
    }
    class CurrentAccount:IAccount
    {
        public void PrintDetails() 
        {
            Console.WriteLine("Current Accont Details");
        }
    }
    class SavingAccount : IAccount
    {
        public void PrintDetails()
        {
            Console.WriteLine("Saving Accont Details");
        }
    }
    // Constructor Injection.

    //class Account
    //{
    //    private IAccount _account;

    //    public Account(IAccount account)
    //    {
    //           this._account = account;
    //    }
    //    public void PrintDetails()
    //    {
    //        _account.PrintDetails();
    //    }

    //}
    // Property Injection
    //class Account
    //{
    //    public IAccount account { get; set; }
    //    public void PrintDetails()
    //    {
    //        account.PrintDetails();
    //    }
    //}
    // Method Injection
    class Account
    {
        public void PrintDetails(IAccount account)
        {
            account.PrintDetails();
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            // how Async and Await works
            //Task2();
            //Task4();
            //Task1();
            //Task3();

            
            // Constructor Injection.
            //IAccount CA = new CurrentAccount();
            //Account a = new Account(CA);
            //a.PrintDetails();

            //IAccount SA = new SavingAccount();
            //Account a2 = new Account(SA);
            //a2.PrintDetails();


            // ProPerty Injection.
            //Account sa = new Account();
            //sa.account = new SavingAccount();
            //sa.PrintDetails();

            //Account ca = new Account();
            //ca.account = new CurrentAccount();
            //ca.PrintDetails();

            // Method Injection
            //Account sa = new Account();    
            //sa.PrintDetails(new SavingAccount());

            //Account ca = new Account();    
            //ca.PrintDetails(new CurrentAccount());


            // How Thread Works 
            Method();
            Console.WriteLine("Main Thread");
            Console.ReadLine();
            Console.ReadKey();
        }
        public static async void Method()
        {
            await Task.Run(new Action(LongTask));
            Console.WriteLine("New Thread");
        }

        public static void LongTask()
        {
            Console.WriteLine("LongTask started");
            for (int count = 20; count > 0; count--)
            {
                Console.WriteLine(count);
                Thread.Sleep(1000); // Sleep for 1 second
            }
            Console.WriteLine("LongTask completed");
        }
        static async void Task1()
        {
            await Task.Run(() =>
            {
                Console.WriteLine("Task1 Started!");
                Thread.Sleep(4000);
                Console.WriteLine("Task1 Ended!");
            });
        }

        static async void Task2()
        {
            await Task.Run(() =>
            {
                Console.WriteLine("Task2 Started!");
                Thread.Sleep(3000);
                Console.WriteLine("Task2 Ended!");
            });
        }

        static async void  Task3()
        {
            await Task.Run(() =>
            {
                Console.WriteLine("Task3 Started!");
                Thread.Sleep(2000);
                Console.WriteLine("Task3 Ended!");
            });
        }

        static async void Task4()
        {
            await Task.Run(() =>
            {
                Console.WriteLine("Task4 Started!");
                Thread.Sleep(1000);
                Console.WriteLine("Task4 Ended!");
            });
        }
    }
}