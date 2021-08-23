using CAF.Core.Extensions;

using System;
using System.Collections.Generic;
using System.Text;

namespace CAF.Core.ViewModel.Common.Request
{
    public class UploadFileRequest
    {
        /// <summary>
        /// Dosyanın base64 string hali
        /// </summary>
        public string FileBase64
        {
            get { return fileBase64; }
            set { fileBase64 = value?.RemovePrefixBase64(); }
        }
        /// <summary>
        /// Dosya adı
        /// </summary>
        public string FileName { get; set; }

        private string fileBase64;
        public byte[] GetBytes()
        {
            if (string.IsNullOrEmpty(FileBase64))
                return Convert.FromBase64String(FileBase64);
            else
                return default;
        }
    }
}
