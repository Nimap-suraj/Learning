using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace March26
{
    internal class Generic
    {
        public static void genericLec()
        {
            int[] intArray = new int[3] { 10, 30, 40 };
            string[] stringsArray = new string[3] { "apple", "banana", "kela" };
            genericRec(intArray);
            //genericRec(stringsArray);
            Console.WriteLine();

            genericRec(stringsArray);
        }
        public static void genericRec<Thing>(Thing[] arr)
        {
            foreach (Thing t in arr)
            {
                Console.Write(t + " ");
            }
        }
    }
}
