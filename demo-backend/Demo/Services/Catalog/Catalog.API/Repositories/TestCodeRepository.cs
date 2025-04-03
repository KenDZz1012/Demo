using System.Data;
using Catalog.API.Interface;
using Catalog.API.Model;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Service.Lib.BaseResponse;
using Service.Lib.Context;

namespace Catalog.API.Repositories
{
    public class TestCodeRepository : ITestCodeRepository
    {
        private readonly IDbConnection _connection;
        private readonly ILogger<TestCodeRepository> _logger;
        public TestCodeRepository(DapperContext dapperContext)
        {
            _connection = dapperContext.CreateConnection();
        }

        public async Task<ApiResponse<List<TestCodeInfo>>> GetTestCode()
        {
            string query = " Select * from tbl_TestCode ";
            var testcodes = await _connection.QueryAsync<TestCodeInfo>(query);
            Log.Information("Executed SQL Query: {Query}", query);
            return new ApiResponse<List<TestCodeInfo>>(true, @$"Lấy danh sách thành công", testcodes.ToList());
        }

        public async Task<ApiResponse<TestCodeInfo>> GetTestByTestCode(string testCode)
        {
            string query = " Select * from tbl_TestCode where TestCode = @TestCode";
            var testcode = await _connection.QueryFirstOrDefaultAsync<TestCodeInfo>(query, new { TestCode = testCode });
            if(testcode != null)
            {
                return new ApiResponse<TestCodeInfo>(true, @$"Lấy xét nghiệm thành công", testcode);
            }
            else
            {
                return new ApiResponse<TestCodeInfo>(false, @$"Không tìm thấy xét nghiệm", testcode);
            }
        }

        public async Task<ApiResponse<bool>> PostTestCode(TestCodeInfo testCode)
        {
            var testCodeExist = await GetTestByTestCode(testCode.TestCode);
            if (testCodeExist.Data == null)
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

        public async Task<ApiResponse<bool>> PutTestCode(TestCodeInfo testCode)
        {
            var testCodeExist = await GetTestByTestCode(testCode.TestCode);
            if (testCodeExist != null)
            {
                string command = " Update tbl_TestCode set TestName = @TestName, Category = @Category, Type = @Type, NormalRange = @NormalRange, Unit = @Unit, Price = @Price where TestCode = @TestCode ";
                var result = await _connection.ExecuteAsync(command, testCode);

                if (result > 0)
                {
                    return new ApiResponse<bool>(true, "Cập nhật xét nghiệm thành công", true);
                }
                else
                {
                    return new ApiResponse<bool>(false, "Không thể cập nhật xét nghiệm", false);
                }
            }
            else
            {
                return new ApiResponse<bool>(false, @$"Mã XN {testCode.TestCode} không tồn tại", false);
            }
        }

        public async Task<ApiResponse<bool>> DeleteTestCode(string TestCode)
        {
            var testCodeExist = await GetTestByTestCode(TestCode);
            if (testCodeExist != null)
            {
                string command = " Delete tbl_TestCode where TestCode = @TestCode ";
                var result = await _connection.ExecuteAsync(command, new { TestCode = TestCode });

                if (result > 0)
                {
                    return new ApiResponse<bool>(true, "Xóa xét nghiệm thành công", true);
                }
                else
                {
                    return new ApiResponse<bool>(false, "Không thể xóa xét nghiệm", false);
                }
            }
            else
            {
                return new ApiResponse<bool>(false, @$"Mã XN {TestCode} không tồn tại", false);
            }
        }
    }
}
