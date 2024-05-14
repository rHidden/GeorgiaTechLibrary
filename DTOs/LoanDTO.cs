namespace GeorgiaTechLibrary.DTOs
{
    public class LoanDTO
    {
        public int Id { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public BookInstanceDTO LoanBookInstance { get; set; }
        public UserDTO User { get; set; }
        public LoanType Type { get; set; }
    }

    public enum LoanType
    {
        Book,
        DigitalItem
    }
}
