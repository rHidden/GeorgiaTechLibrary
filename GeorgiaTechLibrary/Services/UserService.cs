using AutoMapper;
using DataAccess.Models;
using DataAccess.Repositories;
using DataAccess.Repositories.RepositoryInterfaces;
using GeorgiaTechLibrary.DTOs;
using GeorgiaTechLibrary.Services.ServiceInterfaces;

namespace GeorgiaTechLibrary.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;

        }

        public async Task<List<User>> GetMostActiveUsers()
        {
            return await _userRepository.GetMostActiveUsers();
        }

        public async Task<List<LateUserDto>> GetDaysLate()
        {
            return (await _userRepository.GetDaysLate()).Select(result => _mapper.Map<LateUserDto>(result)).ToList();
        }

        public async Task<List<AverageLoanDurationDTO>> GetAverageLoanDuration()
        {
            return (await _userRepository.GetAverageLoanDuration()).Select(
                result => _mapper.Map<AverageLoanDurationDTO>(result)).ToList();
        }
    }
}
