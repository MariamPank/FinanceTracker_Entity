using FinanceTracker_Entity.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceTracker_Entity.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(e => e.UserName).NotEmpty().MinimumLength(4);
            RuleFor(e => e.Email).NotEmpty().EmailAddress();
            RuleFor(e => e.Password).NotEmpty().MinimumLength(8);
        }
    }
}
