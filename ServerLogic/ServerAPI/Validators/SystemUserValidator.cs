using FluentValidation;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerAPI.Validators
{
    public class SystemUserValidator : AbstractValidator<SystemUser>
    {
        public SystemUserValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .NotNull();
        }
    }
}
