namespace DataAccess.Models{
    
    public class DigitalItemLoan : Loan 
    {
        public DigitalItem? DigitalItem { get; set; }

        public DigitalItemLoan() { }

        public DigitalItemLoan(int id, DateTime loanDate, DateTime returnDate, User user, DigitalItem book) 
            : base(id, loanDate, returnDate, user)
        {
            DigitalItem = book;
        }
    } 
}