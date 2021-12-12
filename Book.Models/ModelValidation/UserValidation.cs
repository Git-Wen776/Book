using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Models.ModelValidation
{
    public class UserValidation: AbstractValidator<User>
    {
        public UserValidation()
        {
            RuleFor(p=>p.Name).NotEmpty();
            RuleFor(p=>p.Email).EmailAddress().NotEmpty();
            RuleFor(p => p.Password).NotEmpty().MaximumLength(10);
            RuleFor(p=>p.Account).NotEmpty();
            RuleFor(p => p.Birtime).Must(p=>p<=DateTime.Now);
            RuleFor(p => p.Phone).SetValidator(new RegexValidaProperty<User>(""));
        }
    }
}
