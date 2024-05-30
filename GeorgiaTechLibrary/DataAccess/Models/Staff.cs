namespace DataAccess.Models
{
    public class Staff : User
    {
        public string? LibrarianNumber { get; set; }
        public string? Role { get; set; }

        public Staff() { }

        public Staff(string ssn, string firstName, string lastName, string phoneNum, 
            string librarianNum, string role, Address userAddress)
            : base(ssn, firstName, lastName, phoneNum, userAddress)
        {
            LibrarianNumber = librarianNum;
            Role = role;
        }
    }
}
