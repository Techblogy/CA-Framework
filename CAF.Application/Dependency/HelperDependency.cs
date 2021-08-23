using CAF.Core.Helper;
using CAF.Core.Helper.Interface;
using CAF.Service.Helper;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Text;

namespace CAF.Application.Dependency
{
    internal static class HelperDependency
    {
        internal static IServiceCollection AddHelper(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ICipherHelper, CipherHelper>();
            services.AddSingleton<IMailHelper, MailHelper>();
            return services;
        }
    }
}
