using System;
using System.Collections.Generic;
using System.Text;

namespace CAF.Core.ViewModel.BlobStorage.Request
{
    public class StorageUploadBytesRequest : BaseStorageUploadRequest
    {
        /// <summary>
        /// Yüklenecek dosya bytes
        /// </summary>
        public byte[] FileBytes { get; set; }
    }
}
