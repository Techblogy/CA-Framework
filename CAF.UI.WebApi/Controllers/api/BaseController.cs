using CAF.Core.Interface;
using CAF.Core.Utilities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ExtensionCore;

namespace CAF.UI.WebApi.Controllers.api
{
    [Authorize]
    public class BaseController : ControllerBase
    {
        protected const string route = "api/[controller]/[action]";
        protected readonly IAppSettings AppSettings;
        protected readonly IConfiguration Configuration;
        public Core.Interface.ISession CurrentUser
        {
            get
            {
                return Core.Utilities.AppStatic.Session;
            }
        }

        public BaseController(IConfiguration configuration, IAppSettings appSettings)
        {
            AppSettings = appSettings;
            Configuration = configuration;
        }
        protected T ResolveService<T>()
        {
            return HttpContext.RequestServices.GetService<T>();
        }
    }
}
