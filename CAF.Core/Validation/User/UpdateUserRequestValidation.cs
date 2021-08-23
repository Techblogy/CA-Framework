using CAF.Core.ViewModel.User.Request;

using FluentValidation;

namespace CAF.Core.Validation.User
{
    public class UpdateUserRequestValidation : AbstractValidator<UpdateUserRequest>
    {
        public UpdateUserRequestValidation()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("Adı gerekli.");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Soyadı gerekli.");
            RuleFor(x => x.Email).NotEmpty().WithMessage("E-Posta gerekli.");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Geçerli bir email giriniz");
            RuleFor(x => x.Password).MaximumLength(100).WithMessage("Şifre 100 karakterden fazla olamaz");
            RuleFor(x => x.RoleType).IsInEnum().WithMessage("Kullanıcı rolü gerekli.");
            RuleFor(x => x.DbState).IsInEnum().WithMessage("Kullanıcı durumu gerekli.");
        }
    }
}
