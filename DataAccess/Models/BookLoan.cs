namespace DataAccess.Models{

   public class BookLoan : Loan
   {
       public BookInstance? BookInstance { get; set; }

       public BookLoan() { }

       public BookLoan(int id, DateTime loanDate, DateTime dueDate, DateTime returnDate, User user, BookInstance bookInstance) 
           : base(id, loanDate, dueDate, returnDate, user)
       {
           BookInstance = bookInstance;
       }

        public BookLoan(Loan loan, BookInstance bookInstance)
           : base(loan)
        {
            BookInstance = bookInstance;
        }
    }
}
