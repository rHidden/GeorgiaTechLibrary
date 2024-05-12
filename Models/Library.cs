namespace GeorgiaTechLibrary.Models
{
    public class Library
    {
        public string Name { get; set; }
        public List<User> Users { get; set; }
        public Address LibraryAddress { get; set; }
        public BookInstance BookInstance { get; set; }

        public Library() { }

        public Library(string name, List<User> users, Address libraryAddress, BookInstance bookInstance)
        {
            Name = name;
            Users = users;
            LibraryAddress = libraryAddress;
            BookInstance = bookInstance;
        }
    }
}
