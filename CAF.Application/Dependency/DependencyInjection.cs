using CAF.Core.Utilities;
using CAF.EFRepository;
using CAF.MongoRepository.Core;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System;

namespace CAF.Application.Dependency
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDbContextServices(this IServiceCollection services, IConfiguration configuration)
        {
            ServiceProvider provider = services.BuildServiceProvider();

            string connectionString = configuration.GetConnectionString("DbConnection");
            MongoRepository.Core.MongoMapping.SetMapping();
            services.AddDbContext<EFDatabaseContext>(x => x.UseSqlServer(connectionString));
            Environment.SetEnvironmentVariable("EF_CONNECTION_STRING", connectionString);
            services.AddSingleton<IAppSettings>(serviceProvider => configuration.GetSection("AppSettings").Get<AppSettings>());
            services.AddScoped<IMongoDBContext, MongoDBContext>();

            services.AddHelper(configuration);
            services.AddRepositories(configuration);
            services.AddServices(configuration);
            services.AddValidations(configuration);
            return services;
        }
    }
}
