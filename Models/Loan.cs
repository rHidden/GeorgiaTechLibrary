namespace GeorgiaTechLibrary.Models
{
    public class Loan
    {
        public int Id { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public List<BookInstance> BookInstances { get; set; }
        public User User { get; set; }
        public LoanType Type { get; set; }

        public Loan() { }

        public Loan(int id, DateTime loanDate, DateTime returnDate, List<BookInstance> bookInstances, User user)
        {
            Id = id;
            LoanDate = loanDate;
            ReturnDate = returnDate;
            BookInstances = bookInstances;
            User = user;
        }
    }

    public enum LoanType
    {
        Book,
        DigitalItem
    }
}
