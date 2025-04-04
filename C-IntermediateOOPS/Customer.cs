using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Intermediate
{
    internal class Customer
    {
        public int id;
        public string name;
        public readonly List<Order> Orders = new List<Order>();
        public Customer(int id)
        {
            this.id = id;
        }
        public Customer(int id, string name)
            : this(id)
        {
            this.name = name;
        }
    }
}

