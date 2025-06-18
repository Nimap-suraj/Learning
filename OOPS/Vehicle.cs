using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OOPS
{
    public class Vehicle
    {
        private string _make;
        private string _model;

        public string make
        {
            get => _make;
            set => _make = !string.IsNullOrEmpty(value) ? value : "Unknown";
        }
        public string model
        {
            get => _model;
            set => _model = !string.IsNullOrEmpty(value) ? value : "Unknown"; 
        }
      
        public Vehicle(string make,string model)
        {
            this._make = make;
            this._model = model;
        }
        public void DisplayVehicle()
        {
            Console.WriteLine($"Vehicle Model is : {make}, {model}");
        }
        public virtual void StartEngine()
        {
            Console.WriteLine("Starting the vehicle's engine");
        }
        //public abstract double CalculateTax();
    }
}
