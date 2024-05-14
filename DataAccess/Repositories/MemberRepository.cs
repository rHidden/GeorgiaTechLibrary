using DataAccess.Repositories.RepositoryInterfaces;
using DataAccess.DAO;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        private readonly GTLDbContext _context;

        public MemberRepository(GTLDbContext context)
        {
            _context = context;
        }

        public async Task<Member> GetMember(string SSN)
        {
            var memberDTO = await _context.Member.FindAsync(SSN);
            if (memberDTO != null)
            {
                return memberDTO;
            }
            else
            {
                return null;
            }
        }

        public async Task<List<Member>> ListMembers()
        {
            var memberDTOs = await _context.Member.ToListAsync();
            return memberDTOs.ToList();
        }

        public async Task<Member> CreateMember(Member member)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {

                _context.User.Add(member);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return member;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Failed to create member.", ex);
            }
        }
        public async Task DeleteMember(string SSN)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                var member = await _context.Member.FindAsync(SSN);
                var user = await _context.User.FindAsync(SSN);
                if (member != null && user != null)
                {
                    _context.Member.Remove(member);
                    await _context.SaveChangesAsync();
                    _context.User.Remove(user);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                else
                {
                    throw new Exception("User not found");
                }
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Failed to delete member.", ex);
            }
        }

        public async Task UpdateMember(Member newMember)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                var foundMember = await _context.User.FindAsync(newMember.SSN);
                if (foundMember != null)
                {
                    foundMember.SSN = newMember.SSN;
                    foundMember.FirstName = newMember.FirstName;
                    foundMember.LastName = newMember.LastName;
                    foundMember.PhoneNum = newMember.PhoneNum;
                    foundMember.UserAddress = newMember.UserAddress;
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Failed to update member.", ex);
            }
        }
    }
}
