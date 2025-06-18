using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPS
{
    public class Car : Vehicle
    {
        public int NumberOfDoors { get; set; }
        //private int NumberOfDoors; can't access 
        public Car(string make, string model, int door) : base(make, model)
        {
            this.NumberOfDoors = door;
        }
        public void DisplayDoors()
        {
            Console.WriteLine($"This car having {NumberOfDoors} doors");
        }
        public virtual void StartEngine()
        {
            Console.WriteLine("Starting the Car's engine");
        }
    }
}
