using Channel.Application.Contracts.Persistence;
using MediatR;
using Service.Lib.BaseResponse;
using Service.Lib.Minio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Channel.Application.Features.Server.Commands.CreateServer
{
    public class CreateServerHandler : IRequestHandler<CreateServer, ApiResponse<Guid>>
    {
        private readonly IMinioService _minioService;
        private readonly IServerRepository _serverRepository;
        private readonly IUserValidationService _userValidationService;

        public CreateServerHandler(IMinioService minioService, IServerRepository serverRepository, IUserValidationService userValidationService)
        {
            _minioService = minioService;
            _serverRepository = serverRepository;
            _userValidationService = userValidationService;
        }

        public Task<ApiResponse<Guid>> Handle(CreateServer request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
