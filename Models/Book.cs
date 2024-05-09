namespace GeorgiaTechLibrary.Models
{
    public class Book
    {
        public int ISBN { get; set; }
        public bool CanLoan { get; set; }
        public string Description { get; set; }
        public string SubjectArea { get; set; }

    }
}
