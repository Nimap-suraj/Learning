using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dependency_Injection.Models
{
    public class College : IEducational
    {
        public void Teach(Person person)
        {
            Console.WriteLine("Educate person in College");
        }
    }
}
