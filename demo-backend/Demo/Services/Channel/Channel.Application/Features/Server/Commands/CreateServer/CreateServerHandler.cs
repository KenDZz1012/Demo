using AutoMapper;
using Channel.Application.Contracts.Persistence;
using Channel.Domain.Common.Constants;
using Channel.Domain.Entities;
using MediatR;
using Service.Lib.BaseResponse;
using Service.Lib.Minio;
using Service.Lib.SecureCodeGenerator;

namespace Channel.Application.Features.Server.Commands.CreateServer
{
    public class CreateServerHandler : IRequestHandler<CreateServer, ApiResponse<Guid>>
    {
        private readonly IMinioService _minioService;
        private readonly IServerRepository _serverRepository;
        private readonly IMapper _mapper;
        private readonly IServerMemberRepository _serverMemberRepository;
        private readonly IServerInviteLinkRepository _serverInviteLinkRepository;

        public CreateServerHandler(
            IMinioService minioService,
            IServerRepository serverRepository,
            IMapper mapper,
            IServerMemberRepository serverMemberRepository,
            IServerInviteLinkRepository serverInviteLinkRepository)
        {
            _minioService = minioService;
            _serverRepository = serverRepository;
            _mapper = mapper;
            _serverMemberRepository = serverMemberRepository;
            _serverInviteLinkRepository = serverInviteLinkRepository;
        }

        public async Task<ApiResponse<Guid>> Handle(CreateServer request, CancellationToken cancellationToken)
        {
            try
            {
                var server = _mapper.Map<Domain.Entities.Server>(request);
                var isCreated = await _serverRepository.AddAsync(server);

                if (!isCreated)
                    return ApiResponse<Guid>.Failure("500", "Create server failed");

                var serverMember = new Domain.Entities.ServerMember
                {
                    ServerId = server.Id,
                    UserId = request.OwnerId,
                    Role = ServerMemberRole.Owner
                };
                await _serverMemberRepository.AddAsync(serverMember);

                var inviteLink = new ServerInviteLink
                {
                    Serverid = server.Id,
                    Createdby = request.OwnerId,
                    Createdat = DateTime.UtcNow,
                    Code = SecureCodeGenerator.GenerateSecureInviteCode(8)
                };
                await _serverInviteLinkRepository.AddAsync(inviteLink);

                return ApiResponse<Guid>.Success(server.Id, "Create server successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<Guid>.Failure("500", ex.Message);
            }
        }
    }
}
