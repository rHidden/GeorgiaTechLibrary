﻿using DataAccess.Models;
using DataAccess.Repositories;
using DataAccess.Repositories.RepositoryInterfaces;
using GeorgiaTechLibrary.Services.ServiceInterfaces;

namespace GeorgiaTechLibrary.Services
{
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository _memberRepository;

        public MemberService(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }
        public async Task<Member> GetMember(string SSN)
        {
            return await _memberRepository.GetMember(SSN);
        }

        public async Task<List<Member>> ListMembers()
        {
            return await _memberRepository.ListMembers();
        }

        public async Task<Member> CreateMember(Member member)
        {
            return await _memberRepository.CreateMember(member);
        }
        public async Task<Member> UpdateMember(Member member)
        {
            return await _memberRepository.UpdateMember(member);
        }

        public async Task<bool> DeleteMember(string SSN)
        {
            return await _memberRepository.DeleteMember(SSN);
        }
    }
}
