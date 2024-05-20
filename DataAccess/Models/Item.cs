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
        public List<string>? Authors { get; set; }

        public Item() { }

        public Item(string name, List<string> authors)
        {
            Name = name;
            Authors = authors;
        }
    }
}
