namespace DataAccess.Models
{
    public class Library
    {
        public string Name { get; set; }
        public Address? LibraryAddress { get; set; }
        public List<User>? Users { get; set; }
        public List<BookInstance>? BookInstances { get; set; }
        public List<DigitalItem>? DigitalItems { get; set; }

        public Library() { }

        public Library(string name, Address libraryAddress, List<User> users, List<BookInstance> bookInstances, List<DigitalItem> digitalItems)
        {
            Name = name;
            LibraryAddress = libraryAddress;
            Users = users;
            BookInstances = bookInstances;
            DigitalItems = digitalItems;
        }
    }
}
