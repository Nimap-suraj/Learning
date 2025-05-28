using DSA;
using System;
using System.Collections;
internal class Program
{
    private static void Main(string[] args)
    {
        //   HelloWorld h = new HelloWorld();
        //   h.TwoDimensionalArray();
        // var obj=new HelloWorld();
        //obj.TwoDimensionalArray();
        // HelloWorld.JaggedArrays();
        // HelloWorld.ImplicitTypedArray();
        // HelloWorld.InlineArrays();
        int[] num = { 10, 101, 122 };
        num[0] = 1000;
        // Console.WriteLine(num[0]);
        foreach (var m in num)
        {
            Console.Write(m + " ");
        }
        Console.WriteLine();
        // ICollection collection = (ICollection)num;
        // Console.WriteLine($"Length: {collection.Count}");

        IList list = (IList)num;
        // Console.WriteLine($"Contains: {list.Contains(10)}");
        //   Console.WriteLine($"Index of an Element: {list.IndexOf(1000)}");
        // try {
        //     // list.Add(20); // Not allowed for arrays
        //     list.Clear();   // This line throws NotSupportedException
        // } catch (NotSupportedException e) {
        //     Console.WriteLine("Exception: " + e.Message);
        // }
        try
        {
            list.Insert(2, 1000);   // This line throws NotSupportedException
        }
        catch (NotSupportedException e)
        {
            Console.WriteLine("Exception: " + e.Message);
        }
        // ICollection collection1 = (ICollection)num;
        // Console.WriteLine($"Length: {collection1.Count}");
        // Remove , RemoveAt throws an Exception 

        //HelloWorld.TwoDimensionalArray();
    }
}