using System;
internal static class Program
{
    private static void Main(string[] args)

    {
        //Console.BackgroundColor = ConsoleColor.Blue;
        //Console.ForegroundColor = ConsoleColor.White;
        //Console.ForegroundColor = ConsoleColor.Green;
        //Console.Title = "C#BASICS";
        //Console.WriteLine("BackgroundColor: Blue");
        //Console.WriteLine("ForegroundColor: White");
        //Console.WriteLine("Title: Understanding Console Class");

        //Console.WriteLine("Enter a Key");
        //int var1 = Console.Read();
        //Console.WriteLine($"ASCII Value of the Entered Key is: {var1}");

        //Console.WriteLine("Enter Another Key");
        //ConsoleKeyInfo var2 = Console.ReadKey();
        //Console.WriteLine(var2);
        Console.WriteLine(byte.MinValue);
        Console.WriteLine(byte.MaxValue);

        Console.WriteLine(sbyte.MaxValue);
        Console.WriteLine(sbyte.MinValue);
        //Console.WriteLine($"\nEntered Key: {var2.Key} KeyChar:{var2.KeyChar} ASCII:{(int)var2.KeyChar}");


    }
    public static void Basic()
    {
        // Console.BackgroundColor = ConsoleColor.Blue;
        // Console.Clear();


        //Console.Write("enter a char");
        //var a = Console.Read();
        //Console.WriteLine(a);  // read will return a ASCII vlaue 

        //Console.Beep(); make a sound beep from a speaker
        //Console.Title = "CODING";
        //Console.ResetColor();
    }
    public static void MultipleSortMethod()
    {
        int[] arr = new int[5] { 50, 40, 30, 20, 10 };
        Console.WriteLine("Before Sorting....");
        foreach (var num in arr)
        {
            Console.Write(num + " ");
        }
        Console.WriteLine("\n\nAfter Sorting...");
        bubbleSort(arr);
        Array.Sort(arr);
        Array.Sort(arr, new Comparison<int>((i1, i2) => i1.CompareTo(i2)));
        Array.Sort(arr, delegate (int a, int b) { return a - b; });
        foreach (var num in arr)
        {
            Console.Write(num + " ");
        }
    }
    public static void bubbleSort(int[] arr)
    {
        for(int i = 0; i < arr.Length - 1; i++)
        {
            for(int j = 0;j < arr.Length - i - 1; j++)
            {
                if (arr[j] > arr[j  + 1])
                {
                    int temp = arr[j];
                    arr[j] = arr[j + 1];
                    arr[j + 1] = temp;  
                }
            }
        }
    }
}
