using DataAccess.Models;
using DataAccess.Repositories;
using DataAccess.Repositories.RepositoryInterfaces;
using GeorgiaTechLibrary.Services.ServiceInterfaces;

namespace GeorgiaTechLibrary.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<User>> GetMostActiveUsers()
        {
            return await _userRepository.GetMostActiveUsers();
        }
    }
}
