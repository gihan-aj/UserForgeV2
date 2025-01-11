using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Permissions.Commands.Delete
{
    public class DeletePermissionsCommandValidator : AbstractValidator<DeletePermissionsCommand>
    {
        public DeletePermissionsCommandValidator()
        {
            RuleFor(x => x.Ids)
                .Must(x => x.Count > 0).WithMessage("Permission Ids are required.");

            RuleFor(x => x.ModifiedBy)
                .NotEmpty().WithMessage("Modifiying user id is required.");
        }
    }
}
