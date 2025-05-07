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

        // Lọc theo một điều kiện bất kỳ
        public QueryBuilder<T> Filter(Expression<Func<T, bool>> predicate)
        {
            _query = _query.Where(predicate);
            return this;
        }

        // Phương thức sắp xếp (sort)
        public QueryBuilder<T> Sort<TKey>(Expression<Func<T, TKey>> keySelector, bool ascending = true)
        {
            _query = ascending ? _query.OrderBy(keySelector) : _query.OrderByDescending(keySelector);
            return this;
        }

        // Phương thức chọn các thuộc tính cụ thể (Select)
        public IQueryable<TResult> Select<TResult>(Expression<Func<T, TResult>> selector)
        {
            return _query.Select(selector);
        }

        //Phương thức Include (JOIN)
        public QueryBuilder<T> Include<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath)
        {
            _query = _query.Include(navigationPropertyPath);
            return this;
        }

        // Thực thi truy vấn và trả về danh sách kết quả (ToListAsync hoặc ToList)
        public async Task<List<T>> ToListAsync()
        {
            return await _query.ToListAsync();
        }

        // Trả về kết quả truy vấn
        public IQueryable<T> Build()
        {
            return _query;
        }
    }
}
