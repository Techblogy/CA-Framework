using CAF.Core.ViewModel.Common.Request;

using FluentValidation;

namespace CAF.Core.Validation.Common
{
    public class SearchValidation : AbstractValidator<BaseSearchRequest>
    {
        public SearchValidation()
        {
            //RuleFor(x => x.SearchText).NotNull().WithMessage("Arama metni gereklidir.");
        }
    }
}
