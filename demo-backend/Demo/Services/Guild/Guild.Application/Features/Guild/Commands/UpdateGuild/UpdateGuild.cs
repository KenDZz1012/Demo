using MediatR;
using Microsoft.AspNetCore.Http;
using Service.Lib.BaseResponse;

namespace Guild.Application.Features.Guild.Commands.UpdateGuild
{
    public class UpdateGuild : IRequest<ApiResponse<bool>>
    {
        public Guid GuildId { get; set; }
        public Guid UpdatedBy { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public IFormFile? Icon { get; set; }
    }
}
