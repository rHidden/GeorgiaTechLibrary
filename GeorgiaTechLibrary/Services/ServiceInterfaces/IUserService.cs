using DataAccess.Models;

namespace GeorgiaTechLibrary.Services.ServiceInterfaces
{
    public interface IUserService
    {
        Task<List<User>> GetMostActiveUsers();
    }
}
