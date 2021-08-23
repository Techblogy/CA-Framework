using CAF.Core.Enums;
using CAF.Core.Repository;
using CAF.Core.Service;
using CAF.Service.Services.Base;

using Microsoft.AspNetCore.Http;

using System;
using System.Collections.Generic;
using System.Text;

namespace CAF.Service.Services
{
    public class ActionLogService : BaseService, IActionLogService
    {
        private readonly IActionLogRepository _actionLogRepository;
        public ActionLogService(IHttpContextAccessor httpContextAccessor, IActionLogRepository actionLogRepository,IServiceProvider serviceProvider) : base(httpContextAccessor, serviceProvider)
        {
            _actionLogRepository = actionLogRepository;
        }

        public void AddLog(ActionLogType type, string message, long userId)
        {
            _actionLogRepository.AddLog(type, message, userId);
        }
    }
}
