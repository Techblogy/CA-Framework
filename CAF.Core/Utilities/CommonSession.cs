using CAF.Core.Enums;
using CAF.Core.Interface;

using System;
using System.Collections.Generic;

using Wangkanai.Detection.Models;

namespace CAF.Core.Utilities
{
    public class CommonSession : ISession
    {
        public long Id => 0;

        public string Type => "";

        public string AppName => Core.Constant.CommonConstant.ApplicationName;

        public UserRole Role => UserRole.Standard;

        public bool IsAccessToken => false;

        public string Token => "";

        public Guid LoginId => Guid.Empty;

        public Device Device { get { return Device.Desktop; } }
    }
}
