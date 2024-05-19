namespace DataAccess.Models
{
    public class BookInstance: Item
    {
        public int Id { get; set; }
        public Book? Book { get; set; }
        public bool? IsLoaned { get; set; }

        public BookInstance() { }

        public BookInstance(int id, Book book, bool isLoaned)
        {
            Id = id;
            Book = book;
            IsLoaned = isLoaned;
        }
    }
}
