namespace DataAccess.Models
{
    public class Book: Item
    {
        public string? ISBN { get; set; }
        public string? Description { get; set; }
        public string? SubjectArea { get; set; }
        public BookStatus? Status { get; set; }

        public Book() { }

        public Book(string name, List<string> authors, string isbn, string description, 
            string subjectArea, BookStatus status) : base(name, authors)
        {
            ISBN = isbn;
            Description = description;
            SubjectArea = subjectArea;
            Status = status;
        }

        public enum BookStatus
        {
            loanable,
            unloanable,
            obtain,
        }
    }
}
