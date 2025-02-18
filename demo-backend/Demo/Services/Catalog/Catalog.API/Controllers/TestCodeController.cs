using Catalog.API.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Catalog.API.Interface;

namespace Catalog.API.Controllers
{
    [Route("v1/")]
    [ApiController]
    public class TestCodeController : ControllerBase
    {
        private readonly ITestCodeRepository _testCodeRepository;

        public TestCodeController(ITestCodeRepository testCodeRepository)
        {
            _testCodeRepository = testCodeRepository;
        }

        [HttpGet("TestCode")]
        public async Task<IActionResult> GetTestCodeAsync()
        {
            var result = await _testCodeRepository.GetTestCode();
            return Ok(result);
        }

        [HttpGet("{testCode}")]
        public async Task<IActionResult> GetTestCodeAsync(string testCode)
        {
            var testcode = await _testCodeRepository.GetTestByTestCode(testCode);

            if (testcode != null)
            {
                return Ok(testcode);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("TestCode", Name = "PostTestCode")]
        public async Task<IActionResult> PostTestCodeAsync([FromBody] TestCodeInfo testcode)
        {
            var result = await _testCodeRepository.PostTestCode(testcode);
            if (result.Success)
            {
                return CreatedAtAction(nameof(GetTestCodeAsync), new { testCode = testcode.TestCode }, result);
            }
            else
            {
                return BadRequest(result);
            }
        }
    }
}
