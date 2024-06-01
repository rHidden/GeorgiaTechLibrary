namespace DataAccess.Models
{
    public class Member : User
    {
        public string? CardNumber { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string? Photo { get; set; }
        public string? MemberType { get; set; }

        public Member() { }

        public Member(string ssn, string firstName, string lastName, string phoneNum,
            string cardNum, DateTime expiryDate, string photo, string memberType, Address userAddress)
            : base(ssn, firstName, lastName, phoneNum, userAddress)
        {
            CardNumber = cardNum;
            ExpiryDate = expiryDate;
            Photo = photo;
            MemberType = memberType;
        }
    }
}
