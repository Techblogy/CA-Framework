using CAF.Core.Repository;
using CAF.EFRepository;
using CAF.EFRepository.Repositories;
using CAF.MongoRepository.Repositories;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System.Linq;

namespace CAF.Application.Dependency
{
    internal static class RepositoryDependency
    {
        internal static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            #region Entitiy Framework
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAllFileRepository, AllFileRepository>();
            services.AddScoped<ISettingRepository, SettingRepository>();
            #endregion
            #region Mongo
            services.AddScoped<IErrorLogRepository, ErrorLogRepository>();
            services.AddScoped<IActionLogRepository, EFRepository.Repositories.ActionLogRepository>();
            services.AddScoped<IAccessTokenRepository, AccessTokenRepository>();
            services.AddScoped<IRequestLogRepository, RequestLogRepository>();
            services.AddScoped<ICacheObjectRepository, CacheObjectRepository>();
            #endregion
            #region Rest
            #endregion

            EFDatabaseContextFactory databaseContextFactory = new EFDatabaseContextFactory(configuration);
            EFDatabaseContext context = databaseContextFactory.CreateDbContext();

            EFDatabaseInitializer.Migrate(context);
            EFDatabaseInitializer.Initialize(context, configuration, services);

            //set default system user - hangfire gibi işlemleri yapacak kullanıcı
            var defaultsystemUserId = context.Users.FirstOrDefault(x => x.RoleType == Core.Enums.UserRole.SystemAdmin)?.Id ?? 0;
            Core.Constant.UserRoleConstant.defaultSystemUserId = defaultsystemUserId;

            return services;
        }
    }
}
