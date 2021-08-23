namespace CAF.Core.ViewModel.RequestLog
{
    public class AddRequestLogRequest
    {
        public string RequestUrl { get; set; }
        public string Ip { get; set; }
        public string RequestBody { get; set; }
        public object ResponseBody { get; set; }
        public string RequestHeaders { get; set; }
        public string ResponseHeaders { get; set; }

        public Entities.RequestLog ToEntity()
        {
            return new Entities.RequestLog()
            {
                RequestUrl = RequestUrl,
                Ip = Ip,
                RequestBody = RequestBody,
                ResponseBody = ResponseBody,
                RequestHeaders = RequestHeaders,
                ResponseHeaders = ResponseHeaders,
            };
        }
    }
}
