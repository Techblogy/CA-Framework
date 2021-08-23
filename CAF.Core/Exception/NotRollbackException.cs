namespace CAF.Core.Exception
{
    public class NotRollbackException : BaseExcepiton
    {
        public NotRollbackException(string message) : base(message, string.Empty)
        {

        }
        public NotRollbackException(string message, string detail) : base(message, detail, Enums.ErrorType.Warning)
        {
        }
    }
}
