namespace GeorgiaTechLibrary.Models
{
    public class BookInstance
    {
        public int Id { get; set; }
        public bool IsLoaned { get; set; }

        public BookInstance() { }

        public BookInstance(int id, bool isLoaned)
        {
            Id = id;
            IsLoaned = isLoaned;
        }
    }
}
