using DbContextNamespace;
using GeorgiaTechLibrary.DTOs;
using GeorgiaTechLibrary.Models;
using GeorgiaTechLibrary.Repositories.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeorgiaTechLibrary.Repositories
{
    public class StaffRepository : IStaffRepository
    {
        private readonly GTLDbContext _context;

        public StaffRepository(GTLDbContext context)
        {
            _context = context;
        }

        public async Task<Staff> GetStaff(int SSN)
        {
            throw new NotImplementedException();
        }

        public async Task<Staff> CreateStaff(Staff staff)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateStaff(Staff staff)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteStaff(int SSN)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Staff>> ListStaff()
        {
            throw new NotImplementedException();
        }

        // Method to map StaffDTO to Staff
        private Staff MapStaffDTOToStaff(StaffDTO staffDTO)
        {
            throw new NotImplementedException();
        }
    }
}
