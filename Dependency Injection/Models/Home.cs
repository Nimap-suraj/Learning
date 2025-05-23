using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dependency_Injection.Models
{
    public class Home
    {
        public void ProvideShelter(Person person)
        {
            Console.WriteLine("Stay home.");
        }
    }
}
