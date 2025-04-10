using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using User.Application.DTOs;
using User.Application.Interfaces;
using User.Domain.Interfaces;

namespace User.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;

        public AccountService(IAccountRepository accountRepository, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
        }

        public async Task<List<AccountDTO>> GetAllAsync()
        {
            var accounts = await _accountRepository.GetAllAsync();
            return _mapper.Map<List<AccountDTO>>(accounts);
        }

        public async Task<AccountDTO> GetByIdAsync(string id)
        {
            var account = await _accountRepository.GetByIdAsync(id);
            return _mapper.Map<AccountDTO>(account);
        }

    }
}
