namespace GeorgiaTechLibrary.Models
{
    public class Book
    {
        public string ISBN { get; set; }
        public string Description { get; set; }
        public string SubjectArea { get; set; }
        public bool CanLoan { get; set; }

        public Book() { }

        public Book(string isbn, string description, string subjectArea, bool canLoan, List<BookInstance> bookInstances)
        {
            ISBN = isbn;
            Description = description;
            SubjectArea = subjectArea;
            CanLoan = canLoan;
        }
    }
}
