using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Account.Domain.Entities;
using Account.Infrastructure.Data;
using AutoMapper.QueryableExtensions;
using AutoMapper;
using Service.Lib.QueryBuilder;
using Account.Application.Contracts.Persistence;
using Account.Application.Features.User.Queries.GetUsersQuery;

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

        public async Task<List<User>> GetAllAsync(GetUsers userFilter)
        {
            var queryBuilder = new QueryBuilder<User>(_context.Users);
            if (!string.IsNullOrEmpty(userFilter.UserName))
                queryBuilder.Filter(u => u.UserName.Contains(userFilter.UserName));
            if (!string.IsNullOrEmpty(userFilter.Email))
                queryBuilder.Filter(u => u.Email.Contains(userFilter.Email));
            if(!string.IsNullOrEmpty(userFilter.Status))
                queryBuilder.Filter(u => u.Status == userFilter.Status);
            queryBuilder.Sort(u => u.CreatedAt);
            return await queryBuilder.ToListAsync();
        }
        public async Task<User> GetByIdAsync(Guid ID)
        {
            return await _context.Users.FindAsync(ID);
        }
        public async Task<bool> AddAsync(User user)
        {          
            _context.Users.Add(user);
            return await _context.SaveChangesAsync() > 0;                   
        }
        public async Task<bool> DeleteAsync(User user)
        {
            _context.Users.Remove(user);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> UpdateAsync(User user)
        {
            _context.Users.Update(user);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<User> CheckExistUserName(string userName)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.UserName == userName);
        }

        public async Task<User> CheckExistEmail (string email)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }
    }
}
