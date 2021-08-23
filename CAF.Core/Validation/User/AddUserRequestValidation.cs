using CAF.Core.ViewModel.User.Request;

using FluentValidation;

namespace CAF.Core.Validation.User
{
    public class AddUserRequestValidation : AbstractValidator<AddUserRequest>
    {
        public AddUserRequestValidation()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("Adı gerekli.");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Soyadı gerekli.");
            RuleFor(x => x.Email).NotEmpty().WithMessage("E-Posta gerekli.");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Geçerli bir email giriniz");
            RuleFor(x => x.Password).MaximumLength(100).WithMessage("Şifre 100 karakterden fazla olamaz");
            RuleFor(x => x.RoleType).IsInEnum().WithMessage("Kullanıcı rolü gerekli.");
        }
    }
}
