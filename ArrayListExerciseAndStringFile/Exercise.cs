using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArrayListExercise
{
    internal class Exercise
    {
        public static void E1()
        {
            var list = new List<String>();
            while (true)
            {
                Console.Write("enter Your Name: ");
                var input = Console.ReadLine();
                if (!String.IsNullOrEmpty(input))
                {
                    list.Add(input.ToLower());
                    continue;
                }
                break;
            }
            Thread.Sleep(1000);
            foreach (var item in list)
            {
                Console.WriteLine(item);
            }
        }

        public static void E2()
        {
            Console.Write("Enter a Name: ");
            var input = Console.ReadLine();
            //use a array to reverse
            var arr = new char[10];
            // s u r a j
            // 0 1 2 3 4
            int len = input.Length; // 5
            for (int i = 0; i < len; i++)
            {
                arr[i] = input[len - i - 1];
            }
            var str = "";
            foreach (var item in arr)
            {
                str += item;
            }
            Console.WriteLine(str);
        }

        public static void E3()
        {
            var li = new List<String>();
            var chances = 5;
            while (chances >= 0)
            {
                Console.Write("Enter Unique Name:- ");
                var input = Console.ReadLine().ToLower();
                if (!li.Contains(input))
                {
                    li.Add(input);
                    continue;
                }
                else
                {
                    Console.WriteLine("Error Message");
                    break;
                }
            }
            foreach (var item in li)
            {
                Console.WriteLine(item);
            }

        }

        public static void E4()
        {
            var li = new List<int>();
            while (true)
            {
                Console.Write("Enter a Number: ");
                var input = Console.ReadLine();
                //int number = Convert.ToInt32(input);
                //li.Add(number);
                if (input == "quit")
                {
                    //Console.WriteLine("Error");
                    break;
                }
                else if (String.IsNullOrEmpty(input))
                {
                    break;
                }
                else
                {
                    int number = Convert.ToInt32(input);
                    li.Add((int)number);
                    continue;
                }
            }
            var set = new HashSet<int>();
            foreach (var item in li)
            {
                set.Add(item);
            }
            foreach (var item in set)
            {
                Console.Write(item + " ");
            }


        }


        public static void E5()
        {
            string[] elements;
            while (true)
            {
                Console.Write("Enter a list of comma-separated numbers: ");
                var input = Console.ReadLine();

                if (!String.IsNullOrWhiteSpace(input))
                {
                    elements = input.Split(',');
                    if (elements.Length >= 5)
                        break;
                }

                Console.WriteLine("Invalid List");
            }

            var numbers = new List<int>();
            foreach (var number in elements)
                numbers.Add(Convert.ToInt32(number));

            var smallests = new List<int>();
            while (smallests.Count < 3)
            {
                // Assume the first number is the smallest
                var min = numbers[0];
                foreach (var number in numbers)
                {
                    if (number < min)
                        min = number;
                }
                smallests.Add(min);

                numbers.Remove(min);
            }

            Console.WriteLine("The 3 smallest numbers are: ");
            foreach (var number in smallests)
                Console.WriteLine(number);
        }


        public static void E6()
        {
            Console.WriteLine("enter a numbers separated by a hyphen.");
            var input = Console.ReadLine();
            var numbers = new List<int>();
            foreach (var number in input.Split('-'))
            {
                numbers.Add(Convert.ToInt32(number));
            }
            numbers.Sort();
            bool isConsequitive = true;
            for (int i = 1; i < numbers.Count; i++)
            {
                if (numbers[i] != numbers[i - 1] + 1)
                {
                    isConsequitive = false;
                    break;
                }
            }
            if (isConsequitive)
            {
                Console.WriteLine("Consequitive!!");
            }
            else
            {
                Console.WriteLine("non Consequitives: ");
            }
        }


        public static void E7()
        {
            Console.WriteLine("Enter a number by hyphen:");
            var input = Console.ReadLine();

            if (String.IsNullOrWhiteSpace(input))
            {
                return;
            }

            var arr = new List<int>();
            foreach (var num in input.Split('-'))
                arr.Add(Convert.ToInt32(num));

            var unique = new HashSet<int>(arr); // Use HashSet to automatically handle duplicates
            if (unique.Count < arr.Count)
            {
                Console.WriteLine("Duplicates");
            }
            else
            {
                Console.WriteLine("No duplicates");
            }
        }


        public static void E8()
        {
            Console.WriteLine("Enter a Time: ");
            var input = Console.ReadLine();
            if (String.IsNullOrWhiteSpace(input))
            {
                return;
            }
            var time = input.Split(':');
            if (time.Length < 2)
            {
                return;
            }
            try
            {
                // 12:12
                var hours = Convert.ToInt32(time[0]);
                var minutes = Convert.ToInt32(time[1]);
                if ((hours > 0 && hours < 24) && (minutes > 0 && minutes < 59))
                {
                    Console.WriteLine("Ok");
                }
                else
                {
                    Console.WriteLine("Not Ok");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public static void E9()
        {
            Console.WriteLine("Enter a Spaces: ");
            var input = Console.ReadLine();


            if (String.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Error");
                return;
            }


            var str = "";
            foreach (var num in input.Split(' '))
            {
                var wordWithPascalCase = char.ToUpper(num[0]) + num.ToLower().Substring(1);
                str += wordWithPascalCase;
            }
            Console.WriteLine(str);

        }


        public static void E10()
        {
            Console.WriteLine("Enter a English Word!  ");
            var input = Console.ReadLine();
            int count = 0;
            foreach (var ch in input)
            {
                if ("aeiouAEIOU".Contains(ch))
                {
                    count++;
                }

            }
             Console.WriteLine("Number of Counts of Vowlels in String are :  " + count);
        }
    }
}