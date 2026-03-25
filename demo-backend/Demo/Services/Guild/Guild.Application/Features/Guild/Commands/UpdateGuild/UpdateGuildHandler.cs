using Guild.Application.Contracts.Persistence;
using MediatR;
using Service.Lib.BaseResponse;
using Service.Lib.Minio;

namespace Guild.Application.Features.Guild.Commands.UpdateGuild
{
    public class UpdateGuildHandler : IRequestHandler<UpdateGuild, ApiResponse<bool>>
    {
        private readonly IGuildRepository _guildRepository;
        private readonly IMinioService _minioService;

        public UpdateGuildHandler(IGuildRepository guildRepository, IMinioService minioService)
        {
            _guildRepository = guildRepository;
            _minioService = minioService;
        }

        public async Task<ApiResponse<bool>> Handle(UpdateGuild request, CancellationToken cancellationToken)
        {
            try
            {
                var guild = await _guildRepository.GetByIdAsync(request.GuildId);
                if (guild == null)
                    return ApiResponse<bool>.Failure("404", "Guild not found");

                if (request.Name != null) guild.Name = request.Name;
                if (request.Description != null) guild.Description = request.Description;

                if (request.Icon != null)
                {
                    var minioFile = new MinioFile
                    {
                        formFile = request.Icon.OpenReadStream(),
                        FileName = request.Icon.FileName,
                        Size = request.Icon.Length
                    };
                    var uploadResult = await _minioService.PostFileAsync(minioFile, "guild-icons");
                    if (uploadResult.IsSuccess)
                        guild.IconUrl = uploadResult.Data?.FilePath;
                }

                guild.UpdatedAt = DateTime.UtcNow;
                guild.UpdatedBy = request.UpdatedBy;

                var isUpdated = await _guildRepository.UpdateAsync(guild);
                return isUpdated
                    ? ApiResponse<bool>.Success(true, "Update guild successfully")
                    : ApiResponse<bool>.Failure("500", "Update guild failed");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Failure("500", ex.Message);
            }
        }
    }
}
