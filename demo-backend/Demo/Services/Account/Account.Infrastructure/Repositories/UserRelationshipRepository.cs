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
using Service.Lib.BaseRepository;
using Service.Lib.QueryBuilder;

namespace Account.Infrastructure.Repositories
{
    public class UserRelationshipRepository : BaseRepository<UserRelationship>, IUserRelationshipRepository
    {
        private readonly AppDbContext _context;
        public UserRelationshipRepository(AppDbContext context) : base(context) { }

        /// <summary>
        /// Thêm mối quan hệ giữa 2 người dùng
        /// </summary>
        /// <param name="userRelationship"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(UserRelationship userRelationship)
        {
            await base.AddAsync(userRelationship);
            return await base.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Xóa mối quan hệ giữa 2 người dùng
        /// </summary>
        /// <param name="userRelationship"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(UserRelationship userRelationship)
        {
            base.Delete(userRelationship);
            return await base.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Lấy ra danh sách mối quan hệ 
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<List<UserRelationship>> GetAllAsync(GetListUserRelationship filter)
        {
            var queryBuilder = Query().Include(ur => ur.Requester).Include(ur => ur.Addressee);
            if (filter.RequesterId != null) queryBuilder.Filter(u => u.RequesterId == filter.RequesterId);
            if (filter.AddresseeId != null) queryBuilder.Filter(u => u.AddresseeId == filter.AddresseeId);
            if (!string.IsNullOrEmpty(filter.Status)) queryBuilder.Filter(u => u.Status == filter.Status);
            queryBuilder.Sort(u => u.CreatedAt);
            return await queryBuilder.ToListAsync();
        }

        /// <summary>
        /// Lấy ra mối quan hệ theo ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public async Task<UserRelationship> GetByIdAsync(Guid Id)
        {
            var queryBuilder = Query().Include(ur => ur.Requester).Include(ur => ur.Addressee).Filter(ur => ur.Id == Id);
            return await queryBuilder.FirstOrDefaultAsync();
        }

        /// <summary>
        /// Cập nhật mối quan hệ giữa 2 người dùng
        /// </summary>
        /// <param name="userRelationship"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(UserRelationship userRelationship)
        {
            base.Update(userRelationship);
            return await base.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Kiểm tra xem mối quan hệ giữa 2 người dùng đã tồn tại hay chưa
        /// </summary>
        /// <param name="requesterId"></param>
        /// <param name="addresseeId"></param>
        /// <returns></returns>
        public async Task<UserRelationship> CheckExistRelationship(Guid requesterId, Guid addresseeId)
        {
            var queryBuilder = Query().Filter(ur => (ur.RequesterId == requesterId && ur.AddresseeId == addresseeId) || (ur.RequesterId == addresseeId && ur.AddresseeId == requesterId)).FirstOrDefaultAsync();
            return await queryBuilder;
        }
    }
}
