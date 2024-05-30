using DataAccess.Models;

namespace GeorgiaTechLibrary.DTOs
{
    public class LateUserDto
    {
        public User? User { get; set; }
        public int SumOfDaysOfBeingLate { get; set; }
    }

}
