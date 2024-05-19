namespace DataAccess.Models
{
    public class User
    {
        public string SSN { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public Address? UserAddress { get; set; }

        public User() { }

        public User(string ssn, string firstName, string lastName, string phoneNumber, Address userAddress)
        {
            SSN = ssn;
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            UserAddress = userAddress;
        }
    }
}
