using DbContextNamespace;
using GeorgiaTechLibrary.DTOs;
using GeorgiaTechLibrary.Models;
using GeorgiaTechLibrary.Repositories.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.IO;
using System.Reflection.Emit;
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
                    Street = member.UserAddress.Street,
                    StreetNumber = member.UserAddress.StreetNum,
                    Zipcode = member.UserAddress.ZipCode,
                    City = member.UserAddress.City,
                };

                _context.User.Add(userDTO);

                await _context.SaveChangesAsync();

                var memberDTO = new MemberDTO
                {
                    UserSSN = userDTO.SSN,
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


        public async Task<Member> GetMember(string SSN)
        {
            var memberDTO = await _context.Member.FindAsync(SSN);
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
            var memberDTOs = await _context.Member.ToListAsync();
            return memberDTOs.Select(dto => MapMemberDTOToMember(dto)).ToList();
        }

        public async Task UpdateMember(Member member)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                var userDTO = await _context.User.FindAsync(member.SSN);
                if (userDTO != null)
                {
                    userDTO.FirstName = member.FirstName;
                    userDTO.LastName = member.LastName;
                    userDTO.PhoneNumber = member.PhoneNum;
                    userDTO.Street = member.UserAddress.Street;
                    userDTO.StreetNumber = member.UserAddress.StreetNum;
                    userDTO.Zipcode = member.UserAddress.ZipCode;
                    userDTO.City = member.UserAddress.City;
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

        private Member MapMemberDTOToMember(MemberDTO memberDTO)
        {
            var userDTO = _context.User.FirstOrDefault(u => u.SSN == memberDTO.UserSSN);
            if (userDTO == null)
            {
                throw new Exception("User not found");
            }

            return new Member
            {
                SSN = userDTO.SSN,
                FirstName = userDTO.FirstName,
                LastName = userDTO.LastName,
                PhoneNum = userDTO.PhoneNumber,
                CardNum = memberDTO.CardNumber,
                ExpiryDate = memberDTO.ExpiryDate,
                Photo = memberDTO.Photo,
                Type = memberDTO.Type,
                UserAddress = new Address
                {
                    Street = userDTO.Street,
                    StreetNum = userDTO.StreetNumber,
                    ZipCode = userDTO.Zipcode,
                    City = userDTO.City,
                }
            };
        }
    }
}
