using CAF.Core.Entities;
using CAF.Core.Exception;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CAF.EFRepository.Repositories.Base
{
    public class EFRepositoryInMemory<T> : EFRepositoryBase<T, Guid> where T : UniqueEntity<Guid>

    {
        private static IMemoryCache _memCache;
        private readonly string cacheKey = $"EFRepositoryInMemory.Entity:{typeof(T).Name}";
        public EFRepositoryInMemory(EFDatabaseContext dbContext, IHttpContextAccessor httpContextAccessor, IMemoryCache memCache) : base(dbContext, httpContextAccessor)
        {
            _memCache = memCache;
            _memCache.Set<List<T>>(cacheKey, null);
        }
        private List<T> CacheData
        {
            get
            {
                var data = _memCache.Get<List<T>>(cacheKey);
                if (data == null)
                {
                    var entities = base.All().AsNoTracking().ToList();
                    _memCache.Set<List<T>>(cacheKey, entities);
                    return entities;
                }
                return data;
            }
        }
        private void reloadCacheAsync()
        {
            var entities = base.All().AsNoTracking().ToList();
            _memCache.Set(cacheKey, entities);
        }
        public override Guid Add(T entity)
        {
            var result = base.Add(entity);
            reloadCacheAsync();
            return result;
        }

        public override List<Guid> Add(List<T> entities)
        {
            var result = base.Add(entities);
            reloadCacheAsync();
            return result;
        }

        public override IQueryable<T> All()
        {
            return CacheData.AsQueryable();
        }

        public override IQueryable<T> All(params string[] columns)
        {
            return CacheData.AsQueryable();
        }

        public override bool Any(Expression<Func<T, bool>> where)
        {
            return CacheData.Any(where.Compile());
        }

        public override void Delete(T entity)
        {
            base.Delete(entity);
            reloadCacheAsync();
        }

        public override void Delete(List<T> entities)
        {
            base.Delete(entities);
            reloadCacheAsync();
        }

        public override T Find(Guid id)
        {
            return CacheData.Find(x => x.Id == id);
        }

        public override T FindAndIncludes(Guid id)
        {
            return CacheData.Find(x => x.Id == id);
        }

        public override T FindAndIncludes(Guid id, params string[] columns)
        {
            return CacheData.Find(x => x.Id == id);
        }

        public override T FindOrThrow(Guid id, string message = "")
        {
            var data = this.Find(id);

            if (data == null)
                throw new NotFoundExcepiton(string.IsNullOrEmpty(message) ? $"{typeof(T).Name} bulunamadı." : message, Core.Enums.ErrorType.Warning);

            return data;
        }
        [Obsolete("Inmemory cache de include metodlar desteklenmez", true)]
        public override T FindOrThrow(Guid id, string message = "", params string[] columns)
        {
            throw new NotSupportedException();
        }

        public override T Get(Expression<Func<T, bool>> where)
        {
            return CacheData.FirstOrDefault(where.Compile());
        }
        [Obsolete("Inmemory cache de include metodlar desteklenmez", true)]
        public override T Get(Expression<Func<T, bool>> where, params string[] columns)
        {
            throw new NotSupportedException();
        }

        public override T GetOrThrow(Expression<Func<T, bool>> where)
        {
            var entity = Get(where);
            if (entity == null)
                throw new NotFoundExcepiton("Veri bulunamadı.", Core.Enums.ErrorType.Warning);
            else
                return entity;
        }

        public override IQueryable<T> OrderBy<TKey>(Expression<Func<T, TKey>> orderBy, bool isDesc)
        {

            if (isDesc)
                return CacheData.OrderByDescending(orderBy.Compile()).AsQueryable();
            return CacheData.OrderBy(orderBy.Compile()).AsQueryable();
        }

        public override void Update(T entity)
        {
            base.Update(entity);
            reloadCacheAsync();
        }

        public override void Update(List<T> entities)
        {
            base.Update(entities);
            reloadCacheAsync();
        }

        public override IQueryable<T> Where(Expression<Func<T, bool>> where)
        {
            return CacheData.Where(where.Compile()).AsQueryable();
        }
    }
}
