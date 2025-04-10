using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using User.Application.Interfaces;
using User.Domain.Entities;

namespace User.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAccounts()
        {
            var accounts = await _accountService.GetAllAsync();
            return Ok(accounts);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccountById(string id)
        {
            var user = await _accountService.GetByIdAsync(id);
            return user != null ? Ok(user) : NotFound();
        }
    }
}
