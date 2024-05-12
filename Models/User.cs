﻿namespace GeorgiaTechLibrary.Models
{
    public class User
    {
        public string SSN { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNum { get; set; }
        public Address UserAddress { get; set; }

        public User() { }

        public User(string ssn, string firstName, string lastName, string phoneNum, Address userAddress)
        {
            SSN = ssn;
            FirstName = firstName;
            LastName = lastName;
            PhoneNum = phoneNum;
            UserAddress = userAddress;
        }
    }
}
