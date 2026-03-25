using Guild.Application.Contracts.Persistence;
using Guild.Domain.Entities;
using MediatR;
using Service.Lib.BaseResponse;
using Service.Lib.Minio;
using Service.Lib.SecureCodeGenerator;
using GuildEntity = Guild.Domain.Entities.Guild;
using GuildMemberEntity = Guild.Domain.Entities.GuildMember;

namespace Guild.Application.Features.Guild.Commands.CreateGuild
{
    public class CreateGuildHandler : IRequestHandler<CreateGuild, ApiResponse<Guid>>
    {
        private readonly IGuildRepository _guildRepository;
        private readonly IGuildMemberRepository _guildMemberRepository;
        private readonly IGuildInviteRepository _guildInviteRepository;
        private readonly IMinioService _minioService;

        public CreateGuildHandler(
            IGuildRepository guildRepository,
            IGuildMemberRepository guildMemberRepository,
            IGuildInviteRepository guildInviteRepository,
            IMinioService minioService)
        {
            _guildRepository = guildRepository;
            _guildMemberRepository = guildMemberRepository;
            _guildInviteRepository = guildInviteRepository;
            _minioService = minioService;
        }

        public async Task<ApiResponse<Guid>> Handle(CreateGuild request, CancellationToken cancellationToken)
        {
            try
            {
                string? iconUrl = null;
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
                        iconUrl = uploadResult.Data?.FilePath;
                }

                var guild = new GuildEntity
                {
                    Name = request.Name,
                    Description = request.Description,
                    OwnerId = request.OwnerId,
                    IconUrl = iconUrl,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = request.OwnerId
                };
                await _guildRepository.AddAsync(guild);

                var ownerMember = new GuildMemberEntity
                {
                    GuildId = guild.Id,
                    UserId = request.OwnerId,
                    JoinedAt = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = request.OwnerId
                };
                await _guildMemberRepository.AddAsync(ownerMember);

                var invite = new GuildInvite
                {
                    GuildId = guild.Id,
                    Code = SecureCodeGenerator.GenerateSecureInviteCode(8),
                    CreatorId = request.OwnerId,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = request.OwnerId
                };
                await _guildInviteRepository.AddAsync(invite);

                return ApiResponse<Guid>.Success(guild.Id, "Create guild successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<Guid>.Failure("500", ex.Message);
            }
        }
    }
}
