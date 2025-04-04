using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Intermediate
{
    public class Person
    {
        public string Name;
        public void Introduce()
        {
            Console.WriteLine("Hello my name is "+ Name);
        }
        public static Person Parse(string str)
        {
            var person = new Person();
            person.Name = str;
            return person;
        }
    }
}
