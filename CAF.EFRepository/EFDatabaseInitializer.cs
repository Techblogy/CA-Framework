using CAF.Core.Entities;
using CAF.Core.Helper;

using ExtensionCore;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAF.EFRepository
{
    public class EFDatabaseInitializer
    {
        public static void Migrate(EFDatabaseContext context)
        {
            context.Database.Migrate();
        }
        public static void Initialize(EFDatabaseContext context, IConfiguration configuration, IServiceCollection services)
        {
            AddUsers(context, configuration, services);          
            AddSettings(context, configuration, services);
        }
        private static void AddUsers(EFDatabaseContext context, IConfiguration configuration, IServiceCollection services)
        {
            //var sp = services.BuildServiceProvider();
            var appSettings = configuration.GetSection("AppSettings");
            var adminUserName = appSettings.GetSection("AdminUserName")?.Value;
            var password = appSettings.GetSection("AdminPassword")?.Value ?? "0";

            if (!string.IsNullOrEmpty(adminUserName))
            {
                if (!context.Users.Any(x => x.Email == adminUserName))
                {
                    password = password.ToMD5Hash();
                    context.Users.Add(new Core.Entities.User()
                    {
                        Email = adminUserName,
                        FirstName = "System",
                        LastName = "Admin",
                        Password = password,
                        RoleType = Core.Enums.UserRole.SystemAdmin,
                        DbState = Core.Enums.DbState.Active
                    });
                    context.SaveChanges();
                }

            }

        }
        private static void AddSettings(EFDatabaseContext context, IConfiguration configuration, IServiceCollection services)
        {
        }
      
    }
}
