using DataAccess.Models;

namespace DataAccess.Repositories.RepositoryInterfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetMostActiveUsers();
    }
}
