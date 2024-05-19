namespace GeorgiaTechLibrary.DTOs
{
    public class BookInstanceDTO
    {
        public int Id { get; set; }
        public bool IsLoaned { get; set; }
        public BookDTO Book { get; set; }
    }
}
