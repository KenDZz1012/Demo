using Catalog.API.Model;

namespace Catalog.API.Repositories
{
    public interface ICatalogRepository
    {
        Task<List<TestCodeInfo>> GetTestCode();
    }
}
