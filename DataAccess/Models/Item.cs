using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public abstract class Item
    {
        public string? Name { get; set; }
        public string? Author { get; set; }

        public Item() { }

        public Item(string name, string author)
        {
            Name = name;
            Author = author;
        }
    }
}
