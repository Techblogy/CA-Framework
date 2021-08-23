using CAF.Core.ViewModel.Notification.Request;

using System;
using System.Collections.Generic;
using System.Text;

namespace CAF.Core.Service
{
    public interface INotificationService
    {
        void SendNotification(BasiNotificationRequest request);
    }
}
