﻿using GeorgiaTechLibrary.Models;

namespace GeorgiaTechLibrary.Repositories.RepositoryInterfaces
{
    public interface IMemberRepository
    {
        Task<Member> GetMember(string SSN);
        Task<List<Member>> ListMembers();
        Task<Member> CreateMember(Member member);
        Task UpdateMember(Member member);
        Task DeleteMember(string SSN);
    }
}
