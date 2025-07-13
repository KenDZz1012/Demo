using MediatR;
using Service.Lib.BaseResponse;
using Service.Lib.Minio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Channel.Application.Features.Server.Commands.UpdateServerIcon
{
    public class UpdateServerIconHandler : IRequestHandler<UpdateServerIcon, ApiResponse<string>>
    {
        private readonly IMinioService _minioService;

        public UpdateServerIconHandler(IMinioService minioService)
        {
            _minioService = minioService;
        }

        public async Task<ApiResponse<string>> Handle(UpdateServerIcon request, CancellationToken cancellationToken)
        {
            var fileMinio = new MinioFile()
            {
                FileName = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "_" + Guid.NewGuid() + "_" + request.IconUrl.FileName,
                formFile = request.IconUrl.OpenReadStream(),
                Size = request.IconUrl.Length,
            };

            var postFileResponse = await _minioService.PostFileAsync(fileMinio, "server-icons");
            if (postFileResponse.IsSuccess)
            {
                return ApiResponse<string>.Success(postFileResponse.Data.FilePath, "Upload server icon successfully");
            }
            else
            {
                return ApiResponse<string>.Failure("500", "Upload server icon error");
            }
        }
    }
}
