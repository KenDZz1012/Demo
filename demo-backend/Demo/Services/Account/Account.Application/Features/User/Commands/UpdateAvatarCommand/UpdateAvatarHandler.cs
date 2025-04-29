using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Account.Application.Contracts.Persistence;
using AutoMapper;
using MediatR;
using Minio;
using Minio.DataModel;
using Service.Lib.BaseResponse;
using Service.Lib.Minio;
using Service.Lib.Password;

namespace Account.Application.Features.User.Commands.UpdateAvatarCommand
{
    public class UpdateAvatarHandler : IRequestHandler<UpdateAvatar, ApiResponse<Guid>>
    {
        private readonly IMinioService _minioService;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UpdateAvatarHandler(IUserRepository userRepository, IMapper mapper, IMinioService minioService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _minioService = minioService;
        }
        public async Task<ApiResponse<Guid>> Handle(UpdateAvatar request, CancellationToken cancellationToken)
        {
            try
            {
                var existingUser = await _userRepository.GetByIdAsync(request.ID);
                if (existingUser == null)
                {
                    return ApiResponse<Guid>.Failure("404", "User not found");
                }
                else
                {
                    var fileMinio = new MinioFile()
                    {
                        FileName = request.ID + "_" + Guid.NewGuid() + "_" + request.File.FileName,
                        formFile = request.File.OpenReadStream(),
                        Size = request.File.Length,
                    };
                    var postFileResponse = await _minioService.PostFileAsync(fileMinio, "user-avatar");
                    if (postFileResponse.IsSuccess)
                    {
                        var userUpdate = await _userRepository.GetByIdAsync(request.ID);
                        userUpdate.AvatarUrl = postFileResponse.Data.FilePath;
                        var isUpdatedSuccess = await _userRepository.UpdateAsync(userUpdate);
                        return isUpdatedSuccess ? ApiResponse<Guid>.Success(request.ID, "Cập nhật Avatar thành công") : ApiResponse<Guid>.Failure("500", "Không cập nhật được Avatar");
                    }
                    else
                    {
                        return ApiResponse<Guid>.Failure("500", postFileResponse.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                return await Task.FromResult(ApiResponse<Guid>.Failure("500", ex.Message));
            }
        }
    }
}
