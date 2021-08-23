using CAF.Core.Entities;
using CAF.Core.Interface;
using CAF.EFRepository.Mapping;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CAF.EFRepository
{
    public class EFDatabaseContext : DbContext
    {
        public EFDatabaseContext(DbContextOptions<EFDatabaseContext> options)
          : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<AccessToken> AccessTokens { get; set; }
        public DbSet<ActionLog> ActionLogs { get; set; }      
        public DbSet<AllFile> AllFiles { get; set; }
        public DbSet<Setting> Settings { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Write Fluent API configurations here
            modelBuilder.ApplyConfiguration(new UserMapping());
            modelBuilder.ApplyConfiguration(new LogMapping());
            modelBuilder.ApplyConfiguration(new ActionLogMapping());
            modelBuilder.ApplyConfiguration(new AccessTokenMapping());
            modelBuilder.ApplyConfiguration(new AllFileMapping());
            modelBuilder.ApplyConfiguration(new SettingMapping());
        }

        public override int SaveChanges()
        {
            string logonUserName = Core.Utilities.AppStatic.Session.Id.ToString();
            long? logonUserId = Core.Utilities.AppStatic.Session.Id;

            var entries = ChangeTracker.Entries()
                .Where(x =>
                               x.State == EntityState.Added || x.State == EntityState.Modified || x.State == EntityState.Deleted);
            foreach (var item in entries)
            {
                if (item.State == EntityState.Deleted)
                {
                    #region Delete

                    #region Soft Delete
                    if (item.Entity is IDbState)
                    {
                        ((IDbState)item.Entity).DbState = Core.Enums.DbState.Deleted;
                        item.State = EntityState.Modified;
                    }
                    #endregion

                    #region IUpdateEntity
                    if (item.Entity is IUpdated)
                    {
                        ((IUpdated)item.Entity).UpdateDate = Core.Helper.UtilitiesHelper.DateTimeNow();
                    }
                    if (item.Entity is IUpdateUser)
                    {
                        ((IUpdateUser)item.Entity).UpdateUserId = logonUserName;
                    }
                    if (item.Entity is IUpdateUserId)
                    {
                        ((IUpdateUserId)item.Entity).UpdateUserId = logonUserId;
                    }
                    #endregion

                    #region Ignore Property
                    if (item.Entity is ICreated)
                        Entry(item.Entity).Property("CreateDate").IsModified = false;
                    if (item.Entity is ICreateUser)
                        Entry(item.Entity).Property("CreateUserId").IsModified = false;
                    #endregion

                    #endregion
                }
                else if (item.State == EntityState.Modified)
                {
                    #region Hard Delete
                    if (item.Entity is IDbState)
                    {
                        var entity = (IDbState)item.Entity;
                        if (entity.DbState == Core.Enums.DbState.HardDelete)
                            item.State = EntityState.Deleted;
                    }
                    #endregion

                    #region Update

                    #region IUpdateEntity
                    if (item.Entity is IUpdated)
                    {
                        ((IUpdated)item.Entity).UpdateDate = Core.Helper.UtilitiesHelper.DateTimeNow();
                    }
                    if (item.Entity is IUpdateUser)
                    {
                        ((IUpdateUser)item.Entity).UpdateUserId = logonUserName;
                    }
                    if (item.Entity is IUpdateUserId)
                    {
                        ((IUpdateUserId)item.Entity).UpdateUserId = logonUserId;
                    }
                    #endregion

                    #region Ignore Property
                    if (item.Entity is ICreated)
                        Entry(item.Entity).Property("CreateDate").IsModified = false;
                    if (item.Entity is ICreateUser)
                        Entry(item.Entity).Property("CreateUserId").IsModified = false;
                    #endregion

                    #endregion
                }
                else if (item.State == EntityState.Added)
                {
                    #region Insert

                    #region ICreated
                    if (item.Entity is ICreated)
                    {
                        ((ICreated)item.Entity).CreateDate = Core.Helper.UtilitiesHelper.DateTimeNow();
                    }
                    #endregion

                    #region ICreateEntity
                    if (item.Entity is ICreateUser)
                    {
                        ((ICreateUser)item.Entity).CreateUserId = logonUserName;
                    }
                    if (item.Entity is ICreateUserId)
                    {
                        ((ICreateUserId)item.Entity).CreateUserId = logonUserId;
                    }
                    #endregion

                    #region ISoftDelete
                    if (item.Entity is IDbState)
                    {
                        if (((IDbState)item.Entity).DbState == Core.Enums.DbState.Deleted)
                        {
                            ((IDbState)item.Entity).DbState = Core.Enums.DbState.Active;
                        }
                    }
                    #endregion

                    #region Ignore Property
                    if (item.Entity is IUpdated)
                        Entry(item.Entity).Property("UpdateDate").IsModified = false;
                    if (item.Entity is IUpdateUser)
                        Entry(item.Entity).Property("UpdateUserId").IsModified = false;
                    #endregion
                    #endregion
                }
                else
                {
                    continue;
                }
            }
            return base.SaveChanges();
        }
    }

    public class EFDatabaseContextFactory : IDesignTimeDbContextFactory<EFDatabaseContext>
    {
        public IConfiguration Configuration { get; private set; }
        public EFDatabaseContextFactory()
        {
            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            // Build config
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
        }
        public EFDatabaseContextFactory(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public EFDatabaseContext CreateDbContext(params string[] args)
        {
            var connectionString = Configuration.GetConnectionString("DbConnection");
            var builder = new DbContextOptionsBuilder<EFDatabaseContext>();
            builder.UseSqlServer(connectionString);
            return new EFDatabaseContext(builder.Options);
        }
        public string GetConnectionString()
        {
            // Get environment
            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            // Build config
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location))
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            return configuration.GetConnectionString("DbConnection");
        }

    }
}
