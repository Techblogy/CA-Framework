using CAF.Core.Enums;

namespace CAF.Core.Exception
{
    public class BaseExcepiton : System.Exception
    {
        public override string Message { get; }
        public string Detail { get; }
        public ErrorType Type { get; }
        public BaseExcepiton(string message) : this(message, string.Empty)
        {

        }
        public BaseExcepiton(string message, string detail)
        {
            Message = message;
            Detail = detail;
        }
        public BaseExcepiton(string message, string detail, ErrorType type)
        {
            Message = message;
            Detail = detail;
            Type = type;
        }
    }
}
