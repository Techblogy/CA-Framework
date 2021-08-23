using CAF.Core.Extensions;
using CAF.Core.ViewModel.User.Request;

using FluentValidation;

using System;
using System.Collections.Generic;
using System.Text;

namespace CAF.Core.Validation.User
{
    public class LoginUserRequestValidation : AbstractValidator<LoginUserRequest>
    {
        public LoginUserRequestValidation()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("E-Posta gerekli.");
            When(x => !x.Email.ValidateTCKNO(), () =>
            {
                RuleFor(x => x.Password).NotEmpty().WithMessage("Şifre gerekli.");
            });

        }
    }
}
