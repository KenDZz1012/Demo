using Service.Lib.BaseCatalog;

namespace Catalog.API.Model
{
    public class TestTypeInfo : BaseCatalog
    {
        public string TestTypeID { get; set; }

        public string TestTypeName { get; set; }

        public string Enabled { get; set; }
    }
}
