using Microsoft.EntityFrameworkCore;
using Service.Lib.QueryBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Service.Lib.BaseRepository
{
    public class BaseRepository<T> where T : class
    {
        protected readonly DbContext _context;
        protected readonly DbSet<T> _dbSet;

        public BaseRepository(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        /// <summary>
        /// Lấy ra danh sách
        /// </summary>
        /// <returns></returns>
        public virtual async Task<List<T>> GetAllAsync()
        {
            var queryBuilder = new QueryBuilder<T>(_dbSet.AsQueryable());
            return await queryBuilder.ToListAsync();
        }

        /// <summary>
        /// Lấy theo ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<T?> GetByIdAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        /// <summary>
        /// Thêm mới
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        /// <summary>
        /// Cập nhật
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        /// <summary>
        /// Xóa
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        /// <summary>
        /// Lưu thay đổi
        /// </summary>
        /// <returns></returns>
        public virtual async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Filter
        /// </summary>
        /// <returns></returns>
        public virtual QueryBuilder<T> Query()
        {
            return new QueryBuilder<T>(_dbSet.AsQueryable());
        }

        /// <summary>
        /// Phân trang
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public virtual async Task<List<T>> GetPagedAsync(int page, int pageSize)
        {
            var queryBuilder = new QueryBuilder<T>(_dbSet.AsQueryable())
                .Paginate(page, pageSize);

            return await queryBuilder.ToListAsync();
        }

        /// <summary>
        /// Đếm số lượng bản ghi
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null)
        {
            return predicate == null
                ? await _dbSet.CountAsync()
                : await _dbSet.CountAsync(predicate);
        }
    }
}
