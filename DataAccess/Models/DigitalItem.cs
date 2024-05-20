using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public abstract class DigitalItem : Item
    {
        public int Id { get; set; }
        public string? Format { get; set; }
        public double? Size { get; set; }

        public DigitalItem() : base() { }

        public DigitalItem(string name, List<string> authors, int id, string format, double size) : base(name, authors)
        {
            Id = id;
            Format = format;
            Size = size;
        }

    }
}
