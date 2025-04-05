using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace March26
{
    internal class Program

    {
       
        static void Main(string[] args)
        {


            ////ExceptionHandling.Exception();
            //try
            //{
            //    ExceptionHandling.Exception2(9);
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
            //// TPL.TplClass();
            Generic.genericLec();
            ////G2Class();


            var account = new BankAccount(40);

            try
            {
                account.Withdraw(59); // Attempting to withdraw more than available
            }
            catch (InvalidFundException ex)
            {
                Console.WriteLine(ex.Message);
            }


        }
        static void G1Method()
        {
            var util = new Utility();
            util.Display<int>(100);
            util.Display<String>("suraj shah");
            util.Display<double>(10.23);
        }
        static void G2Class() {
            var newInt = new Box<int>();
            newInt.Add(10);
            Console.WriteLine("The value is " + newInt.GetData());

            var newString = new Box<string>();
            newString.Add("suraj Shah");
            Console.WriteLine("The Value is " + newString.GetData());
        }
        static void Indexer()
        {
            //var collection = new SampleCollection();
            //collection[0] = "apple";
            //collection[1] = "banana";

            //Console.WriteLine(collection[0]);  // Output: Apple
            //Console.WriteLine(collection[1]);  // Output: Banana
        }
    }
}
