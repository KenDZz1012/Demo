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

namespace Channel.Application.Features.Server.Commands.CreateServer
{
    public class CreateServerHandler : IRequestHandler<CreateServer, ApiResponse<Guid>>
    {
        private readonly IMinioService _minioService;
        private readonly IServerRepository _serverRepository;
        private readonly IMapper _mapper;

        public CreateServerHandler(IMinioService minioService, IServerRepository serverRepository, IMapper mapper)
        {
            _minioService = minioService;
            _serverRepository = serverRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<Guid>> Handle(CreateServer request, CancellationToken cancellationToken)
        {
            try
            {
                var server = _mapper.Map<Domain.Entities.Server>(request);
                if (request.IconUrl != null)
                {
                    var fileMinio = new MinioFile()
                    {
                        FileName = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "_" + request.OwnerId + "_" +
                                   request.Name + "_" + request.IconUrl.FileName,
                        formFile = request.IconUrl.OpenReadStream(),
                        Size = request.IconUrl.Length,
                    };
                    // Upload icon to Minio
                    var postFileResponse = await _minioService.PostFileAsync(fileMinio, "server-icons");
                    if (postFileResponse.IsSuccess)
                    {
                        server.IconUrl = postFileResponse.Data.FilePath;
                    }
                    else
                    {
                        server.IconUrl = null;
                    }
                }

                var isCreatedSuccess = await _serverRepository.AddAsync(server);

                return isCreatedSuccess
                    ? ApiResponse<Guid>.Success(server.Id, "Create server successfully")
                    : ApiResponse<Guid>.Failure("500", "Create server failed");
            }
            catch (Exception ex)
            {
                return await Task.FromResult(ApiResponse<Guid>.Failure("500", ex.Message));
            }
        }
    }
}