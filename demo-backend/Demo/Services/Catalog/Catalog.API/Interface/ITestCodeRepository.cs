using Catalog.API.Model;
using Service.Lib.BaseResponse;

namespace Catalog.API.Interface
{
    public interface ITestCodeRepository
    {
        Task<ApiResponse<List<TestCodeInfo>>> GetTestCode();

        Task<ApiResponse<bool>> PostTestCode(TestCodeInfo testCode);
    }
}
