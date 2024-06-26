﻿namespace GeorgiaTechLibrary.DTOs
{
    public class UserDTO
    {
        public string SSN { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public AddressDTO UserAddress { get; set; }
    }
}
