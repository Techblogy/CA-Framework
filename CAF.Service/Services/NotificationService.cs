using CAF.Core.Modules.SignalR;
using CAF.Core.Service;
using CAF.Core.ViewModel.Notification.Request;
using CAF.Core.ViewModel.Notification.Response;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;

using System;
using System.Collections.Generic;
using System.Text;

namespace CAF.Service.Services
{
    public class NotificationService : Base.BaseService, INotificationService
    {
        private readonly IHubContext<NotificationHub> NotificationHub;
        public NotificationService(IHubContext<NotificationHub> notificationHub, IHttpContextAccessor httpContextAccessor, IServiceProvider _serviceProvider) : base(httpContextAccessor, _serviceProvider)
        {
            this.NotificationHub = notificationHub;
        }

        public void SendNotification(BasiNotificationRequest request)
        {
            this.NotificationHub.Clients.All.SendAsync("BasicNotification", new BasicNotificationResponse()
            {
                SenderName = "Çılgın ofli",
                Count = 5,
                Message = $"Gönderdiğin mesaj: {request.MessageText}, Gönderdiğin extra değer: {request.ExtraMessage}"
            });
        }
    }
}
