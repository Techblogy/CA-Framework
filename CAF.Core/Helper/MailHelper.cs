using CAF.Core.Helper.Interface;
using CAF.Core.Utilities;
using CAF.Core.ViewModel.Mail;

using MimeKit;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CAF.Core.Helper
{
    public class MailHelper : IMailHelper
    {
        /*
         [[TO_USER_APPEAL]] HİTAP

        [[MAIL_BUTTON]]
        
         
         [[MAIL_CONTENT]]
         
         [[WEBSITE_MAIN_PAGE]]
         */
        public IAppSettings AppSettings { get; set; }
        public MailHelper(IAppSettings appSettings)
        {
            AppSettings = appSettings;
        }
        public void SendMail(SendMailModel model)
        {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress(AppSettings.MailDisplayName, AppSettings.MailUser));
            model.To.ForEach(x => message.To.Add(new MailboxAddress(x, x)));
            model.Cc.ForEach(x => message.Cc.Add(new MailboxAddress(x, x)));

            message.Subject = model.Subject; //Replace(model.Subject); //TODO: replace

            var mailBody = GetMailTemplate();
            mailBody = mailBody.Replace("[[MAIL_CONTENT]]", model.HtmlContent);
            mailBody = mailBody.Replace("[[TO_USER_APPEAL]]", model.AppealUserName);

            if (!string.IsNullOrEmpty(model.ButtonLink) && !string.IsNullOrEmpty(model.ButtonName))
            {
                mailBody = mailBody.Replace("[[MAIL_BUTTON]]", GeMailButton(model.ButtonLink, model.ButtonName));
            }
            else mailBody = mailBody.Replace("[[MAIL_BUTTON]]", "");

            if (model.Attachments.Any())
            {
                var multipart = new Multipart("mixed");
                var body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = @mailBody };
                multipart.Add(body);

                foreach (var attch in model.Attachments)
                {
                    var mimeTypes = MimeTypes.GetMimeType(attch.FileName).Split('/');

                    var attachment = new MimePart(mimeTypes[0], mimeTypes[1])
                    {
                        Content = new MimeContent(new MemoryStream(attch.FileBytes)),
                        ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                        ContentTransferEncoding = ContentEncoding.Base64,
                        FileName = Path.GetFileName(attch.FileName)
                    };

                    multipart.Add(attachment);
                }


                message.Body = multipart;
            }
            else
            {
                message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = mailBody
                };
            }

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                client.Connect(AppSettings.MailSmtp, AppSettings.MailPort, AppSettings.MailEnableSSL);

                client.Authenticate(AppSettings.MailUser, AppSettings.MailPassword);

                client.Send(message);

                client.Disconnect(true);
            }
        }

        private string GetMailTemplate()
        {
            var assembly = typeof(CAF.Core.Helper.MailHelper).GetTypeInfo().Assembly;
            Stream resource = assembly.GetManifestResourceStream("CAF.Core.Constant.MailTemplates.MailTemplate.html");
            if (resource != null)
            {
                using (StreamReader reader = new StreamReader(resource))
                {
                    string mailTemplate = reader.ReadToEnd();

                    mailTemplate = mailTemplate.Replace("[[WEBSITE_MAIN_PAGE]]", AppSettings.UIWebSiteUrl);
                    mailTemplate = mailTemplate.Replace("[[WEBSITE_APP_NAME]]", AppSettings.UIWebSiteUrl);

                    return mailTemplate;
                }
            }
            else return string.Empty;

        }

        public string GeMailButton(string prefixUrl, string buttonName)
        {
            var url = AppSettings.UIWebSiteUrl.TrimEnd('/') + "/" + prefixUrl.TrimStart('/');

            return $"<a href=\"{url}\" target=\"_blank\">{buttonName}</a>";
        }
    }
}
