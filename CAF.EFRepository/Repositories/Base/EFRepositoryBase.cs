using CAF.Core.Entities;
using CAF.Core.Exception;
using Microsoft.Extensions.DependencyInjection;
using CAF.Core.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using ISession = CAF.Core.Interface.ISession;
using CAF.Core.Repository.Base;

namespace CAF.EFRepository.Repositories.Base
{
    public abstract class EFRepositoryBase<T, Key> : IRepository<T, Key>
        where T : UniqueEntity<Key>
    {
        private IHttpContextAccessor HttpContextAccessor { get; }
        public ISession CurrentUser
        {
            get
            {
                return Core.Utilities.AppStatic.Session;
            }
        }
        public bool IsTopAdmin
        {
            get
            {
                return CurrentUser.Role == Core.Enums.UserRole.SystemAdmin;
            }
        }
        protected EFDatabaseContext Context
        {
            get
            {
                return dbContext;
            }
        }
        private readonly EFDatabaseContext dbContext;
        public EFRepositoryBase(EFDatabaseContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
            this.dbContext = dbContext;
            Table = dbContext.Set<T>();
        }
        public DbSet<T> Table { get; set; }

        #region CUD
        public virtual Key Add(T entity)
        {
            Table.Add(entity);
            Save();
            return entity.Id;
        }

        public virtual void Update(T entity)
        {
            var efEntity = this.Context.Entry(entity);
            efEntity.State = EntityState.Modified;
            Save();
        }

        public virtual void Delete(T entity)
        {
            Table.Remove(entity);
            Save();
        }
        #endregion

        public virtual bool Any(Expression<Func<T, bool>> where)
        {
            return Table.Any(where);
        }

        #region Get
        public virtual T Find(Key id)
        {
            return Table.Find(id);
        }

        public virtual T FindOrThrow(Key id, string message = "")
        {
            var data = this.Find(id);

            if (data == null)
                throw new NotFoundExcepiton(string.IsNullOrEmpty(message) ? $"{typeof(T).Name} bulunamadı." : message, Core.Enums.ErrorType.Warning);

            return data;
        }

        public virtual T FindOrThrow(Key id, string message = "", params string[] columns)
        {
            var entity = FindAndIncludes(id, columns);
            if (entity == null)
                throw new NotFoundExcepiton(string.IsNullOrEmpty(message) ? "Veri bulunamadı." : message, Core.Enums.ErrorType.Warning);

            return entity;
        }

        /// <summary>
        /// Eğer bir nesneyi getirirken ilişkileri dolu halde istiyorsanı bu metod ezilerek include etmek istediğiniz kısımları include ederke kullanın
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual T FindAndIncludes(Key id)
        {
            return Find(id);
        }
        /// <summary>
        /// Service metodların saece belli kolonları doldurmak sitendiğinde kullanabilir
        /// </summary>
        /// <param name="id"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public virtual T FindAndIncludes(Key id, params string[] columns)
        {
            var entity = Find(id);
            if (entity != null)
            {
                foreach (var columnName in columns)
                {
                    Context.Entry(entity).Reference(columnName).Load();
                }
            }
            return entity;
        }

        public virtual IQueryable<T> All()
        {
            return Table;
        }
        public virtual IQueryable<T> All(params string[] columns)
        {
            IQueryable<T> result = Table;
            foreach (var item in columns)
            {
                result = result.Include(item);
            }
            return result;
        }
        public virtual T Get(Expression<Func<T, bool>> where)
        {
            return Table.FirstOrDefault(where);
        }
        public virtual T Get(Expression<Func<T, bool>> where, params string[] columns)
        {
            var entity = Table.FirstOrDefault(where);
            if (entity != null)
            {
                foreach (var columnName in columns)
                {
                    Context.Entry(entity).Reference(columnName).Load();
                }
            }
            return entity;
        }
        public virtual IQueryable<T> Where(Expression<Func<T, bool>> where)
        {
            return Table.Where(where);
        }

        public IQueryable<T> WhereNoFilter(Expression<Func<T, bool>> where)
        {
            return Table.Where(where);
        }
        public virtual IQueryable<T> OrderBy<TKey>(Expression<Func<T, TKey>> orderBy, bool isDesc)
        {
            if (isDesc)
                return Table.OrderByDescending(orderBy);
            return Table.OrderBy(orderBy);
        }


        public virtual T GetOrThrow(Expression<Func<T, bool>> where)
        {
            var entity = Get(where);
            if (entity == null)
                throw new NotFoundExcepiton("Veri bulunamadı.", Core.Enums.ErrorType.Warning);
            else
                return entity;

        }
        #endregion

        #region Bulk
        public virtual List<Key> Add(List<T> entities)
        {
            Table.AddRange(entities);
            Save();
            return entities.Select(x => x.Id).ToList();
        }

        public virtual void Update(List<T> entities)
        {
            Table.UpdateRange(entities);
            Save();
        }

        public virtual void Delete(List<T> entities)
        {
            Table.RemoveRange(entities);
            Save();
        }
        #endregion

        private void Save()
        {
            dbContext.SaveChanges();
        }
        protected TRepository ResolveRepository<TRepository>()
        {
            return HttpContextAccessor.HttpContext.RequestServices.GetService<TRepository>();
        }

    }
}
