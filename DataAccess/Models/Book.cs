namespace DataAccess.Models
{
    public class Book : Item
    {
        public string? ISBN { get; set; }
        public string? Description { get; set; }
        public string? SubjectArea { get; set; }
        public bool CanLoan { get; set; }

        public Book() { }

        public Book(string name, string author, string isbn, string description, 
            string subjectArea, bool canLoan) : base(name, author)
        {
            ISBN = isbn;
            Description = description;
            SubjectArea = subjectArea;
            CanLoan = canLoan;
        }
    }
}
