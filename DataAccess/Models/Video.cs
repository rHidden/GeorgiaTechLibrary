using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Video : DigitalItem
    {
        public Resolution Resolution { get; set; }
        public int Length { get; set; }

        public Video() : base() { }

        public Video(string name, List<string> author, int id, string format, 
            double size, Resolution resolution, int length) : base(name, author, id, format, size)
        {
            Resolution = resolution;
            Length = length;
        }

        public Video(DigitalItem digitalItem, Resolution resolution, int length) : base(digitalItem)
        {
            Resolution = resolution;
            Length = length;
        }
    }
}
