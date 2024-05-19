namespace DataAccess.Models
{
    public abstract class Loan
    {
        public int Id { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public User? User { get; set; }

        public Loan() { }

        public Loan(int id, DateTime loanDate, DateTime returnDate, User user)
        {
            Id = id;
            LoanDate = loanDate;
            ReturnDate = returnDate;
            User = user;
        }
    }
}
