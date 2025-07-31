using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Authorize.Application.Features.RefreshToken.Commands.RefreshTokenCommand;
using Authorize.Application.Models;
using AutoMapper;

namespace Authorize.Application.Mappings
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<RefreshTokenFilter, RefreshToken>().ReverseMap();
        }
    }
}
