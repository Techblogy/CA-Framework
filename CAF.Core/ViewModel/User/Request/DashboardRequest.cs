using CAF.Core.Enums;

using System;

namespace CAF.Core.ViewModel.User.Request
{
    public class DashboardRequest
    {
        public DashboardRequestType Type { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
