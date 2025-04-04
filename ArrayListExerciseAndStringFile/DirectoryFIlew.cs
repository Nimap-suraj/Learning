using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArrayListExercise
{
    internal class DirectoryFIlew
    {
        public static void Dir()
        {
            Directory.CreateDirectory(@"C:\Users\DELL\Documents\Shah");
            var path = @"C:\Users\DELL\Documents\Shah";
            if (Directory.Exists(path))
            {
                Console.WriteLine("File Are there !!! Don't worry");
            }
        }
    }
}
