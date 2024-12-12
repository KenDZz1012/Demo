using Catalog.API.Model;
using Dapper;
using Service.Lib.Context;

namespace Catalog.API.Repositories
{
    public class CatalogRepository : ICatalogRepository
    {
        private readonly DapperContext _dapperContext;
        public CatalogRepository(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task<List<TestCodeInfo>> GetTestCode()
        {
            string query = " Select * from tbl_TestCode ";
            using (var connection = _dapperContext.CreateConnection())
            {
                var testcodes = await connection.QueryAsync<TestCodeInfo>(query);
                return testcodes.ToList();
            }
        }
    }
}
