using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Text : DigitalItem
    {
        public Text() { }

        public Text(string name, string author, int id, string format, 
            double size) : base(name, author, id, format, size) { }
    }
}
