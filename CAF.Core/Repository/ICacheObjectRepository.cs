using CAF.Core.Entities;
using CAF.Core.Repository.Base;

using System;
using System.Collections.Generic;
using System.Text;

namespace CAF.Core.Repository
{
    public interface ICacheObjectRepository : IRepository<CacheObject, string>
    {
        void ClearAllCache();
        void RemoveCache(string clearId);
    }
}
