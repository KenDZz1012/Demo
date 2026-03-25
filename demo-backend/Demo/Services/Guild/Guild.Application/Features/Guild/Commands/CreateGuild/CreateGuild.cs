using MediatR;
using Microsoft.AspNetCore.Http;
using Service.Lib.BaseResponse;

namespace Guild.Application.Features.Guild.Commands.CreateGuild
{
    public class CreateGuild : IRequest<ApiResponse<Guid>>
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public Guid OwnerId { get; set; }
        public IFormFile? Icon { get; set; }
    }
}
