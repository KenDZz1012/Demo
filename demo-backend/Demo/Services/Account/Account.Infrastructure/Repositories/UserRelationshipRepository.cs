using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Account.Application.Contracts.Persistence;
using Account.Domain.Entities;
using Account.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Account.Infrastructure.Repositories
{
    public class UserRelationshipRepository : IUserRelationshipRepository
    {
        private readonly AppDbContext _context;
        public UserRelationshipRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(UserRelationship userRelationship)
        {
           _context.UserRelationships.Add(userRelationship);
        }

        public async Task DeleteAsync(Guid ID)
        {
            var userRelationship = await _context.UserRelationships.FindAsync(ID);
            if (userRelationship != null)
            {
                _context.UserRelationships.Remove(userRelationship);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<UserRelationship>> GetAllAsync()
        {
            return await _context.UserRelationships.ToListAsync();
        }

        public async Task<UserRelationship> GetByIdAsync(Guid ID)
        {
            return await _context.UserRelationships.FindAsync(ID);
        }

        public async Task UpdateAsync(UserRelationship userRelationship)
        {
            _context.UserRelationships.Update(userRelationship);
            await _context.SaveChangesAsync();
        }
    }
}
