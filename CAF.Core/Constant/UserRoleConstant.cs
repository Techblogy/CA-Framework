using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace CAF.Core.Constant
{
    public class UserRoleConstant
    {
        public static long defaultSystemUserId = 0;

        public const string IsAccessToken = "IsAccessToken";
        public const string LoginId = "LoginId";
        public const string Admin = "Admin";
        public const string All = "Admin";
        public static CultureInfo Culture = new CultureInfo("tr-TR");
    }
}
