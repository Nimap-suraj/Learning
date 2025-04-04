using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArrayListExercise
{
    internal class Procedural
    {
        public static void ProceduralCoding()
        {
            Console.Write("Write a name: ");
            var name = Console.ReadLine();
            Console.WriteLine("Your name is " + name);
            var array = new char[name.Length];
            for(int i = name.Length; i > 0; i--)
            {
                array[name.Length-i] = name[i-1];
            }

            var revered = new string(array);
            Console.WriteLine("Revered Array : "+revered);
        }
        }
}
