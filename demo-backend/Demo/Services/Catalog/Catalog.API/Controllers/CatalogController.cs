using Catalog.API.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Catalog.API.Interface;

namespace Catalog.API.Controllers
{
    [Route("v1/")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly ITestCodeRepository _testCodeRepository;

        public CatalogController(ITestCodeRepository testCodeRepository)
        {
            _testCodeRepository = testCodeRepository;
        }

        [HttpGet("TestCode")]
        public async Task<IActionResult> GetTestCodeAsync()
        {
            var testcodes = await _testCodeRepository.GetTestCode();
            return Ok(testcodes);
        }

        [HttpPost("TestCode")]
        public async Task<IActionResult> PostTestCodeAsync(TestCodeInfo testcode)
        {
            var testcodes = await _testCodeRepository.GetTestCode();
            return Created("TestCode", testcodes);
        }
    }
}
