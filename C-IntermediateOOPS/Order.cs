using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Intermediate
{
    internal class Order
    {
        public string food;
        public int price;

        public Order(string food,int price) 
        {
           this.food = food;
          this.price = price;
        }
    }
}
