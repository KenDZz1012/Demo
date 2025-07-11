using Account.Domain.Entities;
using Account.Grpc.Protos;
using AutoMapper;


namespace Account.Grpc.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //User
            CreateMap<User, UserModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.DisplayName))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.AvatarUrl, opt => opt.MapFrom(src => src.AvatarUrl));
        }
    }
}
