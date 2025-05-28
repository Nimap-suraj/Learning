using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSA
{
    public static class Day1
    {
        public static void TwoDimensionalArray()
        {
            // int[] arr = new int[6] { 10, 20, 30, 4, 5 ,6};
            // for(int i = 0; i < arr.Length; i++)
            // {
            //     Console.WriteLine(arr[i]);
            // }
            int[,] multiArr = new int[2, 3]{
            {1,2,3},
            {4,5,6}
        };
            // Console.WriteLine(multiArr.GetLength(0)); 
            // Console.WriteLine(multiArr.GetLength(1));// 2
            for (int j = 0; j < multiArr.GetLength(0); j++)
            {
                for (int i = 0; i < multiArr.GetLength(1); i++)
                {
                    Console.Write(multiArr[j, i] + " ");
                }
                Console.WriteLine();
            }
        }
        public static void JaggedArrays()
        {
            int[][] arr = new int[3][];
            arr[0] = new int[2] { 1, 2 };
            arr[1] = new int[3] { 10, 20, 30 };
            arr[2] = new int[4] { 10, 20, 11, 243 };
            for (int i = 0; i < arr.Length; i++)
            {
                for (int j = 0; j < arr[i].Length; j++)
                {
                    Console.Write(arr[i][j] + " ");
                }
                Console.WriteLine();
            }
        }
        public static void ImplicitTypedArray()
        {
            var arr = new[] { 1, 2, 3, 4 };
            for (int i = 0; i < arr.Length; i++)
            {
                Console.Write(arr[i] + " ");
            }
        }
        public static void InlineArrays()
        {
            var names = new[]
            {
            "suraj",
            "mahesh",
            "rajesh",
            "nakka",
            "gwgeg",
            "ahwhehe"
        };
            foreach (var name in names)
            {
                Console.Write(name + " ");
            }
        }
    }
}
