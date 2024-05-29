using DataAccess.Models;

namespace GeorgiaTechLibrary.DTOs
{
    public class AverageLoanDurationDTO
    {
        public User? User { get; set; }
        public int AverageLoanDuration { get; set; }
    }

}
