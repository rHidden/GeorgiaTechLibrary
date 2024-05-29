namespace DataAccess.Models
{
    public class BookReservation
    {
        public User? User { get; set; }
        public Book? Book { get; set; }
        public DateTime ReservationDate { get; set; }

        public BookReservation() { }

        public BookReservation(User user, Book book, DateTime reservationDate)
        {
            User = user;
            Book = book;
            ReservationDate = reservationDate;
        }
    }
}
