namespace DataAccess.Models
{
    public class Resolution
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public Resolution() { }

        public Resolution(int width, int height)
        {
            Width = width; Height = height;
        }
    }
}
