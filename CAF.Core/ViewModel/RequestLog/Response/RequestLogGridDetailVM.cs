using System;

namespace CAF.Core.ViewModel.RequestLog.Response
{
    public class RequestLogGridDetailVM
    {
        public Guid Id { get; set; }
        public string RequestUrl { get; set; }
        public string Ip { get; set; }
        public string RequestBody { get; set; }
        public string RequestHeaders { get; set; }
        public string ResponseHeaders { get; set; }
        public DateTime Date { get; set; }

        public RequestLogGridDetailVM()
        {

        }
        public RequestLogGridDetailVM(Entities.RequestLog entity)
        {

            Id = entity.Id;
            RequestUrl = entity.RequestUrl;
            Ip = entity.Ip;
            RequestBody = entity.RequestBody;
            RequestHeaders = entity.RequestHeaders;
            ResponseHeaders = entity.ResponseHeaders;
            Date = entity.CreateDate;
        }
    }
}
