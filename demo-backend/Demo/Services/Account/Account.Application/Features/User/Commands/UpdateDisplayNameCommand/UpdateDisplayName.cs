using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Service.Lib.BaseResponse;

namespace Account.Application.Features.User.Commands.UpdateDisplayNameCommand
{
    public class UpdateDisplayName : IRequest<ApiResponse<Guid>>
    {
        public Guid ID { get; set; }
        public string DisplayName { get; set; }
        public string PasswordHash { get; set; }

        public UpdateDisplayName(Guid id, string displayName, string passwordHash)
        {
            ID = id;
            DisplayName = displayName;
            PasswordHash = passwordHash;
        }
    }
}
