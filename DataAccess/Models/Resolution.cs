using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Resolution
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public Resolution() { }

        public Resolution(int width, int height)
        {
            Width = width; Height = height;
        }
    }
}
