using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dependency_Injection.Models
{
    public class Hospital
    {
        public void Cure(Person person)
        {
            Console.WriteLine("cure person");
        }
    }
}
