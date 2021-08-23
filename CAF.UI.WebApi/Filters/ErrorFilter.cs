

using CAF.Core.Exception;
using CAF.Core.Extensions;
using CAF.Core.Helper;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;


namespace CAF.UI.WebApi.Filters
{
    public class ErrorFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is BaseExcepiton)
            {
                var ex = (BaseExcepiton)context.Exception;
                ExceptionModel exceptionModel = new ExceptionModel()
                {
                    Message = ex.Message,
                    Detail = ex.Detail,
                    Type = ex.Type
                };

                if (context.Exception is NotFoundExcepiton) context.HttpContext.Response.StatusCode = 404;
                if (context.Exception is BadRequestException) context.HttpContext.Response.StatusCode = 400;


                context.Result = new JsonResult(exceptionModel);
            }
            else
            {
                //if (!Startup.IsDevelopment)
                //{
                var logger = (ILogger)context.HttpContext.RequestServices.GetService(typeof(ILogger));
                logger.Error(context.Exception);
                //}

                ExceptionModel exceptionModel = new ExceptionModel()
                {
                    Message = context.Exception.GetExceptionMessage(),
                    Detail = context.Exception.ToString(),
                    Type = Core.Enums.ErrorType.Critical
                };

                context.HttpContext.Response.StatusCode = 500;


                context.Result = new JsonResult(exceptionModel);
            }
        }
    }
}
