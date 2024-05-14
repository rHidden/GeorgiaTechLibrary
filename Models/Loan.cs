namespace GeorgiaTechLibrary.Models
{
    public class Loan
    {
        public int Id { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public BookInstance LoanBookInstance { get; set; }
        public User User { get; set; }
        public LoanType Type { get; set; }

        public Loan() { }

        public Loan(int id, DateTime loanDate, DateTime returnDate, BookInstance loanBookInstance, User user)
        {
            Id = id;
            LoanDate = loanDate;
            ReturnDate = returnDate;
            LoanBookInstance = loanBookInstance;
            User = user;
        }
    }

    public enum LoanType
    {
        Book,
        DigitalItem
    }
}
