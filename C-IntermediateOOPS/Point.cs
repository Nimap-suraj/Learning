using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Intermediate
{
    internal class Point
    {
        // data member
        public int X;
        public int Y;

        // Constructor
        public Point(int x,int y)
        {
            this.X = x;
            this.Y = y;
        }

        // method
        public void Move(int x,int y)
        {
            this.X = x;
            this.Y = y;
        }
        // accepting a object
        public void Move(Point newLocation)
        {
            if(newLocation == null)
            {
                throw new ArgumentNullException("newLocation");
            }
            Move(newLocation.X,newLocation.Y);
        }
    }
}
