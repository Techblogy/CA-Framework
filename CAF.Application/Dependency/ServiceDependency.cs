using CAF.Core.Service;
using CAF.Service.Services;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Text;

namespace CAF.Application.Dependency
{
    internal static class ServiceDependency
    {
        internal static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IStorageService, AzureBlobStorageService>();
            services.AddScoped<IActionLogService, ActionLogService>();
            services.AddScoped<IAccessTokenService, AccessTokenService>();
            services.AddScoped<IRequestLogService, RequestLogService>();
            services.AddScoped<IOutputCacheObjectService, OutputCacheObjectService>();
            services.AddScoped<IAllFileService, AllFileService>();
            services.AddScoped<ISettingService, SettingService>();
            services.AddSingleton<IMemoryCacheService, MemoryCacheService>();
            services.AddScoped<IHangfireJobService, HangfireJobService>();
            services.AddScoped<INotificationService, NotificationService>();

            return services;
        }
    }
}
