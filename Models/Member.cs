namespace GeorgiaTechLibrary.Models
{
    public class Member
    {
        public int SSN { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Street { get; set; }
        public string StreetNumber { get; set; }
        public string City { get; set; }
        public string Zipcode { get; set; }
        //Member specific
        public string CardNumber { get; set; }
        public string ExpiryDate { get; set; }
        public string Photo { get; set; }
        public string Type { get; set; }

    }
}
