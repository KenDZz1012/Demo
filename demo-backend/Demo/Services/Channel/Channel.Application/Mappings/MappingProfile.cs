using Channel.Application.Features.Server.Queries.GetServers;
using AutoMapper;
using Channel.Application.Features.Channel.Commands.CreateChannel;
using Channel.Application.Features.Channel.Queries.GetChannels;
using Channel.Application.Features.Server.Commands.CreateServer;
using Channel.Application.Features.Server.Queries.GetServer;
using Channel.Application.Features.ServerMember.Commands.CreateServerMember;
using Channel.Domain.Entities;
using Channel.Application.Features.Server.Queries.GetServerDetail;

namespace Channel.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Server, GetServersVm>().ReverseMap();
            CreateMap<Server, GetServerVm>().ReverseMap();

            CreateMap<Server, GetServerDetailVm>()
                .ForMember(dest => dest.Code, 
                    opt => opt.MapFrom(src => src.ServerInviteLinks.FirstOrDefault().Code));
            
            CreateMap<Domain.Entities.Channel, Features.Server.Queries.GetServerDetail.Channel>().ReverseMap();
            CreateMap<Domain.Entities.ServerMember, Features.Server.Queries.GetServerDetail.ServerMember>()
                .ReverseMap();


            CreateMap<Server, CreateServer>().ReverseMap();
            CreateMap<Domain.Entities.Channel, CreateChannel>().ReverseMap();

            CreateMap<Domain.Entities.ServerMember, CreateServerMember>().ReverseMap();
        }
    }
}