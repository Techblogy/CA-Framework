using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CAF.Core.Service;
using CAF.Core.ViewModel.RequestLog;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CAF.UI.WebApi.Filters
{
    public class RequestLogFilter : ActionFilterAttribute
    {
        public Guid RequestId { get; private set; }
        public override Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            try
            {
                var logger = (IRequestLogService)context.HttpContext.RequestServices.GetService(typeof(IRequestLogService));

                var bodyStream = new StreamReader(context.HttpContext.Request.Body);
                bodyStream.BaseStream.Seek(0, SeekOrigin.Begin);
                var bodyText = bodyStream.ReadToEnd();

                var headers = context.HttpContext.Request.Headers.Select(x => $"{x.Key} : {x.Value}");
                var log = new AddRequestLogRequest()
                {
                    Ip = context.HttpContext?.Connection?.RemoteIpAddress?.ToString(),
                    RequestUrl = context.HttpContext.Request.GetDisplayUrl(),
                    RequestBody = bodyText,
                    RequestHeaders = string.Join(Environment.NewLine, headers)
                };
                RequestId = logger.AddRequestLog(log);

                context.HttpContext.Response.Headers.Add("SigortaligRequestId", RequestId.ToString());
            }
            catch
            {
            }

            return base.OnResultExecutionAsync(context, next);

        }
        public override void OnResultExecuted(ResultExecutedContext context)
        {
            try
            {
                var logger = (IRequestLogService)context.HttpContext.RequestServices.GetService(typeof(IRequestLogService));
                /* if (context.Result is Microsoft.AspNetCore.Mvc.ObjectResult)
                 {
                     var body = ((Microsoft.AspNetCore.Mvc.ObjectResult)context.Result).Value;
                     logger.SetResponseBody(RequestId, body);
                 }*/
            }
            catch
            {
            }

            base.OnResultExecuted(context);
        }
        public string GetRawBodyStringAsync(HttpRequest request, Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;

            request.Body.Seek(0, SeekOrigin.Begin);

            using (StreamReader stream = new StreamReader(request.Body, encoding))
            {
                return stream.ReadToEnd();
            }
        }
    }
}
