using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Audio : DigitalItem
    {
        public int? Length { get; set; }

        public Audio() { }

        public Audio(string name, string author, int id, string format, double size, 
            int length) : base(name, author, id, format,size)
        {
            Length = length;
        }
    }
}
