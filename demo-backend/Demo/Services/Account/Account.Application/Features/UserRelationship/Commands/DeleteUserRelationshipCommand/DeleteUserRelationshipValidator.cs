using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Account.Application.Features.UserRelationship.Commands.DeleteUserRelationshipCommand
{
    public class DeleteUserRelationshipValidator : AbstractValidator<DeleteUserRelationship>
    {
        public DeleteUserRelationshipValidator()
        {
            RuleFor(x => x.UserID)
                .NotEmpty().WithMessage("UserID không được để trống");
            RuleFor(x => x.FriendID)
                .NotEmpty().WithMessage("FriendID không được để trống");
        }
    }
}
