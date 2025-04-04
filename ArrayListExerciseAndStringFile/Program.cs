using System;
using System.Collections.Generic;

namespace ArrayListExercise
{
    internal class Program
    {
       public static void Main(string[] args)
        {


            //LogicalQuestion.SecondMax();
            //LogicalQuestion.ReverseInteger();
            //LogicalQuestion.Swap();
            //LogicalQuestion.Anagram();
            // LogicalQuestion.Anagram1();
            // Divisible to 3
            //NumberDividedByThree();
            //var sum = SumOfNumberOnly();
            //Console.WriteLine($"The sum of Input number are {sum} ");
            //int factorial = FactorialOfNumber();
            //Console.WriteLine($"The Factorial of number is {factorial}");
            //var random = new Random();
            //var secretNumber = random.Next(1, 11);
            //Console.WriteLine(RandomNumberGame(secretNumber));
            //Console.WriteLine($"Maximum number Between these number is {GetMaximum()}");
            //NumberOfPeopleLikePost();
            // ReverseArrayToString();
            //  UniqueFiveNumber();
            //  getUniqueNumber();
            // getThreeSmallest();



            // Exercise  1
            // Exercise.E1();
            // Exercise.E2();
            //Exercise.E3();
            //Exercise.E4();
            //Exercise.E5();
            //Exercise.E6();
            //Exercise.E7();
            //Exercise.E8();
            //Exercise.E9();
            //Exercise.E10();
            //Date.GetTime();

            //StringClass.StringClassLecture();
            //var sentences = "This is Going very hard now i can;t explain it now why this is happening but what is happening i don't know";
            //Console.WriteLine(liveCoding.cutStringTOLimit(sentences,20));
            //StringBuilderClass.stringBuilder();
            // Procedural.ProceduralCoding();

            //FileClassLecture.FileCLass();

            //DirectoryFIlew.Dir();
            // PathLecture.stringCount();
            //PathLecture.LongestWord();

            // debugging
            //var list = new List<int>() { 1,2,3,4,5,6};  
            //var smaller = GetSmallest(list, 3);

            //foreach (var item in smaller)
            //{
            //    Console.WriteLine("Smallest Numbers are: "+item);
            //}
            //Console.Write("List Elements are: ");
            //foreach (var item in list)
            //{
            //    Console.Write(item + " ");
            //}

            int n1 = 10;
            int n2 = 20;

            //int sum = n1 + n2;
            Console.WriteLine($"The sum of {n1} and {n2} is {n1 + n2}");
            //Console.WriteLine("sum is "+sum);
        }
        static void getThreeSmallest()
        {
            String[] element;
            while (true)
            {
                Console.WriteLine("Enter a number: ");
                var input = Console.ReadLine();
                if (!String.IsNullOrWhiteSpace(input))
                {
                    element = input.Split(',');
                    if (element.Length >= 5)
                        break;
                }
                Console.WriteLine("Invalid String! enter number atMost 5");
            }
            var number = new List<int>();
            foreach (var num in element)
            {
                number.Add(Convert.ToInt32(num));
            }
            var smallest = new List<int>();
            while (smallest.Count < 3)
            {
                var min = number[0];
                foreach (var num in number)
                {
                    if (num < min)
                    {
                        min = num;
                    }

                }
                smallest.Add(min);
                number.Remove(min);


            }
            foreach (var item in smallest)
            {
                Console.WriteLine(item);
            }
        }
        static void getUniqueNumber()
        {
            var numbers = new List<int>();
            while (true)
            {
                Console.WriteLine("Enter a number: ");
                var input = Console.ReadLine();
                if (String.IsNullOrEmpty(input))
                {
                    break;
                }
                if (input.ToLower() == "quit")
                {
                    break;
                }
                numbers.Add(Convert.ToInt32(input));
            }
            var unique = new List<int>();
            foreach (var num in numbers)
            {
                if (!unique.Contains(num))
                {
                    unique.Add(num);
                    continue;
                }
            }
            Console.WriteLine("Unique numbers:");
            foreach (var number in unique)
                Console.WriteLine(number);
        }
        static void UniqueFiveNumber()
        {
            var numbers = new List<int>();
            while (numbers.Count < 5)
            {
                Console.Write("enter a Number: ");
                var input = Convert.ToInt32(Console.ReadLine());
                if (numbers.Contains(input))
                {
                    Console.WriteLine("Number was Previusly enter try Again");
                    continue;
                }
                numbers.Add(input);
            }
            Console.Write("[  ");
            foreach (var item in numbers)
            {
                Console.Write(item + " , ");
            }
            Console.Write(" ]");


        }
        static void ReverseArrayToString()
        {
            Console.WriteLine("Enter a String: ");
            var input = Console.ReadLine();
            var array = new char[input.Length];

            for (int i = input.Length; i > 0; i--)
            {
                array[input.Length - i] = input[i - 1];
            }
            var str = new String(array);
            Console.WriteLine($"Reverse string of {input} is {str} ");
        }
        static void NumberOfPeopleLikePost()
        {
            var names = new List<string>();
            while (true)
            {
                Console.Write("Enter a Name: ");
                var input = Console.ReadLine();
                if (String.IsNullOrEmpty(input))
                {
                    break;
                }
                names.Add(input);
                continue;
            }
            if (names.Count > 2)
            {
                Console.WriteLine($"{names[0]} , {names[1]} , {names[2]} Like your Post! ");
            }
            else if (names.Count == 2)
            {
                Console.WriteLine($"{names[0]} {names[1]}Like your Post! ");
            }
            else if (names.Count == 1)
            {
                Console.WriteLine($"{names[0]} Like your Post! ");
            }
            else
            {
                Console.WriteLine();
            }

        }
        static void NumberDividedByThree()
        {
            int count = 0;
            for (var i = 0; i <= 100; i++)
            {
                if (i % 3 == 0)
                {
                    count++;
                }
            }
            Console.WriteLine("numbers between 1 and 100 are divisible by 3 with no Remainder are:" + count);
        }

        static int SumOfNumberOnly()
        {
            int sum = 0;
            while (true)
            {
                Console.Write("Enter a Number : ");
                var input = Console.ReadLine();
                if (input.ToLower() == "quit" || String.IsNullOrEmpty(input))
                {
                    break;
                }
                sum = sum + Convert.ToInt32(input);
            }
            return sum;
        }

        static int FactorialOfNumber()
        {
            int factorial = 1;
            Console.Write("Enter a Number :");
            var number = Convert.ToInt32(Console.ReadLine());
            for (int i = 1; i <= number; i++)
            {
                factorial *= i;
            }
            return factorial;
        }

        static string RandomNumberGame(int secretNumber)
        {
            int chances = 4;
            bool isWinner = false;

            while (chances > 0)
            {
                Console.Write("Guess a Number: ");
                var number = Console.ReadLine();
                if (String.IsNullOrEmpty(number))
                {
                    break;
                }


                if (secretNumber < Convert.ToInt32(number))
                {
                    Console.WriteLine("Number is Too High");
                    chances--;
                    continue;
                }
                else if (secretNumber > Convert.ToInt32(number))
                {
                    Console.WriteLine("Number is Too Low");
                    chances--;
                    continue;
                }
                else
                {

                    isWinner = true;
                    break;
                }
            }
            return (isWinner) ? "You Won" : $"Game Over, Secreate number was {secretNumber}";
        }

        static int GetMaximum()
        {
            Console.WriteLine("Enter a number ");
            var input = Console.ReadLine();
            var number = input.Split(',');
            var max = Convert.ToInt32(number[0]);
            for (int num = 1; num < number.Length; num++)
            {
                var n = Convert.ToInt32(number[num]);
                if (n > max)
                {
                    max = n;
                }
            }
            return max;
        }

         public static List<int> GetSmallest(List<int> list, int count)
         {
             if(count > list.Count)
             {
                 throw new ArgumentOutOfRangeException("count should not greater tham array size");
             }
             var smallest = new List<int>();

             var buffer = new List<int>(list);

             while(smallest.Count < count)
             {
                 var min = GetSmallest(buffer);
                 smallest.Add(min);
                 buffer.Remove(min);
             }
             return smallest;
         }

        public static int GetSmallest(List<int> list)
        {
            var min = list[0];
            for (int i = 1; i < list.Count; i++)
            {
                if (list[i] < min)
                {
                    min = list[i];
                }
            }
            return min;
            //throw new NotImplementedException();  
        }
    }
}
