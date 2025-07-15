
using Channel.Application.Features.Server.Queries.GetServers;
using AutoMapper;
using Channel.Application.Features.Channel.Commands.CreateChannel;
using Channel.Application.Features.Channel.Queries.GetChannels;
using Channel.Application.Features.Server.Commands.CreateServer;
using Channel.Application.Features.ServerMember.Commands.CreateServerMember;
using Channel.Domain.Entities;
using Channel.Application.Features.Server.Queries.GetServer;

namespace Channel.Application.Mappings
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<Server, GetServersVm>().ReverseMap();
            CreateMap<Domain.Entities.Channel, Features.Server.Queries.GetServers.Channel>().ReverseMap();
            CreateMap<Domain.Entities.ServerMember, Features.Server.Queries.GetServers.ServerMember>().ReverseMap();

            CreateMap<Server, GetServerVm>().ReverseMap();
            CreateMap<Domain.Entities.Channel, Features.Server.Queries.GetServer.Channel>().ReverseMap();
            CreateMap<Domain.Entities.ServerMember, Features.Server.Queries.GetServer.ServerMember>().ReverseMap();


            CreateMap<Server, CreateServer>().ReverseMap();
            CreateMap<Domain.Entities.Channel, CreateChannel>().ReverseMap();

            CreateMap<Domain.Entities.ServerMember, CreateServerMember>().ReverseMap();
        }
    }
}
