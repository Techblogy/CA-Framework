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
    public class AccessTokenRepository : DbStateRepository<AccessToken, Guid>, IAccessTokenRepository
    {
        public AccessTokenRepository(EFDatabaseContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
        {
        }

        public override AccessToken FindOrThrow(Guid id, string message = "")
        {
            var accessToken = base.Find(id);

            if (accessToken == null)
                throw new NotFoundExcepiton(string.IsNullOrEmpty(message) ? "Access Token bulunamadı." : message, Core.Enums.ErrorType.Warning);

            return accessToken;
        }
    }
}
