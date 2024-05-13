namespace GeorgiaTechLibrary.DTOs
{
    public class BookInstanceDTO
    {
        public int Id { get; set; }
        public int BookISBN { get; set; }
        public string LibraryName { get; set; }
        public bool IsLoaned { get; set; }
    }
}
