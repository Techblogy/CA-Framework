using CAF.Core.Enums;

using System;
using System.Collections.Generic;
using System.Text;

namespace CAF.Core.Entities
{
    public class ActionLog : KeyEntitiy
    {
        public ActionLogType EnumType { get; set; }
        public string Message { get; set; }
        public long UserId { get; set; }
        public DateTime Date { get; set; }
    }
}
