using CAF.Core.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace CAF.Core.Repository.Base
{
    public interface IRepository<T, Key> where T : UniqueEntity<Key>
    {

        Key Add(T entity);
        List<Key> Add(List<T> entities);
        void Update(T entity);
        void Update(List<T> entities);
        void Delete(T entity);
        void Delete(List<T> entities);
        T Find(Key id);
        T FindOrThrow(Key id, string message = "");
        T FindOrThrow(Key id, string message = "", params string[] columns);
        T FindAndIncludes(Key id);
        T FindAndIncludes(Key id, params string[] columns);

        bool Any(Expression<Func<T, bool>> where);

        IQueryable<T> All();
        IQueryable<T> All(params string[] columns);
        IQueryable<T> Where(Expression<Func<T, bool>> where);
        IQueryable<T> WhereNoFilter(Expression<Func<T, bool>> where);
        T Get(Expression<Func<T, bool>> where, params string[] columns);
        T Get(Expression<Func<T, bool>> where);
        T GetOrThrow(Expression<Func<T, bool>> where);
        IQueryable<T> OrderBy<TKey>(Expression<Func<T, TKey>> orderBy, bool isDesc);
    }
}
