using CAF.Core.Repository;
using CAF.Core.Service;
using CAF.Core.Utilities;

using Microsoft.AspNetCore.Http;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using CAF.Core.ViewModel.Common.Response;
using CAF.Core.ViewModel.User.Request;
using CAF.Core.Extensions;

namespace CAF.Service.Services
{
    public class HangfireJobService : Base.BaseService, IHangfireJobService
    {
        readonly IAppSettings AppSettings;
        private string token;

        public string Token
        {
            get
            {
                if (string.IsNullOrEmpty(token))
                {
                    var userId = AppSettings.SystemUserId > 0 ? AppSettings.SystemUserId : Core.Constant.UserRoleConstant.defaultSystemUserId;
                    token = Core.Helper.TokenHelper.CreateTokenBySystemUser(userId, AppSettings.Secret);
                }

                return token;
            }
        }


        public HangfireJobService(IHttpContextAccessor httpContextAccessor, IAppSettings appSettings, IServiceProvider serviceProvider) : base(httpContextAccessor, serviceProvider)
        {
            AppSettings = appSettings;
        }

        public void ClearErrorLog()
        {
            var repository = base.ResolveService<IErrorLogRepository>();

            var date = Core.Helper.UtilitiesHelper.DateTimeNow().AddDays(AppSettings.ErrorLogExpireDay * -1);
            var oldLogs = repository.Where(x => x.timestamp < date).ToList();

            repository.Delete(oldLogs);
        }

        public void ClearRequestLog()
        {
            var repository = base.ResolveService<IRequestLogRepository>();

            var date = Core.Helper.UtilitiesHelper.DateTimeNow().AddDays(AppSettings.RequestLogExpireDay * -1);
            var oldLogs = repository.Where(x => x.CreateDate < date).ToList();

            repository.Delete(oldLogs);
        }
    }
}
