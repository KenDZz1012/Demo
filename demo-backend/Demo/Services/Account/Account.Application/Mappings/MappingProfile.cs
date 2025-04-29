using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Account.Application.Features.User.Commands.CreateUserCommand;
using Account.Application.Features.User.Commands.UpdateUserCommand;
using Account.Application.Features.User.Queries.GetUserByIDQuery;
using Account.Application.Features.User.Queries.GetUsersQuery;
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
        }
    }
}
