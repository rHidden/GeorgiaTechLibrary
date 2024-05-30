using DataAccess.Models;

namespace GeorgiaTechLibrary.DTOs
{
    public class LoanItemStatisticsDTO
    {
        public double Books { get; set; }
        public double Videos { get; set; }
        public double Audios { get; set; }
        public double Texts { get; set; }
        public double Images { get; set; }
    }

}
