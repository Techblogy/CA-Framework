using CAF.Core.Enums;

using System;
using System.Collections.Generic;
using System.Text;

namespace CAF.Core.Service
{
    public interface IActionLogService
    {
        void AddLog(ActionLogType type, string message, long userId);
    }
}
