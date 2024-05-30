namespace DataAccess.Models
{
    public class BookInstance
    {
        public int Id { get; set; }
        public bool? IsLoaned { get; set; }
        public Book? Book { get; set; }

        public BookInstance() { }

        public BookInstance(int id, bool isLoaned, Book book)
        {
            Id = id;
            IsLoaned = isLoaned;
            Book = book;
        }
    }
}
