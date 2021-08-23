using CAF.Core.ViewModel.CacheObject.Request;

using System;
using System.Collections.Generic;
using System.Text;

namespace CAF.Core.Service
{
    public interface IOutputCacheObjectService
    {
        void AddCache(AddCacheObjectRequest request);
        void RemoveCache(string clearId, string id);
        void RemoveCache(string clearId);
        bool ExistCache(string clearId, string id);
        object GetCache(string clearId, string id);
    }
}
