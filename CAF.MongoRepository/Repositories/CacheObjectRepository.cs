using CAF.Core.Entities;
using CAF.Core.Repository;
using CAF.MongoRepository.Core;

using Microsoft.AspNetCore.Http;

using MongoDB.Driver;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAF.MongoRepository.Repositories
{
    public class CacheObjectRepository : GenericMongoRepositoryBase<CacheObject, string>, ICacheObjectRepository
    {
        public CacheObjectRepository(IMongoDBContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }

        public void ClearAllCache()
        {
            base._mongoContext._db.DropCollection(base._dbCollection.CollectionNamespace.CollectionName);
        }

        public void RemoveCache(string clearId)
        {
            var entities = base.Where(x => x.ClearId == clearId).ToList();
            base.Delete(entities);
        }
    }
}
