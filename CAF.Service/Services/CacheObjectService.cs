using CAF.Core.Repository;
using CAF.Core.Service;
using CAF.Core.ViewModel.CacheObject.Request;
using CAF.Service.Services.Base;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

using System;
using System.Collections.Generic;
using System.Text;

namespace CAF.Service.Services
{
    public class OutputCacheObjectService : BaseService, IOutputCacheObjectService
    {
        private readonly IMemoryCache _memCache;
        public OutputCacheObjectService(IHttpContextAccessor httpContextAccessor, IMemoryCache memCache,IServiceProvider serviceProvider) : base(httpContextAccessor, serviceProvider)
        {
            _memCache = memCache;
        }

        public void AddCache(AddCacheObjectRequest request)
        {
            var cacheDic = (Dictionary<string, Core.Entities.CacheObject>)this._memCache.Get(request.ClearId);
            if (cacheDic == null)
                cacheDic = new Dictionary<string, Core.Entities.CacheObject>();

            var entity = new Core.Entities.CacheObject()
            {
                CacheRegisterDate = Core.Helper.UtilitiesHelper.DateTimeNow(),
                Id = request.Id,
                ObjectTypeFullName = request.Type.FullName,
                OutputResponse = request.Value,
                ClearId = request.ClearId
            };

            if (cacheDic.ContainsKey(request.Id))
                cacheDic[request.Id] = entity;
            else
                cacheDic.Add(request.Id, entity);

            var cacheExpOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddHours(request.ExpiryHours),
                Priority = CacheItemPriority.Normal
            };
            _memCache.Set(request.ClearId, cacheDic, cacheExpOptions);
        }

        public bool ExistCache(string clearId, string id)
        {
            var cacheDic = (Dictionary<string, Core.Entities.CacheObject>)this._memCache.Get(clearId);
            if (cacheDic == null) return false;

            return cacheDic.ContainsKey(id) ? true : false;
        }

        public object GetCache(string clearId, string id)
        {
            var cacheDic = (Dictionary<string, Core.Entities.CacheObject>)this._memCache.Get(clearId);
            if (cacheDic == null) return null;

            return cacheDic.ContainsKey(id) ? cacheDic[id]?.OutputResponse : null;
        }

        public void RemoveCache(string clearId, string id)
        {
            var cacheDic = (Dictionary<string, Core.Entities.CacheObject>)this._memCache.Get(clearId);
            if (cacheDic != null && cacheDic.ContainsKey(id))
            {
                cacheDic.Remove(id);
            }

        }
        public void RemoveCache(string clearId)
        {
            this._memCache.Remove(clearId);
        }
    }
}
