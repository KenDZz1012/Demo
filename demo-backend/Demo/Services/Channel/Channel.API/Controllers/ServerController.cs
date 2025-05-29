using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Channel.API.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class ServerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ServerController(IMediator mediator)
        {
            _mediator = mediator;  
        }
    }
}
