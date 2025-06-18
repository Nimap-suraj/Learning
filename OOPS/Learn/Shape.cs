using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPS.Learn
{
    public class Shape
    {
        public int width {  get; set; }
        public int height { get; set; }

        public void SetWidth(int w)
        {
            width = w;
        }
        public void SetHeight(int h)
        {
            height = h;
        }
    }
    public class Rectangle : Shape
    {
        public int GetArea()
        {
            return width* height;
        }
    }
}
