using CAF.Core.Enums;

namespace CAF.Core.Exception
{
    public class BadRequestException : BaseExcepiton
    {
        public BadRequestException(string message) : base(message, string.Empty)
        {

        }
        public BadRequestException(string message, string detail) : base(message, detail)
        {
        }
        public BadRequestException(string message, string detail, ErrorType type) : base(message, detail, type)
        {
        }
    }
}
