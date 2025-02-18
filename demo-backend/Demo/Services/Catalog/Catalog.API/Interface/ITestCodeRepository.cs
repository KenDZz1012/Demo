using Catalog.API.Model;
using Service.Lib.BaseResponse;

namespace Catalog.API.Interface
{
    public interface ITestCodeRepository
    {
        Task<ApiResponse<List<TestCodeInfo>>> GetTestCode();

        Task<ApiResponse<TestCodeInfo>> GetTestByTestCode(string testCode);

        Task<ApiResponse<bool>> PostTestCode(TestCodeInfo testCode);

        Task<ApiResponse<bool>> PutTestCode(TestCodeInfo testCode);

        Task<ApiResponse<bool>> DeleteTestCode(string TestCode);
    }
}
