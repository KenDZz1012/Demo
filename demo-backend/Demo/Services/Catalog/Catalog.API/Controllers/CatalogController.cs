using Catalog.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [Route("v1/")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly ICatalogRepository _catalogRepository;

        public CatalogController(ICatalogRepository catalogRepository)
        {
            _catalogRepository = catalogRepository;
        }

        [HttpGet("Catalog/TestCode")]
        public async Task<IActionResult> GetTestCodeAsync()
        {
            var testcodes = await _catalogRepository.GetTestCode();

            return Ok(testcodes);
        }
    }
}
