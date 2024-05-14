using AutoMapper;
using DbContextNamespace;
using GeorgiaTechLibrary.DTOs;
using GeorgiaTechLibrary.Models;
using GeorgiaTechLibrary.Repositories.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace GeorgiaTechLibrary.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        private readonly GTLDbContext _context;
        private readonly IMapper _mapper;

        public MemberRepository(GTLDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Member> GetMember(string SSN)
        {
            var memberDTO = await _context.Member.FindAsync(SSN);
            if (memberDTO != null)
            {
                return _mapper.Map<Member>(memberDTO);
            }
            else
            {
                return null;
            }
        }

        public async Task<List<Member>> ListMembers()
        {
            var memberDTOs = await _context.Member.ToListAsync();
            return memberDTOs.Select(dto => _mapper.Map<Member>(dto)).ToList();
        }

        public async Task<Member> CreateMember(Member member)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                var userDTO = new UserDTO
                {
                    SSN = member.SSN,
                    FirstName = member.FirstName,
                    LastName = member.LastName,
                    PhoneNum = member.PhoneNum,
                    UserAddress = new AddressDTO
                    {
                        Street = member.UserAddress.Street,
                        StreetNum = member.UserAddress.StreetNum,
                        ZipCode = member.UserAddress.ZipCode,
                        City = member.UserAddress.City
                    }
                };

                _context.User.Add(userDTO);

                await _context.SaveChangesAsync();

                var memberDTO = new MemberDTO
                {
                    CardNumber = member.CardNum,
                    ExpiryDate = member.ExpiryDate,
                    Photo = member.Photo,
                    Type = member.Type
                };

                _context.Member.Add(memberDTO);

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

        public async Task UpdateMember(Member member)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                var userDTO = await _context.User.FindAsync(member.SSN);
                if (userDTO != null)
                {
                    userDTO.SSN = member.SSN;
                    userDTO.FirstName = member.FirstName;
                    userDTO.LastName = member.LastName;
                    userDTO.PhoneNum = member.PhoneNum;
                    userDTO.UserAddress = new AddressDTO
                    {
                        Street = member.UserAddress.Street,
                        StreetNum = member.UserAddress.StreetNum,
                        ZipCode = member.UserAddress.ZipCode,
                        City = member.UserAddress.City
                    };
                }

                var memberDTO = await _context.Member.FindAsync(member.SSN);
                if (memberDTO != null)
                {
                    memberDTO.CardNumber = member.CardNum;
                    memberDTO.ExpiryDate = member.ExpiryDate;
                    memberDTO.Photo = member.Photo;
                    memberDTO.Type = member.Type;
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
