using CAF.Core.Entities;
using CAF.Core.Repository;
using CAF.Core.Service;
using CAF.Core.ViewModel.RequestLog;
using CAF.Core.ViewModel.RequestLog.Request;
using CAF.Core.ViewModel.RequestLog.Response;
using CAF.Service.Services.Base;

using Microsoft.AspNetCore.Http;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAF.Service.Services
{
    public class RequestLogService : BaseService, IRequestLogService
    {
        private readonly IRequestLogRepository _requestLogRepository;
        public RequestLogService(IHttpContextAccessor httpContextAccessor, IRequestLogRepository requestLogRepository,IServiceProvider serviceProvider) : base(httpContextAccessor, serviceProvider)
        {
            _requestLogRepository = requestLogRepository;
        }

        public Guid AddRequestLog(AddRequestLogRequest request)
        {
            var entity = request.ToEntity();
            entity.CreateDate = CAF.Core.Helper.UtilitiesHelper.DateTimeNow();
            entity.CreateUserId = CurrentUser != null ? CurrentUser.Id.ToString() : string.Empty;

            return _requestLogRepository.Add(entity);
        }

        public List<ErrorLogGridDetailVM> GetErrorLogGrid(GetErrorLogGridRequest request)
        {
            var errorLogRepository = base.ResolveService<IErrorLogRepository>();
            List<ErrorLog> entities;
            if (string.IsNullOrEmpty(request.SearchText))
            {
                entities = errorLogRepository.All()
                           .Skip(request.PageSize * (request.Page - 1))
                           .Take(request.PageSize)
                           .ToList();
            }
            else
            {
                entities = errorLogRepository
                           .Where(x => x.stacktrace.Contains(request.SearchText) || x.message.Contains(request.SearchText))
                           .Skip(request.PageSize * (request.Page - 1))
                           .Take(request.PageSize)
                           .ToList();
            }

            if (request.OrderbyType == Core.Enums.SystemModule.OrderbyType.Asc)
                entities = entities.OrderBy(x => x.timestamp).ToList();
            else
                entities = entities.OrderByDescending(x => x.timestamp).ToList();

            return entities.Select(x => new ErrorLogGridDetailVM(x)).ToList();
        }

        public List<RequestLogGridDetailVM> GetRequestLogGrid(GetRequestLogGridRequest request)
        {
            List<RequestLog> entities;
            if (string.IsNullOrEmpty(request.SearchText))
            {
                entities = _requestLogRepository.All()
                           .Skip(request.PageSize * (request.Page - 1))
                           .Take(request.PageSize)
                           .ToList();
            }
            else
            {
                entities = _requestLogRepository
                            .Where(x => x.RequestUrl.Contains(request.SearchText) || x.Ip.Contains(request.SearchText) || x.RequestHeaders.Contains(request.SearchText))
                           .Skip(request.PageSize * (request.Page - 1))
                           .Take(request.PageSize)
                           .ToList();
            }

            if (request.OrderbyType == CAF.Core.Enums.SystemModule.OrderbyType.Asc)
                entities.OrderBy(x => x.CreateDate);
            else
                entities.OrderByDescending(x => x.CreateDate);

            return entities.Select(x => new RequestLogGridDetailVM(x)).ToList();
        }

        public void SetResponseBody(Guid Id, object response)
        {
            var log = _requestLogRepository.Find(Id);
            log.ResponseBody = response;
            _requestLogRepository.Update(log);
        }
    }
}
