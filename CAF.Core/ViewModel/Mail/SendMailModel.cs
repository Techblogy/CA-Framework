using System;
using System.Collections.Generic;
using System.Text;

namespace CAF.Core.ViewModel.Mail
{
    public class SendMailModel
    {
        /// <summary>
        /// Hitap bölümü
        /// </summary>
        public string AppealUserName { get; set; }
        /// <summary>
        /// Mail konusu
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// html mail içeriği
        /// </summary>
        public string HtmlContent { get; set; }
        public List<string> To { get; set; } = new List<string>();
        public List<string> Cc { get; set; } = new List<string>();
        public List<AttachmentModel> Attachments { get; set; } = new List<AttachmentModel>();
        /// <summary>
        /// Buton linki
        /// </summary>
        public string ButtonLink { get; set; }
        /// <summary>
        /// Butonun görünen adı
        /// </summary>
        public string ButtonName { get; set; }
    }
    public class AttachmentModel
    {
        public string FileName { get; set; }
        public byte[] FileBytes { get; set; }
        public AttachmentModel(string fileName, byte[] fileBytes)
        {
            FileName = fileName;
            FileBytes = fileBytes;
        }
    }
}
