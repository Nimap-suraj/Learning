using System;
using System.Collections.Generic;

namespace ArrayListExercise
{
    internal class Program
    {
       public static void Main(string[] args)
        {
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

       /* public static List<int> GetSmallest(List<int> list, int count)
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
            for(int i = 1; i < list.Count; i++)
            {
                if(list[i] < min)
                {
                    min = list[i];
                }
            }
            return min;
            //throw new NotImplementedException();  
        }*/
    }
}
