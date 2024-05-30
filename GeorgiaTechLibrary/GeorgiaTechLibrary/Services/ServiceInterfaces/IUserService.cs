using DataAccess.Models;
using GeorgiaTechLibrary.DTOs;

namespace GeorgiaTechLibrary.Services.ServiceInterfaces
{
    public interface IUserService
    {
        Task<List<User>> GetMostActiveUsers();
        Task<List<LateUserDto>> GetDaysLate();
        Task<List<AverageLoanDurationDTO>> GetAverageLoanDuration();
    }
}
