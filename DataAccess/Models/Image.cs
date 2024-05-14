using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Image : DigitalItem
    {
        public Resolution? Resolution { get; set; }

        public Image() { }

        public Image(string name, string author, int id, string format, double size, 
            Resolution resolution) : base(name, author, id, format, size)
        {
            Resolution = resolution;
        }
    }
}
