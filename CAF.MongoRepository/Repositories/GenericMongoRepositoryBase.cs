using CAF.Core.Entities;
using CAF.Core.Exception;
using CAF.Core.Repository;
using CAF.MongoRepository.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;

using MongoDB.Driver;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using MongoDB.Bson;
using CAF.Core.Repository.Base;

namespace CAF.MongoRepository.Repositories
{
    public class GenericMongoRepositoryBase<T, Key> : IRepository<T, Key>
         where T : UniqueEntity<Key>
    {
        protected readonly IMongoDBContext _mongoContext;
        protected IMongoCollection<T> _dbCollection;
        private readonly IHttpContextAccessor HttpContextAccessor;

        protected GenericMongoRepositoryBase(IMongoDBContext context, IHttpContextAccessor httpContextAccessor)
        {
            _mongoContext = context;
            HttpContextAccessor = httpContextAccessor;
            _dbCollection = _mongoContext.GetCollection<T>(typeof(T).Name);
        }

        public virtual Key Add(T entity)
        {
            _dbCollection.InsertOne(entity);
            return entity.Id;
        }

        public List<Key> Add(List<T> entities)
        {
            _dbCollection.InsertMany(entities);
            return entities.Select(x => x.Id).ToList();
        }

        public virtual IQueryable<T> All()
        {
            return _dbCollection.AsQueryable();
        }

        public virtual IQueryable<T> All(params string[] columns)
        {
            return _dbCollection.AsQueryable();
        }

        public virtual bool Any(Expression<Func<T, bool>> where)
        {
            return _dbCollection.Find<T>(where).Any();
        }

        public virtual void Delete(T entity)
        {
            var filter = Builders<T>.Filter.Eq("_id", entity.Id);
            _dbCollection.DeleteOne(filter);
        }

        public virtual void Delete(List<T> entities)
        {
            
            entities.ForEach(x =>
            {
                var filter = Builders<T>.Filter.Eq("_id", x.Id);
                _dbCollection.DeleteOne(filter);
            });
        }

        public virtual T Find(Key id)
        {
            var filter = Builders<T>.Filter.Eq("_id", id);
            return _dbCollection.Find(filter).FirstOrDefault();
        }

        public virtual T FindAndIncludes(Key id)
        {
            return Find(id);
        }

        public virtual T FindAndIncludes(Key id, params string[] columns)
        {
            return Find(id);
        }

        public virtual T FindOrThrow(Key id, string message = "")
        {
            return FindOrThrow(id, message, (new List<string>()).ToArray());
        }

        public virtual T FindOrThrow(Key id, string message = "", params string[] columns)
        {
            var entity = Find(id);

            if (entity == null)
                throw new NotFoundExcepiton(string.IsNullOrEmpty(message) ? "Veri bulunamadı." : message, CAF.Core.Enums.ErrorType.Warning);
            else
                return entity;
        }

        public virtual T Get(Expression<Func<T, bool>> where)
        {
            return _dbCollection.Find<T>(where).FirstOrDefault();
        }

        public virtual T Get(Expression<Func<T, bool>> where, params string[] columns)
        {
            return _dbCollection.Find<T>(where).FirstOrDefault();
        }

        public virtual T GetOrThrow(Expression<Func<T, bool>> where)
        {
            var entity = Get(where);

            if (entity == null)
                throw new NotFoundExcepiton("Veri bulunamadı.", CAF.Core.Enums.ErrorType.Warning);
            else
                return entity;
        }

        public virtual IQueryable<T> OrderBy<TKey>(Expression<Func<T, TKey>> orderBy, bool isDesc)
        {
            return isDesc ? All().OrderByDescending(orderBy) : All().OrderBy(orderBy);
        }

        public virtual void Update(T entity)
        {
            var filter = Builders<T>.Filter.Eq("_id", entity.Id);
            _dbCollection.ReplaceOne(filter, entity);
        }

        public virtual void Update(List<T> entities)
        {
            entities.ForEach(x => Update(x));
        }

        public virtual IQueryable<T> Where(Expression<Func<T, bool>> where)
        {
            return _dbCollection.Find<T>(where).ToList().AsQueryable();
        }

        public virtual IQueryable<T> WhereNoFilter(Expression<Func<T, bool>> where)
        {
            return _dbCollection.Find<T>(where).ToList().AsQueryable();
        }
        protected TRepository ResolveRepository<TRepository>()
        {
            return HttpContextAccessor.HttpContext.RequestServices.GetService<TRepository>();
        }
    }
}
