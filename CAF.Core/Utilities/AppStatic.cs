using CAF.Core.Interface;

using System;

namespace CAF.Core.Utilities
{
    public static class AppStatic
    {
        private static ISession _session { get; set; } = new CommonSession();
        public static ISession Session
        {
            get
            {
                return _session;
            }
            set
            {
                _session = value;
            }
        }

        public static bool IsDevelopment
        {
            get
            {
                return Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
            }
        }
    }
}
