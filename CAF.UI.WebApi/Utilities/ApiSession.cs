using CAF.Core.Constant;
using CAF.Core.Enums;
using CAF.Core.Enums.SystemModule;

using ExtensionCore;

using Microsoft.AspNetCore.Http;

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Wangkanai.Detection.Services;

namespace CAF.UI.WebApi.Utilities
{
    public class ApiSession : Core.Interface.ISession
    {
        private readonly IHttpContextAccessor Context;
        public ApiSession(IHttpContextAccessor context)
        {
            Context = context;
        }

        private List<Claim> Claims
        {
            get
            {
                var IsHangfireThread = System.Threading.Thread.CurrentThread.Name?.StartsWith("Worker #") == true;

                if (IsHangfireThread)
                {
                    var stream = Core.Hangfire.Tokens.Get(System.Threading.Thread.CurrentThread.Name);
                    var handler = new JwtSecurityTokenHandler();
                    var tokenS = handler.ReadToken(stream) as JwtSecurityToken;

                    return tokenS.Claims.ToList();
                }
                else
                {
                    return Context.HttpContext.User.Claims.ToList();
                }
            }
        }
        public long Id
        {
            get
            {

                long deff;

                long.TryParse(Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name || x.Type == "unique_name")?.Value, out deff);

                return deff;
            }
        }

        public string Type
        {
            get
            {
                return Claims.FirstOrDefault(x => x.Type == "UserType")?.Value;
            }
        }

        public string AppName { get; } = CommonConstant.ApplicationName;

        public UserRole Role
        {
            get
            {
                var roleStr = Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role || x.Type == "role")?.Value;
                if (string.IsNullOrEmpty(roleStr)) return UserRole.Public;

                UserRole role = Enum.Parse<UserRole>(roleStr);

                return role;
            }
        }

        public bool IsAccessToken
        {
            get
            {
                var isAccessTokenStr = Claims.FirstOrDefault(x => x.Type == UserRoleConstant.IsAccessToken)?.Value;
                if (string.IsNullOrEmpty(isAccessTokenStr)) return false;

                return isAccessTokenStr == "true";
            }
        }

        public string Token
        {
            get
            {
                var IsHangfireThread = System.Threading.Thread.CurrentThread.Name?.StartsWith("Worker #") == true;
                if (IsHangfireThread)
                {
                    return Core.Hangfire.Tokens.Get(System.Threading.Thread.CurrentThread.Name);
                }
                else
                {
                    string authorization = Context.HttpContext.Request.Headers["Authorization"];

                    // If no authorization header found, nothing to process further
                    if (string.IsNullOrEmpty(authorization)) return string.Empty;

                    if (authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                    {
                        return authorization.Substring("Bearer ".Length).Trim();
                    }

                    return string.Empty;
                }

            }
        }

        public Guid LoginId
        {
            get
            {
                var loginStr = Claims.FirstOrDefault(x => x.Type == UserRoleConstant.LoginId)?.Value;
                if (string.IsNullOrEmpty(loginStr))
                    return Guid.Empty;
                else
                {
                    if (Guid.TryParse(loginStr, out Guid agencyId))
                        return agencyId;
                    else return Guid.Empty;
                }
            }
        }

        public Wangkanai.Detection.Models.Device Device
        {
            get
            {
                var service = (IDetectionService)Context.HttpContext.RequestServices.GetService(typeof(IDetectionService));
                return service?.Device?.Type ?? Wangkanai.Detection.Models.Device.Desktop;
                //IDetectionService detectionService
            }
        }
    }
}
