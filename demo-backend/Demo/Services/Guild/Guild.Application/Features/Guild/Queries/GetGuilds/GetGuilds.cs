using MediatR;
using Service.Lib.BaseResponse;

namespace Guild.Application.Features.Guild.Queries.GetGuilds
{
    public class GetGuilds : IRequest<ApiResponse<List<GetGuildsVm>>>
    {
        public Guid UserId { get; set; }

        public GetGuilds(Guid userId)
        {
            UserId = userId;
        }
    }
}
