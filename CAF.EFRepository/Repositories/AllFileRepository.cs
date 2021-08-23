using CAF.Core.Entities;
using CAF.Core.Exception;
using CAF.Core.Repository;
using CAF.EFRepository.Repositories.Base;

using Microsoft.AspNetCore.Http;

using System;
using System.Collections.Generic;
using System.Text;

namespace CAF.EFRepository.Repositories
{
    public class AllFileRepository : DbStateRepository<AllFile, Guid>, IAllFileRepository
    {
        public AllFileRepository(EFDatabaseContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
        {
        }

        public override AllFile FindOrThrow(Guid id, string message = "")
        {
            var file = base.Find(id);

            if (file == null)
                throw new NotFoundExcepiton(string.IsNullOrEmpty(message) ? "Dosya bulunamadı." : message, Core.Enums.ErrorType.Warning);

            return file;
        }
    }
}
