namespace GeorgiaTechLibrary.Models
{
    public class Address
    {
        public string Street { get; set; }
        public string StreetNum { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }

        public Address() { }

        public Address(string street, string streetNum, string city, string zipCode)
        {
            Street = street;
            StreetNum = streetNum;
            City = city;
            ZipCode = zipCode;
        }
    }
}
