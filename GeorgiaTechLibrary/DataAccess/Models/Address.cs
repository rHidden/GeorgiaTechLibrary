namespace DataAccess.Models
{
    public class Address
    {
        public string? Street { get; set; }
        public string? StreetNumber { get; set; }
        public string? City { get; set; }
        public string? ZipCode { get; set; }

        public Address() { }

        public Address(string street, string streetNumber, string city, string zipCode)
        {
            Street = street;
            StreetNumber = streetNumber;
            City = city;
            ZipCode = zipCode;
        }
    }
}
