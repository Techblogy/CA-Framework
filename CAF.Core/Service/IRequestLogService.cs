using CAF.Core.ViewModel.RequestLog;
using CAF.Core.ViewModel.RequestLog.Request;
using CAF.Core.ViewModel.RequestLog.Response;

using System;
using System.Collections.Generic;
using System.Text;

namespace CAF.Core.Service
{
    public interface IRequestLogService
    {
        Guid AddRequestLog(AddRequestLogRequest request);
        void SetResponseBody(Guid Id, object response);
        List<RequestLogGridDetailVM> GetRequestLogGrid(GetRequestLogGridRequest request);
        List<ErrorLogGridDetailVM> GetErrorLogGrid(GetErrorLogGridRequest request);
    }
}
