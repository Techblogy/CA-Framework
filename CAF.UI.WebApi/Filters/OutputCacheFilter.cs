using CAF.Core.Service;
using CAF.Core.ViewModel.CacheObject.Request;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CAF.UI.WebApi.Filters
{
    public class OutputCacheFilter : ActionFilterAttribute
    {
        /// <summary>
        /// Default 3 Haours
        /// </summary>
        public int ExpiryHours { get; set; }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var id = GetUniqId(context.HttpContext);

            var service = (IOutputCacheObjectService)context.HttpContext.RequestServices.GetService(typeof(IOutputCacheObjectService));

            var response = service.GetCache(context.HttpContext.Request.Path.Value, id);
            if (response != null)
            {
                context.HttpContext.Response.StatusCode = 200;
                context.Result = new JsonResult(response);
            }
            else
            {
                base.OnActionExecuting(context);
            }

        }

        public override void OnResultExecuted(ResultExecutedContext context)
        {
            if (context.Result is ObjectResult)
            {
                var objectResult = (ObjectResult)context.Result;

                var service = (IOutputCacheObjectService)context.HttpContext.RequestServices.GetService(typeof(IOutputCacheObjectService));
                var id = GetUniqId(context.HttpContext);

                var request = new AddCacheObjectRequest(id, context.HttpContext.Request.Path.Value, objectResult.Value, objectResult.DeclaredType)
                {
                    ExpiryHours = this.ExpiryHours < 1 ? 3 : this.ExpiryHours
                };
                service.AddCache(request);
            }
            base.OnResultExecuted(context);

        }
        private string GetUniqId(Microsoft.AspNetCore.Http.HttpContext httpContext)
        {
            var url = httpContext.Request.Path.Value;
            var bodyText = string.Empty;
            if (httpContext.Request.Method == "POST")
            {
                var bodyStream = new StreamReader(httpContext.Request.Body);
                bodyStream.BaseStream.Seek(0, SeekOrigin.Begin);
                bodyText = bodyStream.ReadToEnd();
            }
            var token = string.Empty;
            if (httpContext.Request.Headers.Any(x => x.Key == "Authorization"))
            {
                token = Core.Utilities.AppStatic.Session.Token;
            }

            var id = $"{url}|{token}|{bodyText}";

            return id.GetHashCode().ToString();
        }
    }
}
