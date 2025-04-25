using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Account.Domain.Entities;
using Account.Application.DTOs.User;
using Account.Application.DTOs.UserRelationship;

namespace Account.Infrastructure.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDTO>();

            CreateMap<UserRelationship, UserRelationshipDTO>()
                .ForMember(dest => dest.RequesterName, opt => opt.MapFrom(src => src.Requester.UserName))
                .ForMember(dest => dest.AddresseeName, opt => opt.MapFrom(src => src.Addressee.UserName));

        }
    }
}
