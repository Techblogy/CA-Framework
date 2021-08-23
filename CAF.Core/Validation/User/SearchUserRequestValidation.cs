using CAF.Core.Validation.Common;
using CAF.Core.ViewModel.User.Request;

using FluentValidation;

namespace CAF.Core.Validation.User
{
    public class SearchUserRequestValidation : AbstractValidator<SearchUserRequest>
    {
        public SearchUserRequestValidation()
        {
            RuleFor(x => x).SetValidator(new SearchValidation());
        }
    }
}
