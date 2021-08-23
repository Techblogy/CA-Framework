using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using CAF.Core.Exception;
using CAF.Core.Service;

using Microsoft.AspNetCore.Mvc.Filters;

namespace CAF.UI.WebApi.Filters
{
    public class AccessTokenFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (Core.Utilities.AppStatic.Session.IsAccessToken)
            {
                var accessTokenService = (IAccessTokenService)context.HttpContext.RequestServices.GetService(typeof(IAccessTokenService));

                var tokenValidateControl = accessTokenService.TokenValidateControl(Core.Utilities.AppStatic.Session.Token);
                if (tokenValidateControl.Valid)
                {
                    base.OnActionExecuting(context);
                }
                else
                {
                    var errorTitle = "Access Token doğrulanamadı.";
                    var error = tokenValidateControl.Message;
                    throw new BadRequestException(errorTitle, error, Core.Enums.ErrorType.Critical);
                }

            }
            else
            {
                base.OnActionExecuting(context);
            }
        }
    }
}
