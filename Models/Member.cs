namespace GeorgiaTechLibrary.Models
{
    public class Member : User
    {
        public string CardNum { get; set; }
        public DateTime ExpiryDate { get; set; }
        public byte[] Photo { get; set; }
        public string Type { get; set; }

        public Member() { }

        public Member(string ssn, string firstName, string lastName, string phoneNum, string cardNum, DateTime expiryDate, byte[] photo, string type, Address userAddress)
            : base(ssn, firstName, lastName, phoneNum, userAddress)
        {
            CardNum = cardNum;
            ExpiryDate = expiryDate;
            Photo = photo;
            Type = type;
        }
    }
}
