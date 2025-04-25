using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Account.Domain.Entities;
using Account.Domain.Interfaces;
using Account.Infrastructure.Data;
using Account.Application.DTOs;
using AutoMapper.QueryableExtensions;
using AutoMapper;
using Account.Domain.Filters;
using Service.Lib.QueryBuilder;
using Account.Domain.Model.User;

namespace Account.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public UserRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<User>> GetAllAsync(UserFilter userFilter)
        {
            var queryBuilder = new QueryBuilder<User>(_context.Users);
            if (!string.IsNullOrEmpty(userFilter.UserName))
                queryBuilder.Filter(u => u.UserName.Contains(userFilter.UserName));
            if (!string.IsNullOrEmpty(userFilter.Email))
                queryBuilder.Filter(u => u.Email.Contains(userFilter.Email));
            if(!string.IsNullOrEmpty(userFilter.Status))
                queryBuilder.Filter(u => u.Status.Contains(userFilter.Status));
            queryBuilder.Sort(u => u.CreatedAt);
            return await queryBuilder.ToListAsync();
        }
        public async Task<User> GetByIdAsync(Guid ID)
        {
            return await _context.Users.FindAsync(ID);
        }
        public async Task AddAsync(IUserCreateInfo userCreateInfo)
        {
            var user = new User
            {
                UserName = userCreateInfo.UserName,
                Email = userCreateInfo.Email,
                Status = userCreateInfo.Status
            };
            _context.Users.Add(user);
        }
        public async Task DeleteAsync(Guid ID)
        {
            var user = await _context.Users.FindAsync(ID);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
        public async Task UpdateAsync(User user)
        {
            var existUser = await _context.Users.FindAsync(user.ID);
            if (existUser == null)
            {
                throw new Exception("User not found");
            }
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
