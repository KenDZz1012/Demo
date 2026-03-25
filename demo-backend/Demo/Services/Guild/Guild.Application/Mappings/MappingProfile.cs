using AutoMapper;
using Guild.Application.Features.Guild.Queries.GetGuild;
using Guild.Application.Features.Guild.Queries.GetGuilds;
using Guild.Application.Features.GuildMember.Queries.GetGuildMembers;
using GuildEntity = Guild.Domain.Entities.Guild;

namespace Guild.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<GuildEntity, GetGuildsVm>();
            CreateMap<GuildEntity, GetGuildVm>();
            CreateMap<Guild.Domain.Entities.GuildMember, GetGuildMembersVm>();
        }
    }
}
