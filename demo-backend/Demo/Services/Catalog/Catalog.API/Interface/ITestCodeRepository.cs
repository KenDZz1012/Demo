using Catalog.API.Model;

namespace Catalog.API.Interface
{
    public interface ITestCodeRepository
    {
        Task<List<TestCodeInfo>> GetTestCode();

        Task<bool> PostTestCode(TestCodeInfo testCode);
    }
}
