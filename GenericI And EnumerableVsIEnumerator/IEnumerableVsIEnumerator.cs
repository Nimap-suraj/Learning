using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace March26
{
    internal class IEnumerableVsIEnumerator
    {
        static void Difference()
        {
            var list = new List<int>();
            list.Add(10);
            list.Add(20);
            list.Add(30);
            list.Add(40);
            list.Add(50);

            //IEnumerable<int> ienum = (IEnumerable<int>)list;
            //foreach (var i in ienum)
            //{
            //    Console.WriteLine(i);
            //}

            IEnumerator<int> numerator = list.GetEnumerator();

            while (numerator.MoveNext())
            {
                Console.WriteLine(numerator.Current.ToString());
            }
        }
    }
}
