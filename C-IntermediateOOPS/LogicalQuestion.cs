using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Intermediate
{
    internal static class LogicalQuestion
    {
       /* public static void ReverseString()
        {
            Console.Write("Enter a String: ");
            var input = Console.ReadLine();
            //Console.WriteLine(input);
            
            char[] array = input.ToCharArray();
            //Array.Reverse(array);
            int start = 0;
            int end = array.Length - 1;
            while (start < end)
            {
                var temp = array[start];
                array[start] = array[end];
                array[end] = temp;
                start++;
                end--;
            }
            var st = new string(array);
            Console.WriteLine("Reverse String is "+st );
        }
       */
        public static bool palindrome()
        {
            Console.WriteLine("enter string: ");
            var input = Console.ReadLine();
            // Type Inference
            var  array = input.ToCharArray();
            for(int i = 0; i <  array.Length / 2 ; i++)
            {
                if (array[i]  != array[array.Length - i - 1])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
