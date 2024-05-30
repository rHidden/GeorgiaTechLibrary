using static DataAccess.Models.Book;

namespace GeorgiaTechLibrary.DTOs
{
    public class BookDTO
    {
        public string ISBN { get; set; }
        public string Description { get; set; }
        public string SubjectArea { get; set; }
        public BookStatus Status { get; set; }
    }
}
