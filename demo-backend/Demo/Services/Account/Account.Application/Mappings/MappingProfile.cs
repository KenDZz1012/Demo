using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Account.Application.Features.User.Commands.CreateUserCommand;
using Account.Application.Features.User.Commands.UpdateUserCommand;
using Account.Application.Features.User.Queries.GetUserByIDQuery;
using Account.Application.Features.User.Queries.GetUsersQuery;
using Account.Application.Features.UserRelationship.Commands.CreateUserRelationshipCommand;
using Account.Application.Features.UserRelationship.Queries.GetListFriendQuery;
using Account.Application.Features.UserRelationship.Queries.GetListUserRelationshipQuery;
using Account.Domain.Entities;
using AutoMapper;

namespace Account.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //User
            CreateMap<User, GetUsersVm>().ReverseMap();
            CreateMap<User, GetUserByIDVm>().ReverseMap();
            CreateMap<User, CreateUser>().ReverseMap();
            CreateMap<User, UpdateUser>().ReverseMap();


            //UserRelationship
            CreateMap<UserRelationship, GetListUserRelationshipVm>()
                .ForMember(dest => dest.RequesterName, opt => opt.MapFrom(src => src.Requester.UserName))
                .ForMember(dest => dest.AddresseeName, opt => opt.MapFrom(src => src.Addressee.UserName))
               ;
            CreateMap<UserRelationship, CreateUserRelationship>().ReverseMap();
            CreateMap<User, GetListFriendVm>().ReverseMap();
        }
    }
}
