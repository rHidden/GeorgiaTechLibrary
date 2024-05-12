namespace GeorgiaTechLibrary.Models
{
    public class BookInstance
    {
        public int Id { get; set; }
        public bool Loaned { get; set; }

        public BookInstance() { }

        public BookInstance(int id, bool loaned)
        {
            Id = id;
            Loaned = loaned;
        }
    }
}
