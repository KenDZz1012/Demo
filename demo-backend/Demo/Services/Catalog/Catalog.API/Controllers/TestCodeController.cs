using Catalog.API.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Catalog.API.Interface;

namespace Catalog.API.Controllers
{
    [Route("v1/TestCode")]
    [ApiController]
    public class TestCodeController : ControllerBase
    {
        private readonly ITestCodeRepository _testCodeRepository;

        public TestCodeController(ITestCodeRepository testCodeRepository)
        {
            _testCodeRepository = testCodeRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetTestCodeAsync()
        {
            var result = await _testCodeRepository.GetTestCode();
            return Ok(result);
        }

        [HttpGet("{testCode}")]
        public async Task<IActionResult> GetTestCodeAsync(string testCode)
        {
            var testcode = await _testCodeRepository.GetTestByTestCode(testCode);
            return Ok(testcode);
        }

        [HttpPost(Name = "PostTestCode")]
        public async Task<IActionResult> PostTestCodeAsync([FromBody] TestCodeInfo testcode)
        {
            var result = await _testCodeRepository.PostTestCode(testcode);
            if (result.Success)
            {
                return Created("PostTestCode", result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPut(Name = "PutTestCode")]
        public async Task<IActionResult> PutTestCodeAsync([FromBody] TestCodeInfo testcode)
        {
            var result = await _testCodeRepository.PutTestCode(testcode);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }


        [HttpDelete("{testCode}", Name = "DeleteTestCode")]
        public async Task<IActionResult> DeleteTestCodeAsync(string testCode)
        {
            var result = await _testCodeRepository.DeleteTestCode(testCode);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
    }
}
