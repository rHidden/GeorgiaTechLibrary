namespace GeorgiaTechLibrary.Models
{
    public class Library
    {
        public string Name { get; set; }
        public List<User> Users { get; set; }
        public Address LibraryAddress { get; set; }
        public List<BookInstance> BookInstances { get; set; }

        public Library() { }

        public Library(string name, List<User> users, Address libraryAddress, List<BookInstance> bookInstances)
        {
            Name = name;
            Users = users;
            LibraryAddress = libraryAddress;
            BookInstances = bookInstances;
        }
    }
}
