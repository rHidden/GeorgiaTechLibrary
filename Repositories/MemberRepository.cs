using DbContextNamespace;
using GeorgiaTechLibrary.DTOs;
using GeorgiaTechLibrary.Models;
using GeorgiaTechLibrary.Repositories.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GeorgiaTechLibrary.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        private readonly GTLDbContext _context;

        public MemberRepository(GTLDbContext context)
        {
            _context = context;
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
                    PhoneNumber = member.PhoneNum,
                    // Map other properties of User here
                };

                _context.Users.Add(userDTO);

                var memberDTO = new MemberDTO
                {
                    UserSSN = member.SSN,
                    CardNumber = member.CardNum,
                    ExpiryDate = member.ExpiryDate,
                    Photo = member.Photo,
                    Type = member.Type
                };

                _context.Members.Add(memberDTO);

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
            var member = await _context.Members.FindAsync(SSN);
            if (member != null)
            {
                _context.Members.Remove(member);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Member> GetMember(string SSN)
        {
            var memberDTO = await _context.Members.FindAsync(SSN);
            if (memberDTO != null)
            {
                return MapMemberDTOToMember(memberDTO);
            }
            else
            {
                return null;
            }
        }

        public async Task<List<Member>> ListMembers()
        {
            var memberDTOs = await _context.Members.ToListAsync();
            return memberDTOs.Select(dto => MapMemberDTOToMember(dto)).ToList();
        }

        public async Task UpdateMember(Member member)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                var userDTO = await _context.Users.FindAsync(member.SSN);
                if (userDTO != null)
                {
                    userDTO.FirstName = member.FirstName;
                    userDTO.LastName = member.LastName;
                    userDTO.PhoneNumber = member.PhoneNum;
                    // Address?
                }

                var memberDTO = await _context.Members.FindAsync(member.SSN);
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

        // Method to map MemberDTO to Member
        private Member MapMemberDTOToMember(MemberDTO memberDTO)
        {
            var userDTO = _context.Users.FirstOrDefault(u => u.SSN == memberDTO.UserSSN);

            return new Member
            {
                SSN = userDTO.SSN,
                FirstName = userDTO.FirstName,
                LastName = userDTO.LastName,
                PhoneNum = userDTO.PhoneNumber,
                CardNum = memberDTO.CardNumber,
                ExpiryDate = memberDTO.ExpiryDate,
                Photo = memberDTO.Photo,
                Type = memberDTO.Type
                // Address?
            };
        }
    }
}
