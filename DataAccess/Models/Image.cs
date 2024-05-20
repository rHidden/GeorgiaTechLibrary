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

        public Image(string name, List<string> authors, int id, string format, double size, 
            Resolution resolution) : base(name, authors, id, format, size)
        {
            Resolution = resolution;
        }
    }
}
