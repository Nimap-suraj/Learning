using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace March26
{
    internal class SampleCollection
    {
        private string[] str = new string[5]; // string of names
        public string this[int index]
        {
            get { return str[index]; }
            set { str[index] = value; }
        }

    }
}
