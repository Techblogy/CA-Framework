using CAF.Core.ViewModel.User.Request;

using FluentValidation;

namespace CAF.Core.Validation.User
{
    public class ForgetPasswordRequestValidation : AbstractValidator<ForgetPasswordRequest>
    {
        public ForgetPasswordRequestValidation()
        {
            RuleFor(x => x).Must(x => !string.IsNullOrEmpty(x.Email) || !string.IsNullOrEmpty(x.Tckn)).WithMessage("E-Posta gereklidir.");
        }
    }
}
