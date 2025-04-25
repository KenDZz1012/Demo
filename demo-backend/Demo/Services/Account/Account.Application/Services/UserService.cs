using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Account.Application.Interfaces;
using Account.Domain.Interfaces;
using Account.Domain.Entities;
using Account.Domain.Filters;
using Service.Lib.Password;
using Account.Application.DTOs.User;
using Account.Domain.Model.User;

namespace Account.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<List<UserDTO>> GetAllAsync(UserFilter userFilter)
        {
            return _mapper.Map<List<UserDTO>>(await _userRepository.GetAllAsync(userFilter));
        }

        public async Task<UserDTO> GetByIdAsync(Guid id)
        {
            return _mapper.Map<UserDTO>(await _userRepository.GetByIdAsync(id));
        }

        public async Task AddAsync(UserCreateDTO user)
        {
            user.PasswordHash = await PasswordMD5.CreateMD5(user.PasswordHash);
            await _userRepository.AddAsync(_mapper.Map<IUserCreateInfo>(user));
        }

        public async Task UpdateAsync(User user)
        {
            await _userRepository.UpdateAsync(user);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _userRepository.DeleteAsync(id);
        }
    }
}
