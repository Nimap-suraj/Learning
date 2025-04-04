using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArrayListExercise
{
    class LogicalQuestion
    {
        public static void SecondMax()
        {
            var array = new List<int>() { 1, 2, 3, 4 };
            int max = 0;
            int secondMax = -1;
            for (int i = 0; i < array.Count; i++)
            {
                if (array[i] > max)
                {
                    secondMax = max;
                    max = array[i];
                }
                else if (array[i] != max && array[i] > secondMax)
                {

                    secondMax = array[i];
                }

            }
            Console.WriteLine($"Maximum: {max} and SecondMaximum: {secondMax}");

        }

        public static void ReverseInteger()
        {
            Console.Write("enter a number : ");
            var number = Convert.ToInt32(Console.ReadLine());
            var t = number;
            // 1234
            var rev = 0;
            while (number > 0)
            {
                var rem = number % 10;// 4 
                rev = rev * 10 + rem; // 0 * 10 + 4 => 4 => 4 * 10 + 3 => 43
                number = number / 10;
            }
            Console.WriteLine($"The Reverse of {t} is {rev}");
        }

        public static void Swap()
        {
            var a = 10;
            var b = 20;
            Console.WriteLine($"before changing values a: {a} b: {b}");
            a = a + b; // 30
            b = a - b; // 30 - 20 => 10
            a = a - b; // 30 - 10 => 20
            Console.WriteLine($"after changing values a: {a} b: {b}");

        }

        public static void Anagram()
        {
            Console.WriteLine("Enter a string 1 : ");
            var str1 = Console.ReadLine();
            Console.WriteLine("Enter a string 2 : ");
            var str2 = Console.ReadLine();

            var isVisit = new Boolean[str2.Length];
            if (str1.Length != str2.Length)
            {
                Console.WriteLine("String are not anagram! ");
            }
            for (var i = 0; i < str1.Length; i++)
            {
                for (var j = 0; j < str2.Length; j++)
                {
                    if (str1[i] == str2[j] && !isVisit[i])
                    {
                        isVisit[i] = true;
                        continue;
                    }
                }
            }
            var ischecked = true;
            for (int i = 0; i < isVisit.Length; i++)
            {
                if (!isVisit[i])
                {
                    ischecked = false;
                    break;
                }
            }
            if (ischecked)
            {
                Console.WriteLine("anagram!");
            }
            else
            {
                Console.WriteLine("non anagram !!");
            }

        }

        public static void Anagram1()
        {
            Console.WriteLine("Enter a string 1 : ");
            var str1 = Console.ReadLine();
            Console.WriteLine("Enter a string 2 : ");
            var str2 = Console.ReadLine();

            char[] arr1 = str1.ToCharArray();
            char[] arr2 = str2.ToCharArray();

            Array.Sort(arr1);
            str1 = arr1.ToString();
            //str1 = new string(arr1);
            Array.Sort(arr2);
            str2 = arr2.ToString();
            
            if (str1.Equals(str2))
            {
                Console.WriteLine("Anagram!!");
            }
            else
            {
                Console.WriteLine("Not Anagram!!");
            }
        }

    }
}
