using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Account.Application.Contracts.Persistence;
using Account.Application.Features.UserRelationship.Queries.GetListUserRelationshipQuery;
using Account.Application.Models.Filter.User;
using Account.Domain.Entities;
using Account.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Service.Lib.QueryBuilder;

namespace Account.Infrastructure.Repositories
{
    public class UserRelationshipRepository : IUserRelationshipRepository
    {
        private readonly AppDbContext _context;
        public UserRelationshipRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> AddAsync(UserRelationship userRelationship)
        {
           _context.UserRelationships.Add(userRelationship);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(UserRelationship userRelationship)
        {
            _context.UserRelationships.Remove(userRelationship);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<UserRelationship>> GetAllAsync(GetListUserRelationship filter)
        {
            var queryBuilder = new QueryBuilder<UserRelationship>(_context.UserRelationships);
            queryBuilder.Include(ur => ur.Requester)
                .Include(ur => ur.Addressee);
            if (filter.RequesterId != null)
                queryBuilder.Filter(u => u.RequesterId == filter.RequesterId);
            if(filter.AddresseeId != null)
                queryBuilder.Filter(u => u.AddresseeId == filter.AddresseeId);
            if (!string.IsNullOrEmpty(filter.Status))
                queryBuilder.Filter(u => u.Status == filter.Status);
            queryBuilder.Sort(u => u.CreatedAt);
            
            return await queryBuilder.ToListAsync();
        }

        public async Task<UserRelationship> GetByIdAsync(Guid ID)
        {
            return await _context.UserRelationships.FindAsync(ID);
        }

        public async Task<bool> UpdateAsync(UserRelationship userRelationship)
        {
            _context.UserRelationships.Update(userRelationship);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<UserRelationship> CheckExistRelationship(Guid requesterId, Guid addresseeId)
        {
            return await _context.UserRelationships
                .Where(ur => ur.RequesterId == requesterId && ur.AddresseeId == addresseeId)
                .FirstOrDefaultAsync();
        }
    }
}
