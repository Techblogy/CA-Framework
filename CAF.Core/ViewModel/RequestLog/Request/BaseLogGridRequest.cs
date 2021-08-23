using CAF.Core.Enums;
using CAF.Core.Enums.SystemModule;

namespace CAF.Core.ViewModel.RequestLog.Request
{
    public class BaseLogGridRequest
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public OrderbyType OrderbyType { get; set; }
        public string SearchText { get; set; }
    }
}
