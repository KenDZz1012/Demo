using System.Data;
using Catalog.API.Interface;
using Catalog.API.Model;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Service.Lib.Context;

namespace Catalog.API.Repositories
{
    public class TestCodeRepository : ITestCodeRepository
    {
        private readonly IDbConnection _connection;
        public TestCodeRepository(DapperContext dapperContext)
        {
            _connection = dapperContext.CreateConnection();
        }

        public async Task<List<TestCodeInfo>> GetTestCode()
        {
            string query = " Select * from tbl_TestCode ";
            var testcodes = await _connection.QueryAsync<TestCodeInfo>(query);
            return testcodes.ToList();

        }

        public async Task<bool> PostTestCode(TestCodeInfo testCode)
        {
            string command = " Insert into tbl_TestCode(TestCode, TestName, Category, Type, NormalRange, Unit, Price) values (@TestCode, @TestName, @Category, @Type, @NormalRange, @Unit ,@Price) ";
            return await _connection.ExecuteAsync(command, testCode) > 0;

        }
    }
}
