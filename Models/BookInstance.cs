namespace GeorgiaTechLibrary.Models
{
    public class BookInstance
    {
        public int Id { get; set; }
        public Book Book { get; set; }
        public Library Library { get; set; }
        public bool IsLoaned { get; set; }

        public BookInstance() { }

        public BookInstance(int id, Book book, Library library, bool isLoaned)
        {
            Id = id;
            Book = book;
            Library = library;
            IsLoaned = isLoaned;
        }
    }
}
