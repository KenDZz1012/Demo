using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Account.Domain.Common.Constants;
using FluentValidation;

namespace Account.Application.Features.UserRelationship.Commands.UpdateStatusCommand
{
    public class UpdateStatusValidator : AbstractValidator<UpdateStatus>
    {
        public UpdateStatusValidator()
        {
            RuleFor(x => x.ID)
                .NotEmpty().WithMessage("ID không được để trống");
            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Trạng thái không được để trống")
                .Must(x => x == UserRelationshipStatus.Accepted || x == UserRelationshipStatus.Blocked).WithMessage("Trạng thái không hợp lệ");
        }
    }
}
