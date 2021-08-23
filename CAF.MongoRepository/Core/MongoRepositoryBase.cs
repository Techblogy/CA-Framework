using CAF.Core.Entities;
using CAF.Core.Exception;
using CAF.MongoRepository.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;

using MongoDB.Driver;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using CAF.Core.Repository.Base;

namespace CAF.MongoRepository.Repositories
{
    public class MongoRepositoryBase<T> : IRepository<T, Guid>
        where T : GuidEntitiy
    {
        protected readonly IMongoDBContext _mongoContext;
        protected IMongoCollection<T> _dbCollection;
        private readonly IHttpContextAccessor HttpContextAccessor;

        protected MongoRepositoryBase(IMongoDBContext context, IHttpContextAccessor httpContextAccessor)
        {
            _mongoContext = context;
            HttpContextAccessor = httpContextAccessor;
            _dbCollection = _mongoContext.GetCollection<T>(typeof(T).Name);
        }

        public Guid Add(T entity)
        {
            _dbCollection.InsertOne(entity);
            return entity.Id;
        }

        public List<Guid> Add(List<T> entities)
        {
            _dbCollection.InsertMany(entities);
            return entities.Select(x => x.Id).ToList();
        }

        public IQueryable<T> All()
        {
            return _dbCollection.Find<T>(x => true).ToList().AsQueryable();
        }

        public IQueryable<T> All(params string[] columns)
        {
            return _dbCollection.Find<T>(x => true).ToList().AsQueryable();
        }

        public bool Any(Expression<Func<T, bool>> where)
        {
            return _dbCollection.Find<T>(where).Any();
        }

        public void Delete(T entity)
        {
            _dbCollection.DeleteOne<T>(x => x.Id == entity.Id);
        }

        public void Delete(List<T> entities)
        {
            entities.ForEach(x => _dbCollection.DeleteOne(d => d.Id == x.Id));
        }

        public T Find(Guid id)
        {
            return _dbCollection.Find<T>(x => x.Id == id).FirstOrDefault();
        }

        public T FindAndIncludes(Guid id)
        {
            return Find(id);
        }

        public T FindAndIncludes(Guid id, params string[] columns)
        {
            return Find(id);
        }

        public T FindOrThrow(Guid id, string message = "")
        {
            return FindOrThrow(id, message, (new List<string>()).ToArray());
        }

        public T FindOrThrow(Guid id, string message = "", params string[] columns)
        {
            var entity = Find(id);

            if (entity == null)
                throw new NotFoundExcepiton(string.IsNullOrEmpty(message) ? "Veri bulunamadı." : message, CAF.Core.Enums.ErrorType.Warning);
            else
                return entity;
        }

        public T Get(Expression<Func<T, bool>> where)
        {
            return _dbCollection.Find<T>(where).FirstOrDefault();
        }

        public T Get(Expression<Func<T, bool>> where, params string[] columns)
        {
            return _dbCollection.Find<T>(where).FirstOrDefault();
        }

        public T GetOrThrow(Expression<Func<T, bool>> where)
        {
            var entity = Get(where);

            if (entity == null)
                throw new NotFoundExcepiton("Veri bulunamadı.", CAF.Core.Enums.ErrorType.Warning);
            else
                return entity;
        }

        public IQueryable<T> OrderBy<TKey>(Expression<Func<T, TKey>> orderBy, bool isDesc)
        {
            return isDesc ? All().OrderByDescending(orderBy) : All().OrderBy(orderBy);
        }

        public void Update(T entity)
        {
            _dbCollection.ReplaceOne<T>(x => x.Id == entity.Id, entity);
        }

        public void Update(List<T> entities)
        {
            entities.ForEach(x => Update(x));
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> where)
        {
            return _dbCollection.Find<T>(where).ToList().AsQueryable();
        }

        public IQueryable<T> WhereNoFilter(Expression<Func<T, bool>> where)
        {
            return _dbCollection.Find<T>(where).ToList().AsQueryable();
        }
        protected TRepository ResolveRepository<TRepository>()
        {
            return HttpContextAccessor.HttpContext.RequestServices.GetService<TRepository>();
        }
    }
}
