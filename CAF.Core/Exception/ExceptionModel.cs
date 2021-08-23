using CAF.Core.Enums;

namespace CAF.Core.Exception
{
    public class ExceptionModel
    {
        public string Message { get; set; }
        public string Detail { get; set; }
        public ErrorType Type { get; set; }
    }
}
