using CAF.Core.ViewModel.User.Request;

using FluentValidation;

namespace CAF.Core.Validation.User
{
    public class GetUserGridRequestValidation : AbstractValidator<GetUserGridRequest>
    {
        public GetUserGridRequestValidation()
        {
        }
    }
}
