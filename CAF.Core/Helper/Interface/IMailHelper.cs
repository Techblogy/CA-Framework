using CAF.Core.ViewModel.Mail;

namespace CAF.Core.Helper.Interface
{
    public interface IMailHelper
    {
        void SendMail(SendMailModel model);
        string GeMailButton(string url, string buttonName);
    }
}
