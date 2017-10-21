using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cwagnerBugTracker.Hubs
{
    public class NotificationHub : Hub
    {
        public static void UpdateNotifications(string userId)
        {
            var hub = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();

            hub.Clients.User(userId).updateNotifications();
        }
    }
}