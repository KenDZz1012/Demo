using Channel.Application.Contracts.Persistence;
using MediatR;
using Service.Lib.BaseResponse;
using Service.Lib.Minio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Channel.Application.Features.Server.Queries.GetServers;

namespace Channel.Application.Features.Server.Commands.CreateServer
{
    public class CreateServerHandler : IRequestHandler<CreateServer, ApiResponse<GetServersVm>>
    {
        private readonly IMinioService _minioService;
        private readonly IServerRepository _serverRepository;
        private readonly IMapper _mapper;
        private readonly IServerMemberRepository _serverMemberRepository;
        public CreateServerHandler(IMinioService minioService, IServerRepository serverRepository, IMapper mapper, IServerMemberRepository serverMemberRepository)
        {
            _minioService = minioService;
            _serverRepository = serverRepository;
            _mapper = mapper;
            _serverMemberRepository = serverMemberRepository;
        }

        public async Task<ApiResponse<GetServersVm>> Handle(CreateServer request, CancellationToken cancellationToken)
        {
            try
            {
                var server = _mapper.Map<Domain.Entities.Server>(request);
                var isCreatedSuccess = await _serverRepository.AddAsync(server);
                if (isCreatedSuccess)
                {
                    var serverMember = new Domain.Entities.ServerMember
                    {
                        ServerId = server.Id,
                        UserId = request.OwnerId,
                        Role = "Owner",
                    };
                    await _serverMemberRepository.AddAsync(serverMember);
                }
                var serverVm = _mapper.Map<GetServersVm>(server);
                return isCreatedSuccess
                    ? ApiResponse<GetServersVm>.Success(serverVm, "Create server successfully")
                    : ApiResponse<GetServersVm>.Failure("500", "Create server failed");
            }
            catch (Exception ex)
            {
                return await Task.FromResult(ApiResponse<GetServersVm>.Failure("500", ex.Message));
            }
        }
    }
}