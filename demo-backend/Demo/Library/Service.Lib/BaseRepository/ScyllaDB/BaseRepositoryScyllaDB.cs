using Cassandra;
using Cassandra.Mapping;
using Cassandra.Mapping.Attributes;
using System.Reflection;

namespace Service.Lib.BaseRepository.ScyllaDB
{
    public class BaseRepositoryScyllaDB<T> where T : class
    {
        private readonly IScyllaContext _context;
        private readonly string _tableName;
        private readonly string _keyspaceName;

        public BaseRepositoryScyllaDB(IScyllaContext context)
        {
            _context = context;
            var tableAttr = typeof(T).GetCustomAttribute<TableAttribute>();
            if (tableAttr == null)
                throw new InvalidOperationException($"Entity {typeof(T).Name} thiếu [Table] attribute");

            _tableName = tableAttr.Name;
            _keyspaceName = string.IsNullOrEmpty(tableAttr.Keyspace)
                            ? _context.Keyspace
                            : tableAttr.Keyspace;
        }

        public async Task InsertAsync(T entity) => await _context.Mapper.InsertAsync(entity);
        public async Task UpdateAsync(T entity) => await _context.Mapper.UpdateAsync(entity);
        public async Task DeleteAsync(T entity) => await _context.Mapper.DeleteAsync(entity);

        public async Task<IEnumerable<T>> GetAllAsync() =>
            await _context.Mapper.FetchAsync<T>($"SELECT * FROM {_keyspaceName}.{_tableName}");

        public async Task<IEnumerable<T>> GetWhereAsync(string whereClause, params object[] values) =>
            await _context.Mapper.FetchAsync<T>(
                $"SELECT * FROM {_keyspaceName}.{_tableName} WHERE {whereClause}", values);

        public async Task<T?> FirstOrDefaultAsync(string whereClause, params object[] values) =>
            await _context.Mapper.FirstOrDefaultAsync<T>(
                $"WHERE {whereClause}", values);

    }
}
