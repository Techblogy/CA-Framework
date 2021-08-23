using CAF.Core.Service;
using CAF.Service.Services.Base;
using Microsoft.AspNetCore.Http;

using Azure.Storage.Blobs;
using System.IO;
using CAF.Core.ViewModel.BlobStorage.Response;
using CAF.Core.ViewModel.BlobStorage.Request;
using CAF.Core.Utilities;
using CAF.Core.Exception;
using CAF.Core.Enums;
using System;

namespace CAF.Service.Services
{
    public class AzureBlobStorageService : BaseService, IStorageService
    {
        BlobContainerClient container;
        public AzureBlobStorageService(IHttpContextAccessor httpContextAccessor, IAppSettings appSettings,IServiceProvider serviceProvider) : base(httpContextAccessor, serviceProvider)
        {
            if (string.IsNullOrEmpty(appSettings.AzureBlobStorageConneconString) || string.IsNullOrEmpty(appSettings.AzureBlobStorageContainerName))
                return;

            container = new BlobContainerClient(appSettings.AzureBlobStorageConneconString, appSettings.AzureBlobStorageContainerName);
            container.CreateIfNotExists();
        }

        public void Delete(string filePath)
        {
            if (container == null) throw new BadRequestException("Azure Blob Storage ayarları yapılmamıştır.");

            var blob = container.GetBlobClient(filePath);
            blob.DeleteIfExists();
        }

        public byte[] DownloadFile(string path)
        {
            if (container == null) throw new BadRequestException("Azure Blob Storage ayarları yapılmamıştır.");

            var blob = container.GetBlobClient(path);
            var response = blob.Download();

            using (var memoryStream = new MemoryStream())
            {
                response.Value.Content.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        public StorageUploadResponse FileUpload(StorageUploadBytesRequest request)
        {
            if (container == null) throw new BadRequestException("Azure Blob Storage ayarları yapılmamıştır.");

            container.SetAccessPolicy(Azure.Storage.Blobs.Models.PublicAccessType.Blob);

            var blobName = Path.Combine(request.FolderName, request.FileName);
            BlobClient blob = container.GetBlobClient(blobName);

            if (request.ExistFileAction == ExistFileAction.None && blob.Exists().Value == true)
                throw new BadRequestException("Aynı dosyadan bulunmaktadır. Sadece dosyayı ezerek veya yeniden isimlendireme seçeneği ile yüklenebilir");

            if (blob.Exists().Value == true && request.ExistFileAction == ExistFileAction.Rename)
            {
                var newName = GetFileNewName(request.FolderName, request.FileName);
                blobName = Path.Combine(request.FolderName, newName);
                blob = container.GetBlobClient(blobName);
            }
            Stream stream = new MemoryStream(request.FileBytes);
            blob.Upload(stream, request.ExistFileAction == ExistFileAction.Overwrite);

            return new StorageUploadResponse(blob.Uri.AbsoluteUri, request.FileName, blobName);
        }

        public string GetFileNewName(string folder, string fileName, int maxTryPrefix = 20)
        {
            if (container == null) throw new BadRequestException("Azure Blob Storage ayarları yapılmamıştır.");

            var existCount = 0;
            var extension = Path.GetExtension(fileName);
            while (existCount <= maxTryPrefix + 1)
            {
                existCount++;
                var blobName = Path.Combine(folder, fileName);
                BlobClient blob = container.GetBlobClient(blobName);
                if (blob.Exists().Value == true)
                {
                    fileName = Path.GetFileNameWithoutExtension(fileName) + "(1)" + extension;
                }
                else
                {
                    return fileName;
                }
            }

            throw new BadRequestException($"{maxTryPrefix} kere yeni isimlendirme denenmiştir. Hala aynı isme ait dosya bulunmaktadır. Lütfen yeni dosya ile yükleme yapınız.");
        }

        public string GetFileUrl(string path)
        {
            if (container == null) throw new BadRequestException("Azure Blob Storage ayarları yapılmamıştır.");

            var blob = container.GetBlobClient(path);
            return blob.Uri.AbsoluteUri;
        }
    }
}
