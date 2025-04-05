using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArrayListExercise
{
    class LogicalQuestion
    {
        public static void Reverse()
        {
            int rev = 0;
            Console.Write("Enter a Number: ");
            int number = Convert.ToInt32(Console.ReadLine());
            int temp = number;
            while (number > 0)
            {
                rev = rev * 10 + number % 10;
                number /= 10;
            }
            Console.WriteLine($" Reverse Number of this {temp} is {rev}");

        }
        public static int RecurssionReverseNumber(int number, int digit)
        {
            if (number < 10) return 1;
            int lastDigit = number % 10;
            return lastDigit * (int)Math.Pow(10, digit - 1) + RecurssionReverseNumber(number / 10, digit - 1);
        }
        public static void SecondMax()
        {
            int[] arr = new int[] { 40, 41, 39, 38, 12, 35, 1, 10, 34, 1, 35 };
            int max = 0;
            foreach (int num in arr)
            {
                if (num > max)
                {
                    max = num;
                }
            }
            Console.WriteLine($"Max: {max}");
            int secondMax = -1;
            foreach (int num in arr)
            {
                if (num != max && num > secondMax)
                {
                    secondMax = num;
                }
            }
            Console.WriteLine($"secondMax: {secondMax}");
        }
        public static void swapMethod(ref int a, ref int b)
        {
            int temp = a; // temp = 10
            a = b; // a = 20
            b = temp; // b = 10;
        }
        public static void swapMethod2(ref int a, ref int b)
        {
            a = a + b; // 10 + 20 = 30
            b = a - b; // 30 - 20 = 10
            a = a - b; // 30 - 10 = 20
        }
        public static void swapMethod3(ref int a, ref int b)
        {
            a = a ^ b; // 10 + 20 = 30
            b = a ^ b; // 30 - 20 = 10
            a = a ^ b; // 30 - 10 = 20
        }
        public static void isAnagram1()
        {

            string str1 = "gum";
            string str2 = "mug";

            // Check if lengths are the same first
            if (str1.Length != str2.Length)
            {
                Console.WriteLine("Not Anagram");
                return;
            }

            bool[] isVisit = new bool[str2.Length];
            bool isCheck = true;

            for (int i = 0; i < str1.Length; i++)
            {
                bool foundMatch = false;
                for (int j = 0; j < str2.Length; j++)
                {
                    if (str1[i] == str2[j] && !isVisit[j])
                    {
                        isVisit[j] = true;
                        foundMatch = true;
                        break;
                    }
                }
                if (!foundMatch)
                {
                    isCheck = false;
                    break;
                }
            }

            // Check if all characters in str2 were matched
            for (int i = 0; i < isVisit.Length; i++)
            {
                if (!isVisit[i])
                {
                    isCheck = false;
                    break;
                }
            }

            if (isCheck)
            {
                Console.WriteLine("Anagram");
            }
            else
            {
                Console.WriteLine("Not Anagram");
            }

        }
        public static bool checkAnaram(string s1, string s2)
        {
            if (s1.Length != s2.Length)
            {
                return false;
            }

            int[] charArray = new int[256];

            for (int i = 0; i < s1.Length; i++)
            {
                charArray[s1[i]]++;
            }
            for (int i = 0; i < s2.Length; i++)
            {
                charArray[s2[i]]--;

                if (charArray[s2[i]] < 0)
                {
                    return false;
                }
            }
            for (int i = 0; i < 256; i++)
            {
                if (charArray[i] != 0)
                {
                    return false;
                }
            }
            return true;
        }
        public static string ReverseString(string s1)
        {
            // return s1.ToUpper();
            var arr = s1.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }
        public static string ReverseString2(string s1)
        {
            if (s1 == null) return null;
            var arr = s1.ToCharArray();
            int len = arr.Length - 1;
            for (int i = 0; i < len; i++, len--)
            {
                arr[i] ^= arr[len];
                arr[len] ^= arr[i];
                arr[i] ^= arr[len];
            }
            return new string(arr);
        }
        public static int MissingNumber1(int[] arr)
        {
            int ans = 0;
            for (int i = 1; i <= 8; i++)
            {
                if (!arr.Contains(i))
                {
                    ans = i;
                }
            }
            return ans;
        }
        public static int MissingNumber2(int[] arr)
        {
            int ans = 0;
            for (int i = 1; i <= 8; i++)
            {
                if (i != arr[i])
                {
                    ans = i;
                }
            }
            return ans;
        }
        public static List<int> MultipleMissingNumber3(int[] arr)
        {
            var list = new List<int>();
            int ans = 0;
            for (int i = 1; i <= 12; i++)
            {
                if (!arr.Contains(i))
                {
                    list.Add(i);
                }
            }
            return list;
        }
        public static void isPalindrom1()
        {
            string str = "abbaa";
            // palindrome number 
            var arr = str.ToCharArray();
            Array.Reverse(arr);
            // reverse store nahi karna 
            var rev = new string(arr);
            if (rev == str)
            {
                Console.WriteLine("palindrome");
            }
            else
            {
                Console.WriteLine("Non palindrome");
            }
        }
        public static bool isPalindrom2(string str)
        {
            int start = 0;
            int end = str.Length - 1;
            while (start < end)
            {
                if (str[start] != str[end])
                {
                    return false;
                }
                start++;
                end--;
            }
            return true;
        }
        public static void MainFibonacciMethod()
        {
            int n = 5;
            for (int i = 0; i < 5; i++)
            {
                Console.Write(FibonacciWithRecursion(i) + " ");
            }
        }
        public static void FibonacciWithOutRecursion()
        {
            // 0 1 1 2 3 5 8
            int a = 0;
            int b = 1;
            Console.Write($" {a} {b} ");
            for (int i = 3; i <= 5; i++)
            {
                int c = a + b;
                Console.Write(" " + c + " ");

                a = b;
                b = c;
            }
        }
        public static int FibonacciWithRecursion(int n)
        {
            if (n <= 1) return n;
            return FibonacciWithRecursion(n - 1) + FibonacciWithRecursion(n - 2);
        }
        public static void EvenNumberList()
        {
            List<int> list = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8 };
            List<int> newList = new List<int>();
            for (int i = 1; i <= list.Count; i++)
            {
                if (i % 2 == 0)
                {
                    newList.Add(i);
                }
            }
            foreach (var num in newList)
            {
                Console.Write(num + " ");
            }
        }
        public static void EvenNumberList2()
        {
            List<int> list = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8 };
            List<int> newList = new List<int>();
            for (int i = 1; i <= list.Count; i++)
            {
                if ((i & 1) == 0)
                {
                    newList.Add(i);
                }
            }
            foreach (var num in newList)
            {
                Console.Write(num + " ");
            }
        }
        public static void PrimeNumber()
        {
            for (int i = 1; i <= 10; i++)
            {
                if (IsPrime(i))
                {
                    Console.WriteLine(i);
                }
            }
        }
        public static bool IsPrime(int n)
        {
            if (n <= 1) return false;
            if (n == 2) return true;
            if (n % 2 == 0) return false;
            for (int i = 3; i * i <= n; i++)
            {
                if (n % i == 0)
                {
                    return false;
                }
            }
            return true;
        }
        public static void Sqrt()
        {
            int answer = SqrtNumber3();
            Console.WriteLine($"Square Root: {answer}");
        }
        public static void SqrtNumber1()
        {
            int number = 16;
            Console.WriteLine(Math.Sqrt(number));
        }
        public static void SqrtNumber2()
        {
            int n = 25;
            // target 
            int ans = 1;
            for (int i = 1; i <= n; i++)
            {
                if (i * i <= n)
                {
                    ans = i;
                }
                else
                {
                    break;
                }
            }
            Console.WriteLine($"Answer: {ans}");
        }
        public static int SqrtNumber3()
        {
            Console.WriteLine("Enter a Number: ");
            int number = Convert.ToInt32(Console.ReadLine());
            int low = 1;
            int high = number;
            int ans = 1;
            while (low <= high)
            {
                int mid = low + (high - low) / 2;
                if (mid * mid > number)
                {
                    high = mid - 1;
                }
                else if (mid * mid <= number)
                {
                    ans = mid;
                    low = mid + 1;
                }
            }
            return high;
        }
        public static void DuplicateNumber1()
        {
            int[] arr = new int[] { 1, 1, 2, 2, 3, 3, 4 };
            var li = new List<int>();

            // Duplicates
            for (int i = 0; i < arr.Length; i++)
            {
                int count = 0;
                for (int j = 0; j < arr.Length; j++)
                {
                    if (arr[i] == arr[j])
                    {
                        count++;
                    }
                }
                if (count == 2 && !li.Contains(arr[i]))
                {
                    li.Add(arr[i]);
                }

            }
            foreach (var num in li)
            {
                Console.Write(num + " ");
            }
        }
        public static void DuplicateNumber2()
        {
            int[] arr = new int[] { 1, 1, 1, 2, 2, 2, 4, 4, 4, 10 };
            var frequency = new Dictionary<int, int>();
            var list = new List<int>();
            foreach (var num in arr)
            {
                if (frequency.ContainsKey(num))
                {
                    frequency[num]++;
                }
                else
                {
                    frequency[num] = 1;
                }
            }
            foreach (var k in frequency)
            {
                if (k.Value > 1)
                {
                    list.Add(k.Key);
                }
            }
            foreach (var num in list)
            {
                Console.Write(num + " ");
            }
        }

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
