using CAF.Core.ViewModel.User.Request;

using FluentValidation;

namespace CAF.Core.Validation.User
{
    public class NewPasswordByResetCodeRequestValidation : AbstractValidator<NewPasswordByResetCodeRequest>
    {
        public NewPasswordByResetCodeRequestValidation()
        {
            RuleFor(x => x.ResetCode).NotEmpty().WithMessage("Şifre sıfırlama kodu gereklidir.");
            RuleFor(x => x.NewPassword).NotEmpty().WithMessage("Yeni şifre gereklidir.");
            RuleFor(x => x.NewPassword).MaximumLength(100).WithMessage("Şifre 100 karakterden fazla olamaz.");
            RuleFor(x => x.ConfirmNewPassword).NotEmpty().WithMessage("Şifre doğrulama gereklidir.");
            RuleFor(x => x).Must(x => x.NewPassword == x.ConfirmNewPassword).WithMessage("Yeni şifre ve doğrulaması eşleşmemektedir.");
        }
    }
}
