using System;
using System.Collections.Generic;
using System.Text;

namespace CAF.Core.ViewModel.Notification.Response
{
    public class BasicNotificationResponse
    {
        public string Message { get; set; }
        public string SenderName { get; set; }
        public int Count { get; set; }
    }
}
