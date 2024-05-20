namespace DataAccess.Models{

   public class BookLoan : Loan
   {
       public BookInstance? Book { get; set; }

       public BookLoan() { }

       public BookLoan(int id, DateTime loanDate, DateTime dueDate, DateTime returnDate, User user, BookInstance book) 
           : base(id, loanDate, dueDate, returnDate, user)
       {
           Book = book;
       }
   }
}
