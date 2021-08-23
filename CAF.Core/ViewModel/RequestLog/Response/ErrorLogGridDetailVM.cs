using System;

namespace CAF.Core.ViewModel.RequestLog.Response
{
    public class ErrorLogGridDetailVM
    {
        public Guid Id { get; set; }
        public DateTime timestamp { get; set; }
        public string level { get; set; }
        public string stacktrace { get; set; }
        public string message { get; set; }

        public ErrorLogGridDetailVM()
        {

        }
        public ErrorLogGridDetailVM(Entities.ErrorLog entity)
        {
            Id = entity.Id;
            timestamp = entity.timestamp;
            level = entity.level;
            stacktrace = entity.stacktrace;
            message = entity.message;
        }
    }
}
