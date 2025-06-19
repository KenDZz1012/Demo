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
using Service.Lib.BaseRepository;

namespace Account.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context) { }
        /// <summary>
        /// Lấy ra danh sách User
        /// </summary>
        /// <param name="userFilter"></param>
        /// <returns></returns>
        public async Task<List<User>> GetAllAsync(GetUsers userFilter)
        {
            var queryBuilder = Query();
            if (!string.IsNullOrEmpty(userFilter.UserName)) queryBuilder.Filter(u => u.UserName.Contains(userFilter.UserName));
            if (!string.IsNullOrEmpty(userFilter.Email)) queryBuilder.Filter(u => u.Email.Contains(userFilter.Email));
            if (!string.IsNullOrEmpty(userFilter.Status)) queryBuilder.Filter(u => u.Status == userFilter.Status);
            return await queryBuilder.ToListAsync();
        }

        /// <summary>
        /// Lấy ra User theo ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public async Task<User> GetByIdAsync(Guid Id)
        {
            return await base.GetByIdAsync(Id);
        }

        /// <summary>
        /// Thêm User
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(User user)
        {
            await base.AddAsync(user);
            return await base.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Xóa User
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(User user)
        {
            base.Delete(user);
            return await base.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Cập nhật User
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(User user)
        {
            base.Update(user);
            return await base.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Check tồn tại UserName
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<User> CheckExistUserName(string userName)
        {
            var queryBuilder = Query();
            queryBuilder.Filter(x => x.UserName == userName);
            return await queryBuilder.FirstOrDefaultAsync();
        }

        /// <summary>
        /// Check tồn tại Email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<User> CheckExistEmail(string email)
        {
            var queryBuilder = Query();
            queryBuilder.Filter(x => x.Email == email);
            return await queryBuilder.FirstOrDefaultAsync();
        }

        /// <summary>
        /// Lấy ra danh sách User
        /// </summary>
        /// <param name="userFilter"></param>
        /// <returns></returns>
        public async Task<User> GetUserByUserNameOrEmail(string search)
        {
            var queryBuilder = Query();
            queryBuilder.Filter(u => u.UserName == search || u.Email == search);
            return await queryBuilder.FirstOrDefaultAsync();
        }
    }
}
