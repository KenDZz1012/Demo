using Account.Grpc.Context;
using Account.Grpc.Protos;
using AutoMapper;


namespace Account.Grpc.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //User
            CreateMap<UserModel, User>().ReverseMap();
        }
    }
}
