using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArrayListExercise
{
    internal class liveCoding
    {
        public static string cutStringTOLimit(string text, int maxLength)
        {
            if (text.Length < maxLength)
            {
                return text;
            }
            //text
            var data = text.Split(' ');
            var str = new List<string>();
            int counter = 0;
            foreach (var charcter in data)
            {
                str.Add(charcter);
                counter += charcter.Length;
                if (counter > maxLength)
                {
                    break;
                }
            }
            return string.Join(" ", str) + "...";
        }
    }
}
