//using System;
//using System.Threading;

//class Program
//{
//    static void Main()
//    {
//        Console.CursorVisible = false;
//        ShowGameStyleLoader("Loading Data", 100); // Simulate 100 steps

//        Console.WriteLine("\n\n✅ Data loading complete!");
//        Console.CursorVisible = true;
//    }

//    static void ShowGameStyleLoader(string message, int totalSteps)
//    {
//        Console.WriteLine(message);
//        int progressBarWidth = 50;

//        for (int i = 0; i <= totalSteps; i++)
//        {
//            double percentage = (double)i / totalSteps;
//            int filledBars = (int)(percentage * progressBarWidth);
//            string bar = new string('█', filledBars).PadRight(progressBarWidth, '-');

//            Console.Write($"\r[{bar}] {percentage * 100:0}%");

//            Thread.Sleep(30); // Simulate time delay for loading (replace with real logic)
//        }
//    }
//}
