using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
class InvalidAgeException : Exception
{
    public InvalidAgeException(string message) : base(message) { }
}

namespace March26
{
    internal class ExceptionHandling
    {
        public static void Exception()
        {
            try
            {
                Console.WriteLine("Enter a number: ");
                int number = Convert.ToInt32(Console.ReadLine());
                int result = number / 0;
                Console.WriteLine($"the Result: {result}");
            }
            catch (DivideByZeroException ex)
            {
                Console.WriteLine("❌ Error: Division by zero is not allowed.");
            }
            catch (FormatException ex)
            {
                Console.WriteLine("Invalid Exception !");
            }
            finally
            {
                Console.WriteLine("✅ Operation completed.");
            }
        }
        public static void Exception2(int age)
        {
            if(age < 18)
                {
                    throw new InvalidAgeException("Age must be 18 and above ");
                }
            else
            {
                Console.WriteLine("Age Verification Successfull!✔");
            }
            }
        }
    }

