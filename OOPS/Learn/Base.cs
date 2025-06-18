using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPS.Learn
{
    public class Base
    {
        public int Add(int num1,int num2)
        {
            return num1 + num2;
        }
        public int Add(double n1,double n2)
        {
            return (int)(n1 + n2);
        }
    }
}
