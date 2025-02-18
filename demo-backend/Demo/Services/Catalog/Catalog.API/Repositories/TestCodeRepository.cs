using System.Data;
using Catalog.API.Interface;
using Catalog.API.Model;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Service.Lib.BaseResponse;
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

        public async Task<ApiResponse<List<TestCodeInfo>>> GetTestCode()
        {
            string query = " Select * from tbl_TestCode ";
            var testcodes = await _connection.QueryAsync<TestCodeInfo>(query);
            return new ApiResponse<List<TestCodeInfo>>(true, @$"Lấy danh sách thành công", testcodes.ToList());
        }

        public async Task<TestCodeInfo> GetTestByTestCode(string testCode)
        {
            string query = " Select * from tbl_TestCode where TestCode = @TestCode";
            var testcode = await _connection.QueryFirstAsync<TestCodeInfo>(query, new { TestCode = testCode });
            return testcode;
        }

        public async Task<ApiResponse<bool>> PostTestCode(TestCodeInfo testCode)
        {
            var testCodeExist = await GetTestByTestCode(testCode.TestCode);
            if (testCodeExist == null)
            {
                string command = " Insert into tbl_TestCode(TestCode, TestName, Category, Type, NormalRange, Unit, Price) values (@TestCode, @TestName, @Category, @Type, @NormalRange, @Unit ,@Price) ";
                var result = await _connection.ExecuteAsync(command, testCode);

                if (result > 0)
                {
                    return new ApiResponse<bool>(true, "Thêm mới xét nghiệm thành công", true);
                }
                else
                {
                    return new ApiResponse<bool>(false, "Không thể thêm mới xét nghiệm", false);
                }
            }
            else
            {
                return new ApiResponse<bool>(false, @$"Mã XN {testCode.TestCode} đã tồn tại", false);
            }
        }


    }
}
