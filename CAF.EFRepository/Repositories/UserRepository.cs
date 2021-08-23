using CAF.Core.Entities;
using CAF.Core.Exception;
using CAF.Core.Helper;
using CAF.Core.Repository;
using CAF.EFRepository.Repositories.Base;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace CAF.EFRepository.Repositories
{
    public class UserRepository : DbStateRepository<User, long>, IUserRepository
    {
        public UserRepository(EFDatabaseContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
        {
        }

        #region IUserRepository
        public IQueryable<User> GetAllUsersWithJoins()
        {
            return All();
            //.Include();
        }

        public User Login(string email, string password)
        {
            var _password = password.ToMD5Hash();

            var user = Get(x => x.Email == email);
            if (user != null && user.Password != _password)
                throw new BadRequestException("Kullanıcı bulunamadı. Bilgileri kontrol ediniz.", "", Core.Enums.ErrorType.Warning);

            //if (user != null)
            //    base.Context.Entry(user).Reference(p => p.Agency).Load(); //Kullanıcının agenta bilgisini doldur
            return user;
        }


        public bool DatabaseInitializer(IConfiguration configuration, IServiceCollection services)
        {
            CAF.EFRepository.EFDatabaseInitializer.Initialize(base.Context, configuration, services);
            return true;
        }


        public DataTable GetQuery(string query)
        {
            return Helpers.EFHelper.RawSqlQuery(base.Context, query);
        }

        #endregion

        #region Override

        public override User FindOrThrow(long id, string message = "")
        {
            var user = base.Find(id);

            //1- Kullanıcı bulunamadıysa
            //2- Bulunan kullanıcısı kendi acentesıan ait değilse. Eğer Top Admin ise zaten yetki kontrolüne gerek yok.
            if (user == null) //|| (!IsTopAdmin && user?.AgencyId != CurrentUser.AgencyId))
                throw new NotFoundExcepiton(string.IsNullOrEmpty(message) ? "Kullanıcı bulunamadı." : message, Core.Enums.ErrorType.Warning);

            return user;
        }

        public override User GetOrThrow(Expression<Func<User, bool>> where)
        {
            var entity = Get(where);
            if (entity == null)
                throw new NotFoundExcepiton("Veri bulunamadı.", Core.Enums.ErrorType.Warning);
            else
                return entity;
        }
        /// <summary>
        /// Tüm kolonlar include olmalıdır
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override User FindAndIncludes(long id)
        {
            var user = base.FindAndIncludes(id);

            //base.Context.Entry(user).Reference(p => p.Agency).Load(); //Kullanıcının agenta bilgisini doldur

            return user;
        }


        #endregion
    }
}
