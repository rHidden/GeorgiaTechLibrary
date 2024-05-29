using DataAccess.Models;

namespace DataAccess.Repositories.RepositoryInterfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetMostActiveUsers();
        Task<List<(User, int SumOfDaysOfBeingLate)>> GetDaysLate();
        Task<List<(User, int AvgLoanDuration)>> GetAverageLoanDuration();
    }
}
