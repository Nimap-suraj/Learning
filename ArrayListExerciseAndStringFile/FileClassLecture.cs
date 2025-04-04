using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArrayListExercise
{
    internal class FileClassLecture
    {
        public static void FileCLass() 
        {
            var path = @"C:\Users\DELL\Documents\Suraj Documents\data.txt";
            var copyPath = @"C:\Users\DELL\Documents\date.txt";
            if (File.Exists(path))
            {
                Console.WriteLine("file is present boss");
            }
            else
            {
                Console.WriteLine("I don't know where file is or something went wrong!!");
            }
                //File.Copy(path, copyPath);
            var content = File.ReadAllText(path);
            Console.WriteLine(content);
            File.Delete(copyPath);
        }
    }
}
