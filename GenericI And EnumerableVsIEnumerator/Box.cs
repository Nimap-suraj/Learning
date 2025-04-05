using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace March26
{
    internal class Box<T>
    {
        public T data;
        public void Add(T value)=>data = value;
        public T GetData() => data;
    }
}
