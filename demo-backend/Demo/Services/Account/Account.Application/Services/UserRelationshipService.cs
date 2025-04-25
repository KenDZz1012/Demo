using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Account.Application.DTOs.UserRelationship;
using Account.Application.Interfaces;
using Account.Domain.Entities;
using Account.Domain.Interfaces;
using AutoMapper;

namespace Account.Application.Services
{
    public class UserRelationshipService : IUserRelationshipService
    {
        private readonly IUserRelationshipRepository _userRelationshipRepository;
        private readonly IMapper _mapper;

        public UserRelationshipService(IUserRelationshipRepository userRelationshipRepository, IMapper mapper)
        {
            _userRelationshipRepository = userRelationshipRepository;
            _mapper = mapper;
        }
        public async Task<List<UserRelationshipDTO>> GetAllAsync()
        {
            return _mapper.Map<List<UserRelationshipDTO>>(await _userRelationshipRepository.GetAllAsync());
        }
        public async Task<UserRelationshipDTO> GetByIdAsync(Guid id)
        {
            return _mapper.Map<UserRelationshipDTO>(await _userRelationshipRepository.GetByIdAsync(id));
        }
        public async Task AddAsync(UserRelationship user)
        {
            await _userRelationshipRepository.AddAsync(user);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _userRelationshipRepository.DeleteAsync(id);
        }
        public async Task UpdateAsync(UserRelationship user)
        {
            await _userRelationshipRepository.UpdateAsync(user);
        }
    }
}
