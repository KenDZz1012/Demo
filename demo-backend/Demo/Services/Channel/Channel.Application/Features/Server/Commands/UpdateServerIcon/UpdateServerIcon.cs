using MediatR;
using Microsoft.AspNetCore.Http;
using Service.Lib.BaseResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Channel.Application.Features.Server.Commands.UpdateServerIcon
{
    public class UpdateServerIcon : IRequest<ApiResponse<string>>
    {
        public IFormFile IconUrl { get; set; }
        public UpdateServerIcon() { } 
        public UpdateServerIcon(IFormFile iconUrl)
        {
            IconUrl = iconUrl;
        }
    }
}
