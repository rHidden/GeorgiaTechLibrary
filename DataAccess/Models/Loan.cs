namespace DataAccess.Models
{
    public class Loan
    {
        public int Id { get; set; }
        public DateTime? LoanDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public DigitalItem? DigitalItem { get; set; }
        public BookInstance? BookInstance { get; set; }
        public User? User { get; set; }
        public LoanType? Type { get; set; }

        public Loan() { }

        public Loan(int id, DateTime loanDate, DateTime dueDate, DateTime returnDate, DigitalItem digitalItem, BookInstance bookInstance, User user, LoanType type)
        {
            Id = id;
            LoanDate = loanDate;
            DueDate = dueDate;
            ReturnDate = returnDate;
            DigitalItem = digitalItem;
            BookInstance = bookInstance;
            User = user;
            Type = type;
        }
    }

    public enum LoanType
    {
        Book,
        DigitalItem
    }
}
