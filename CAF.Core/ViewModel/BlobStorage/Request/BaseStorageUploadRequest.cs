using CAF.Core.Enums;

using System;
using System.Collections.Generic;
using System.Text;

namespace CAF.Core.ViewModel.BlobStorage.Request
{
    public class StorageUploadRequest : BaseStorageUploadRequest
    {
        /// <summary>
        /// Yüklenecek dosya
        /// </summary>
        public string Base64String { get; set; }

        public StorageUploadBytesRequest ToBytes()
        {
            return new StorageUploadBytesRequest()
            {
                FileBytes = System.Convert.FromBase64String(Base64String),
                FileName = base.FileName,
                FolderName = base.FolderName,
                ExistFileAction = base.ExistFileAction
            };
        }
    }
    public class BaseStorageUploadRequest
    {
        /// <summary>
        /// Dosyanın yükleneceği dizin
        /// </summary>
        public string FolderName { get; set; }
        /// <summary>
        /// Dosyanın adı
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// Aynı dosyadan varsa üzerine yazsın mı
        /// </summary>
        public ExistFileAction ExistFileAction { get; set; }
    }
}
