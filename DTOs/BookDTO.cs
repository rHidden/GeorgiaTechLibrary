namespace GeorgiaTechLibrary.DTOs
{
    public class BookDTO
    {
        public string ISBN { get; set; }
        public bool CanLoan { get; set; }
        public string Description { get; set; }
        public string SubjectArea { get; set; }
        public List<BookInstanceDTO> BookInstances { get; set; }
    }
}
