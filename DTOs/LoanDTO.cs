namespace GeorgiaTechLibrary.DTOs
{
    public class LoanDTO
    {
        public int LoanId { get; set; }
        public int BookInstanceId { get; set; }
        public int DigitalItemId { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}
