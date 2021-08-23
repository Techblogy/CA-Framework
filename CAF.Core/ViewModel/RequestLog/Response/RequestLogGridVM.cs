using System;

namespace CAF.Core.ViewModel.RequestLog.Response
{
    public class RequestLogGridVM
    {
        public Guid Id { get; set; }
        public string RequestUrl { get; set; }
        public string Ip { get; set; }
        public DateTime Date { get; set; }
    }
}
