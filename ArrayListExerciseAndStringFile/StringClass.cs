using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArrayListExercise
{
    internal class StringClass
    {
        public static void StringClassLecture()
        {
            string fullname = "sunil shetty";
            //Console.WriteLine(String.Format("Trim: '{0}'",fullname.Trim()));
            //// suraj shah
            //var index = fullname.IndexOf(' ');
            //var firstname = fullname.Substring(0,index);
            //Console.WriteLine();
            //var lastname = fullname.Substring(index+1);

            //Console.WriteLine(String.Format("FIrstName: {0} LastName: {1}",firstname,lastname));

            //var names = fullname.Split(' ');
            //Console.WriteLine(names[0]); 
            //Console.WriteLine(names[1]); 

            // Currency
            float price = 1243;
            Console.WriteLine(price.ToString("C"));


        }
    }
}
