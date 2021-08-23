using CAF.Core.Entities;
using CAF.Core.Repository;
using CAF.MongoRepository.Core;

using Microsoft.AspNetCore.Http;

using System;
using System.Collections.Generic;
using System.Text;

namespace CAF.MongoRepository.Repositories
{
    public class RequestLogRepository : MongoRepositoryBase<RequestLog>, IRequestLogRepository
    {
        public RequestLogRepository(IMongoDBContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }
    }
}
