using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Video : DigitalItem
    {
        public string? Quality { get; set; }
        public int Length { get; set; }

        public Video() : base() { }

        public Video(string name, string author, int id, string format, 
            double size, string quality, int length) : base(name, author, id, format, size)
        {
            Quality = quality;
            Length = length;
        }
    }
}
