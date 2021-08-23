using CAF.Core.Constant;
using CAF.Core.Enums;
using CAF.Core.Repository;
using CAF.Core.Service;
using CAF.Core.ViewModel.BlobStorage.Request;
using CAF.Service.Services.Base;

using Microsoft.AspNetCore.Http;

using System;
using System.Collections.Generic;
using System.Text;

namespace CAF.Service.Services
{
    public class AllFileService : BaseService, IAllFileService
    {
        private readonly IAllFileRepository allFileRepository;
        public AllFileService(IHttpContextAccessor httpContextAccessor,IServiceProvider serviceProvider) : base(httpContextAccessor, serviceProvider)
        {
            allFileRepository = ResolveService<IAllFileRepository>();
        }

        public void AddFile(Guid relationId, byte[] file, string fileName, FileCategory category)
        {
            var storageService = base.ResolveService<IStorageService>();

            //Dokümanın uplaod işlemleri
            var uploadRequest = new StorageUploadBytesRequest()
            {
                FileBytes = file,
                FileName = fileName,
                FolderName = string.Format(StorageFolderConstant.AllFile, relationId, (int)category),
                ExistFileAction = Core.Enums.ExistFileAction.Overwrite
            };
            var response = storageService.FileUpload(uploadRequest);

            allFileRepository.Add(new Core.Entities.AllFile()
            {
                FileCategory = category,
                FileName = fileName,
                FilePath = response.Path,
                FileUrl = response.FileUrl,
                RelationId = relationId
            });
        }
    }
}
