using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Service.Lib.BaseResponse;
using Microsoft.AspNetCore.Http;

namespace Account.Application.Features.User.Commands.UpdateAvatarCommand
{
    public class UpdateAvatar : IRequest<ApiResponse<Guid>>
    {
        public Guid ID { get; set; }
        public IFormFile File { get; set; }

        public UpdateAvatar(Guid id, IFormFile file)
        {
            ID = id;
            File = file;
        }
    }
}
