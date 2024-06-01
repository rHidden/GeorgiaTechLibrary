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

        public Image(DigitalItem digitalItem, Resolution resolution) : base(digitalItem)
        {
            Resolution = resolution;
        }
    }
}
