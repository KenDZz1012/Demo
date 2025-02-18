using Service.Lib.BaseCatalog;

namespace Catalog.API.Model
{
    public class CategoryInfo : BaseCatalog
    {
        public string CategoryID { get; set; }

        public string CategoryName { get; set; }

        public bool Enabled { get; set; }
    }
}
