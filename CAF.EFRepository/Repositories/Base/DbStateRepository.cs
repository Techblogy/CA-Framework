using CAF.Core.Entities;
using CAF.Core.Exception;
using CAF.Core.Interface;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

using System;
using System.Linq;
using System.Linq.Expressions;

namespace CAF.EFRepository.Repositories.Base
{
    public abstract class DbStateRepository<T, Key> : EFRepositoryBase<T, Key> where T : UniqueEntity<Key>, IDbState
        where Key:struct
    {
        public DbStateRepository(EFDatabaseContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
        {
        }
        public override IQueryable<T> All()
        {
            return base.All().Where(x => x.DbState != Core.Enums.DbState.Deleted);
        }
        public override IQueryable<T> All(params string[] columns)
        {
            IQueryable<T> result = All();
            foreach (var item in columns)
            {
                result = result.Include(item);
            }
            return result;
        }
        public override T Get(Expression<Func<T, bool>> where)
        {
            return Where(where).FirstOrDefault();
        }
        public override T Get(Expression<Func<T, bool>> where, params string[] columns)
        {
            var entity = Table.Where(x => x.DbState != Core.Enums.DbState.Deleted).FirstOrDefault(where);
            if (entity != null)
            {
                foreach (var columnName in columns)
                {
                    Context.Entry(entity).Reference(columnName).Load();
                }
            }
            return entity;
        }

        public override IQueryable<T> Where(Expression<Func<T, bool>> where)
        {
            return base.Where(where).Where(x => x.DbState != Core.Enums.DbState.Deleted);
        }

        public override T GetOrThrow(Expression<Func<T, bool>> where)
        {
            var entity = Get(where);
            if (entity == null)
                throw new NotFoundExcepiton("Veri bulunamadı.", Core.Enums.ErrorType.Warning);
            else
                return entity;
        }
        public override bool Any(Expression<Func<T, bool>> where)
        {
            return base.Where(x => x.DbState != Core.Enums.DbState.Deleted).Any(where);
        }

    }

}
