using CAF.Core.Utilities;

using System;
using System.Collections.Generic;
using System.Text;

namespace CAF.Core.Entities
{
    public class Log : KeyEntitiy
    {
        public Log()
        {
            AppName = AppStatic.Session.AppName;
            LogDate = CAF.Core.Helper.UtilitiesHelper.DateTimeNow();
            Username = "";
            UserId = AppStatic.Session.Id;
            UserType = AppStatic.Session.Type;
        }


        public string AppName { get; set; }
        public DateTime LogDate { get; set; }
        public string Username { get; set; }
        public string UserType { get; set; }
        public long UserId { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string Detail { get; set; }
    }
}
