using MediatR;
using Service.Lib.BaseResponse;
using AutoMapper;
using Channel.Application.Contracts.Persistence;
using Channel.Application.GrpcServices;

namespace Channel.Application.Features.Server.Queries.GetServers
{
    public class GetServersHandler : IRequestHandler<GetServers, ApiResponse<List<GetServersVm>>>
    {
        public readonly IServerRepository _serverRepository;
        public readonly IMapper _mapper;
        private readonly UserGrpcService _userGrpcService;

        public GetServersHandler(IServerRepository serverRepository, IMapper mapper, UserGrpcService userGrpcService)
        {
            _serverRepository = serverRepository;
            _mapper = mapper;
            _userGrpcService = userGrpcService;
        }

        public async Task<ApiResponse<List<GetServersVm>>> Handle(GetServers request,
            CancellationToken cancellationToken)
        {
            try
            {
                var servers = await _serverRepository.GetServers(request);
                var serverDto = _mapper.Map<List<GetServersVm>>(servers);
                var userIds = serverDto
                    .SelectMany(s => s.ServerMembers)
                    .Select(m => m.UserId.ToString())
                    .Distinct()
                    .ToList();
                var userResponse = await _userGrpcService.GetUserInfoInChannel(userIds);
                var userDict = userResponse.Users.ToDictionary(u => u.Id, u => u);

                if (serverDto.Any())
                {
                    foreach (var server in serverDto)
                    {
                        
                        if (server.ServerMembers.Any())
                        {
                            foreach (var member in server.ServerMembers)
                            {
                                var userIdStr = member.UserId.ToString();
                                if (userDict.TryGetValue(userIdStr, out var userInfo))
                                {
                                    member.UserName = userInfo.UserName;
                                    member.AvatarUrl = userInfo.AvatarUrl;
                                    member.Email = userInfo.Email;
                                    member.DisplayName = userInfo.DisplayName;
                                }
                            }
                        }
                    }
                }

                return ApiResponse<List<GetServersVm>>.Success(serverDto, "Get list server successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<GetServersVm>>.Failure("500", ex.Message);
            }
        }
    }
}