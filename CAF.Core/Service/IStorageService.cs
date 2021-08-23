using CAF.Core.ViewModel.BlobStorage.Request;
using CAF.Core.ViewModel.BlobStorage.Response;

using System;
using System.Collections.Generic;
using System.Text;

namespace CAF.Core.Service
{
    public interface IStorageService
    {
        StorageUploadResponse FileUpload(StorageUploadBytesRequest request);
        string GetFileUrl(string path);
        void Delete(string filePath);
        string GetFileNewName(string folder, string fileName, int maxTryPrefix = 20);
        byte[] DownloadFile(string path);
    }
}
