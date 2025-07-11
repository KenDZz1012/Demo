
using Channel.Application.Features.Server.Queries.GetServers;
using AutoMapper;
using Channel.Application.Features.Channel.Commands.CreateChannel;
using Channel.Application.Features.Channel.Queries.GetChannels;
using Channel.Application.Features.Server.Commands.CreateServer;
using Channel.Application.Features.ServerMember.Commands.CreateServerMember;
using Channel.Domain.Entities;
using ServerMember = Channel.Domain.Entities.ServerMember;

namespace Channel.Application.Mappings
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<Server, GetServersVm>().ReverseMap();
            CreateMap<Domain.Entities.Channel, Features.Server.Queries.GetServers.Channel>().ReverseMap();
            CreateMap<ServerMember, Features.Server.Queries.GetServers.ServerMember>().ReverseMap();
            
            CreateMap<Server, CreateServer>().ReverseMap();
            CreateMap<Domain.Entities.Channel, CreateChannel>().ReverseMap();

            CreateMap<ServerMember, CreateServerMember>().ReverseMap();
        }
    }
}
