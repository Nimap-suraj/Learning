using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace C_Intermediate
{ 
    internal class Program
    {
      
        static void Main(string[] args)
        {
            //  TodayCode();
            Console.WriteLine();


            ////LogicalQuestion.ReverseString();
            //if (LogicalQuestion.palindrome())
            //{
            //    Console.WriteLine("palindrome");
            //}
            //else
            //{
            //    Console.WriteLine("non");
            //}
            //Console.WriteLine(square(5));
            
            Func<int, int> func = x => x * x;
            Console.WriteLine("Square of number is "+func(10));

            //Post p1 = new Post();
            //p1.Title = "task1";
            //p1.Description = "Today's task done";
            //p1.UpVote();
            //p1.UpVote();
            //Console.WriteLine($"Current Vote: {p1.Vote} {p1.CreatedAt}");
            //string str = "suraj";
            //str += "+";
            //str += "shah";
            //Console.WriteLine(str);

            // Case sensitive.
            //int age = 10;
            //int AGE = 10;

        }
        static void MissingNumber()
        {
            int[] arr = { 1, 2, 4, 5, 7, 8, 10 };
            var list = new List<int>(arr);
            // min
            // max
            for (int i = 1; i <= 10; i++)
            {
                if (!list.Contains(i))
                {
                    list.Add(i);
                }
                else
                {
                    list.Remove(i);
                }
            }
            foreach (var item in list)
            {
                Console.Write(item + " ");
            }
        }
        static int square(int numbers)
        {
            return numbers * numbers;
        }
        static void UseDate()
        {
            //    var student = new Student(new DateTime(2003, 08, 20));
            //Console.WriteLine("Your age is " + student.Age);
            //Console.WriteLine("Your month running is " + student.Month);
            //    student.SetBirthDate(new DateTime(2003,08,20));
            //}
            //Console.WriteLine("Student birthDate is "+ student.GetBirthdate());
        }
        static void TodayCode()
        {
            //Greeting();
            //TryParse();
            //Parse();
            //useParam();
            //personObjectCreation();
            //objectInilization();
            //UsePoints();
            //Customer();
        }
        static void Greeting()
        {
            Console.WriteLine("################ Welcome to C# Intermedate Course!!!!############# ");
            Console.WriteLine();
        }
        static void TryParse()
        {
            int number;
            var result = int.TryParse("1",out  number);
            if (result)
            {
                Console.WriteLine(number);
            }else
                Console.WriteLine("Convertion failed!!");

        }
        static void Parse()
        {
            try
            {
                var input = int.Parse("abc");
                Console.WriteLine(input);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occured !!");
            }
        }
        static void useParam()
        {
            var calculator = new Calculator();
            var sum = calculator.Add(new int[] { 1, 2, 3 });
            var paramSum = calculator.Add(1, 2, 3, 3, 5, 6, 7, 8, 99, 9, 9, 9, 9, 9);
            Console.WriteLine("Sum of an array is " + sum);
            Console.WriteLine("Sum of an array is " + paramSum);
        }
        static void personObjectCreation()
        {
            var person = new Person();
            var p = Person.Parse("suraj shah");
            var q = Person.Parse("Om Sambhar");
            p.Introduce();
            q.Introduce();
        }
        static void objectInilization()
        {
            //Object Initalizer
            var person = new Person
            {
                Name = "suraj shah"
            };
            //Console.WriteLine("Your name is " + person.Name);
        }
        static void UsePoints()
        {
            try
            {
                var point = new Point(10, 20);
                Console.WriteLine($"x: {point.X} y: {point.Y}");


                //point.Move(null); // value cannot be null this line thows an exception becauzw null.x doesn;t make a sense
                //Console.WriteLine($"x: {point.X} y: {point.Y}");

                point.Move(100, 200);
                Console.WriteLine($"x: {point.X} y: {point.Y}");

                point.Move(1000, 2000);
                Console.WriteLine($"x: {point.X} y: {point.Y}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("an unexpected error");
            }
        }

        static void Customer()
        {
            var customer1 = new Customer(1, "suraj");
            var customer2 = new Customer(2, "om");
            Console.WriteLine("Customer id is " + customer2.id + " Your name is " + customer2.name);


            var order = new Order("pizza", 100);
            customer1.Orders.Add(order);

            customer1.Orders.Add(new Order("franky", 120));
            customer1.Orders.Add(new Order("vada pav", 20));

            Console.WriteLine("Orders of customer 1 :");
            foreach (var ord in customer1.Orders)
            {
                Console.WriteLine($"- Item: {ord.food}, Price: {ord.price}");
            }
        }
    }
}
