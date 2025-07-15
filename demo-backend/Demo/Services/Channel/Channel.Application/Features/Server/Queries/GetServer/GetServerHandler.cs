using AutoMapper;
using Channel.Application.Contracts.Persistence;
using Channel.Application.Features.Server.Queries.GetServers;
using Channel.Application.GrpcServices;
using MediatR;
using Service.Lib.BaseResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Channel.Application.Features.Server.Queries.GetServer
{
    public class GetServerHandler : IRequestHandler<GetServer, ApiResponse<GetServerVm>>
    {
        public readonly IServerRepository _serverRepository;
        public readonly IMapper _mapper;
        private readonly UserGrpcService _userGrpcService;

        public GetServerHandler(IServerRepository serverRepository, IMapper mapper, UserGrpcService userGrpcService)
        {
            _serverRepository = serverRepository;
            _mapper = mapper;
            _userGrpcService = userGrpcService;
        }

        public async Task<ApiResponse<GetServerVm>> Handle(GetServer request, CancellationToken cancellationToken)
        {
            try
            {
                var server = await _serverRepository.GetServer(request.Id);
                var serverDto = _mapper.Map<GetServerVm>(server);

                if (serverDto.ServerMembers.Any())
                {
                    foreach (var member in serverDto.ServerMembers)
                    {
                        var userInfo = await _userGrpcService.GetUserInfoInChannel(member.UserId.ToString());
                        member.UserName = userInfo.UserName;
                        member.AvatarUrl = userInfo.AvatarUrl;
                        member.Email = userInfo.Email;
                        member.DisplayName = userInfo.DisplayName;
                    }
                }

                return ApiResponse<GetServerVm>.Success(serverDto, "Get server successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<GetServerVm>.Failure("500", ex.Message);
            }
        }
    }
}
