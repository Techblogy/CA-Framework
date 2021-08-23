using CAF.Core.Enums;
using CAF.Core.Exception;
using CAF.Core.Extensions;
using CAF.Core.Service;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Text;

using ISession = CAF.Core.Interface.ISession;

namespace CAF.Service.Services.Base
{
    public abstract class BaseService
    {
        private IHttpContextAccessor HttpContextAccessor { get; }

        private IOutputCacheObjectService _cacheObjectService;
        private readonly IServiceProvider serviceProvider;

        protected IOutputCacheObjectService cacheObjectService
        {
            get
            {
                if (_cacheObjectService == null)
                    _cacheObjectService = ResolveService<IOutputCacheObjectService>();
                return _cacheObjectService;
            }
        }
        public ISession CurrentUser
        {
            get
            {
                return Core.Utilities.AppStatic.Session;
            }
        }
        public bool IsLogin
        {
            get { return (Core.Utilities.AppStatic.Session != null && Core.Utilities.AppStatic.Session?.Id > 0); }
        }
        public bool IsTopAdmin
        {
            get
            {
                return CurrentUser.Role == Core.Enums.UserRole.SystemAdmin;
            }
        }

        public BaseService(IHttpContextAccessor httpContextAccessor, IServiceProvider _serviceProvider)
        {
            HttpContextAccessor = httpContextAccessor;
            this.serviceProvider = _serviceProvider;
        }
        protected T ResolveService<T>()
        {
            if (HttpContextAccessor?.HttpContext?.RequestServices != null)
                return HttpContextAccessor.HttpContext.RequestServices.GetService<T>();
            if (serviceProvider != null)
                return serviceProvider.GetService<T>();

            throw new BadRequestException("Interface not resolved");
        }
        protected void ClearOutputCache(params CachePath[] path)
        {
            foreach (var item in path)
            {
                cacheObjectService.RemoveCache(item.GetDisplayNameValue());
            }
        }
    }
}
