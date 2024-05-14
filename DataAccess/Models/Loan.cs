namespace DataAccess.Models
{
    public class Loan
    {
        public int Id { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public Item Item { get; set; }
        public User? User { get; set; }
        public LoanType Type { get; set; }

        public Loan() { }

        public Loan(int id, DateTime loanDate, DateTime returnDate, Item item, User user)
        {
            Id = id;
            LoanDate = loanDate;
            ReturnDate = returnDate;
            Item = item;
            User = user;
        }
    }

    public enum LoanType
    {
        Book,
        DigitalItem
    }
}
