using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Resolution
    {
        public string? Width { get; set; }

        public string? Height { get; set; }

        public Resolution() { }

        public Resolution(string width, string height)
        {
            Width = width; Height = height;
        }
    }
}
