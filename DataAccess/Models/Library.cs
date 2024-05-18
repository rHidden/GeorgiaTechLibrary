namespace DataAccess.Models
{
    public class Library
    {
        public string? Name { get; set; }
        public List<User>? Users { get; set; }
        public Address? LibraryAddress { get; set; }
        public List<BookInstance>? BookInstances { get; set; }
        public List<DigitalItem>? DigitalItems { get; set; }

        public Library() { }

        public Library(string name, List<User> users, Address libraryAddress, List<BookInstance> bookInstances, List<DigitalItem> digitalItems)
        {
            Name = name;
            Users = users;
            LibraryAddress = libraryAddress;
            BookInstances = bookInstances;
            DigitalItems = digitalItems;
        }
    }
}
