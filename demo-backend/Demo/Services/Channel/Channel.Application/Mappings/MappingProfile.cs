using AutoMapper;
using Channel.Application.Features.Channel.Commands.CreateChannel;
using Channel.Application.Features.Channel.Queries.GetChannels;

namespace Channel.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Domain.Entities.Channel, GetChannelsVm>().ReverseMap();
            CreateMap<Domain.Entities.Channel, CreateChannel>().ReverseMap();
        }
    }
}
