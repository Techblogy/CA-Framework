namespace CAF.Core.Helper
{
    public interface ILogger
    {
        void Error(System.Exception ex);
        void Error(string appName, string message, string log);
    }
}
