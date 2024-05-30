namespace DataAccess.Models
{
    public class Loan
    {
        public int Id { get; set; }
        public DateTime? LoanDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public User? User { get; set; }

        public Loan() { }

        public Loan(int id, DateTime loanDate, DateTime dueDate, DateTime returnDate, User user)
        {
            Id = id;
            LoanDate = loanDate;
            DueDate = dueDate;
            ReturnDate = returnDate;
            User = user;
        }

        public Loan(Loan loan)
        {
            Id = loan.Id;
            LoanDate = loan.LoanDate;
            DueDate = loan.DueDate;
            ReturnDate = loan.ReturnDate;
            User = loan.User;
        }
    }
    public enum LoanType { 
        Book,
        DigitalItem
    }
}
