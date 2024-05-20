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

        public Audio(string name, List<string> authors, int id, string format, double size, 
            int length) : base(name, authors, id, format,size)
        {
            Length = length;
        }
    }
}
