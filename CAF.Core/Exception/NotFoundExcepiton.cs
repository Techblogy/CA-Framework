using CAF.Core.Enums;

namespace CAF.Core.Exception
{
    public class NotFoundExcepiton : BaseExcepiton
    {
        public NotFoundExcepiton(string message) : base(message, string.Empty)
        {

        }
        public NotFoundExcepiton(string message, string detail) : base(message, detail)
        {
        }
        public NotFoundExcepiton(string message, string detail, ErrorType type) : base(message, detail, type)
        {
        }
        public NotFoundExcepiton(string message, ErrorType type) : base(message, string.Empty, type)
        {
        }
    }
}
