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
            var staffDTO = await _context.Staff.FindAsync(SSN);
            if (staffDTO != null)
            {
                return MapStaffDTOToStaff(staffDTO);
            }
            else
            {
                return null;
            }
        }

        public async Task<Staff> CreateStaff(Staff staff)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                var userDTO = new UserDTO
                {
                    SSN = staff.SSN,
                    FirstName = staff.FirstName,
                    LastName = staff.LastName,
                    PhoneNumber = staff.PhoneNum,
                    // maybe add address here?
                };

                _context.Users.Add(userDTO);

                var staffDTO = new StaffDTO
                {
                    UserSSN = staff.SSN,
                    Role = staff.Role,
                    LibrarianNumber = staff.LibrarianNum
                };

                _context.Staff.Add(staffDTO);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return staff;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Failed to create staff.", ex);
            }
        }

        public async Task UpdateStaff(Staff staff)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                var userDTO = await _context.Users.FindAsync(staff.SSN);
                if (userDTO != null)
                {
                    userDTO.FirstName = staff.FirstName;
                    userDTO.LastName = staff.LastName;
                    userDTO.PhoneNumber = staff.PhoneNum;
                    // maybe add address here?
                }

                var staffDTO = await _context.Staff.FindAsync(staff.SSN);
                if (staffDTO != null)
                {
                    staffDTO.Role = staff.Role;
                    staffDTO.LibrarianNumber = staff.LibrarianNum;
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Failed to update staff.", ex);
            }
        }

        public async Task DeleteStaff(int SSN)
        {
            var staff = await _context.Staff.FindAsync(SSN);
            if (staff != null)
            {
                _context.Staff.Remove(staff);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Staff>> ListStaff()
        {
            var staffDTOs = await _context.Staff.ToListAsync();
            return staffDTOs.Select(dto => MapStaffDTOToStaff(dto)).ToList();
        }

        // Method to map StaffDTO to Staff
        private Staff MapStaffDTOToStaff(StaffDTO staffDTO)
        {
            var userDTO = _context.Users.FirstOrDefault(u => u.SSN == staffDTO.UserSSN);
            if (userDTO == null) {
                return null;
            }

            return new Staff
            {
                SSN = userDTO.SSN,
                FirstName = userDTO.FirstName,
                LastName = userDTO.LastName,
                PhoneNum = userDTO.PhoneNumber,
                LibrarianNum = staffDTO.LibrarianNumber,
                Role = staffDTO.Role,
                // Address here?
            };
        }
    }
}
