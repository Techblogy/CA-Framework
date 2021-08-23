using System;
using System.Collections.Generic;
using System.Text;

namespace CAF.Core.ViewModel.BlobStorage.Response
{
    public class StorageUploadResponse
    {
        public string FileUrl { get; set; }
        public string Path { get; set; }
        public string FileName { get; set; }

        public StorageUploadResponse(string fileUrl, string fileName, string path)
        {
            FileName = fileName;
            FileUrl = fileUrl;
            Path = path;
        }
    }
}
