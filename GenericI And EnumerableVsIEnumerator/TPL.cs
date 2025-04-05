using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace March26
{
    internal class TPL
    {
        public static void TplClass()
        {
            //Thread t1 = new Thread(RunMillionIteratiom);
            //t1.Start();
            Parallel.For(0, 1000000, x => RunMillionIteratiom());
            Console.Read();
        }
        static void RunMillionIteratiom()
        {
            for (int i = 0; i < 1000000; i++)
            {
                Console.WriteLine(i);
            }
        }
    }
}
