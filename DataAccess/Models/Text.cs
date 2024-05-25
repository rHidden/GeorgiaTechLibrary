namespace DataAccess.Models
{
    public class Text : DigitalItem
    {
        public Text() { }

        public Text(string name, List<string> authors, int id, string format, 
            double size) : base(name, authors, id, format, size) { }
        public Text(DigitalItem digitalItem) : base(digitalItem) { }
    }
}
