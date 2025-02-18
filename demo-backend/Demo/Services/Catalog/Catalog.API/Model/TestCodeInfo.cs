using System.ComponentModel.DataAnnotations;

namespace Catalog.API.Model
{
    public class TestCodeInfo
    {
        public string TestCode { get; set; }

        public string TestName { get; set; }

        public string? Category { get; set; }

        public string? Type { get; set; }

        public string? NormalRange { get; set; }

        public string? Unit { get; set; }

        public double? Price { get; set; }
    }
}
