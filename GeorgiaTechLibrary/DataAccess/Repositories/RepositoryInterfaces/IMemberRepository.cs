﻿using DataAccess.Models;

namespace DataAccess.Repositories.RepositoryInterfaces
{
    public interface IMemberRepository
    {
        Task<Member> GetMember(string SSN);
        Task<List<Member>> ListMembers();
        Task<Member> CreateMember(Member member);
        Task<Member> UpdateMember(Member member);
        Task<bool> DeleteMember(string SSN);
    }
}
