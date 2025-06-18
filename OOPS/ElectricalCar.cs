using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPS
{
    internal class ElectricalCar : Car
    {
        public int BatteryCapacity { get; set; }
        public ElectricalCar(string make, string model, int door, int batteryCapacity   ) : base(make, model, door)
        {
            BatteryCapacity = batteryCapacity;
        }
    }
}
