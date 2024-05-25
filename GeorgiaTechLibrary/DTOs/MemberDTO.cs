namespace GeorgiaTechLibrary.DTOs
{
    public class MemberDTO : UserDTO
    {
        public string CardNumber { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Photo { get; set; }
        public string Type { get; set; }
    }
}
