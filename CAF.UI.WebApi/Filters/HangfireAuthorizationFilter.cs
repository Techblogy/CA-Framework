using CAF.Core.Helper;
using CAF.Core.Repository;
using CAF.Core.Service;

using Hangfire.Dashboard;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAF.UI.WebApi.Filters
{
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();
            //httpContext.Response.Headers["x-frame-options"] = "allow-from *";
            httpContext.Request.Cookies.TryGetValue("Hangfire-BasicAuthentication", out string value);
            if (!string.IsNullOrEmpty(value))
            {
                byte[] data = Convert.FromBase64String(value);
                var userNamePassword = Encoding.UTF8.GetString(data)?.Split(':')?.ToList();
                var userName = userNamePassword[0];
                var password = userNamePassword[1];
                var userRepository = (IUserRepository)httpContext.RequestServices.GetService(typeof(IUserRepository));
                var user = userRepository.Login(userName, password);

                return user.RoleType == Core.Enums.UserRole.SystemAdmin;
            }
            else return false;


        }
    }
}
