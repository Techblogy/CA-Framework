using CAF.Core.Enums;

using System;
using System.Collections.Generic;
using System.Text;

namespace CAF.Core.Repository
{
    public interface IActionLogRepository
    {
        void AddLog(ActionLogType type, string message, long userId);
    }
}
