using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Service.Lib.QueryBuilder
{
    public class QueryBuilder<T> where T : class
    {
        private IQueryable<T> _query;

        public QueryBuilder(IQueryable<T> query)
        {
            _query = query;
        }

        /// <summary>
        /// Bộ lọc
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public QueryBuilder<T> Filter(Expression<Func<T, bool>> predicate)
        {
            _query = _query.Where(predicate);
            return this;
        }

        /// <summary>
        /// Sắp xếp
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="keySelector"></param>
        /// <param name="ascending"></param>
        /// <returns></returns>
        public QueryBuilder<T> Sort<TKey>(Expression<Func<T, TKey>> keySelector, bool ascending = true)
        {
            _query = ascending ? _query.OrderBy(keySelector) : _query.OrderByDescending(keySelector);
            return this;
        }

        /// <summary>
        /// Sort theo tên field
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="ascending"></param>
        /// <returns></returns>
        public QueryBuilder<T> Sort(string propertyName, bool ascending = true)
        {
            var param = Expression.Parameter(typeof(T), "x");
            var body = Expression.PropertyOrField(param, propertyName);
            var lambda = Expression.Lambda(body, param);

            var methodName = ascending ? "OrderBy" : "OrderByDescending";
            var resultExp = Expression.Call(
                typeof(Queryable),
                methodName,
                new Type[] { typeof(T), body.Type },
                _query.Expression,
                Expression.Quote(lambda)
            );

            _query = _query.Provider.CreateQuery<T>(resultExp);
            return this;
        }


        /// <summary>
        /// Select các trường
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="selector"></param>
        /// <returns></returns>
        public IQueryable<TResult> Select<TResult>(Expression<Func<T, TResult>> selector)
        {
            return _query.Select(selector);
        }

        /// <summary>
        /// Sử dụng INNER JOIN
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="navigationPropertyPath"></param>
        /// <returns></returns>
        public QueryBuilder<T> Include<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath)
        {
            _query = _query.Include(navigationPropertyPath);
            return this;
        }

        /// <summary>
        /// Lấy ra danh sách
        /// </summary>
        /// <returns></returns>
        public async Task<List<T>> ToListAsync()
        {
            return await _query.ToListAsync();
        }

        /// <summary>
        /// Lấy bản ghi đầu tiên
        /// </summary>
        /// <returns></returns>
        public async Task<T?> FirstOrDefaultAsync()
        {
            return await _query.FirstOrDefaultAsync();
        }


        /// <summary>
        /// Phân trang
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public QueryBuilder<T> Paginate(int pageNumber, int pageSize)
        {
            _query = _query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            return this;
        }

        /// <summary>
        /// Đếm số lượng
        /// </summary>
        /// <returns></returns>
        public async Task<int> CountAsync()
        {
            return await _query.CountAsync();
        }

        public IQueryable<T> Build()
        {
            return _query;
        }
    }
}
