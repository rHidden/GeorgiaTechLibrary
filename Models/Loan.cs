namespace GeorgiaTechLibrary.Models
{
    public class Loan
    {
        public int Id { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public List<BookInstance> BookInstances { get; set; }
        public List<User> Users { get; set; }

        public Loan() { }

        public Loan(int id, DateTime loanDate, DateTime returnDate, List<BookInstance> bookInstances, List<User> users)
        {
            Id = id;
            LoanDate = loanDate;
            ReturnDate = returnDate;
            BookInstances = bookInstances;
            Users = users;
        }
    }
}
