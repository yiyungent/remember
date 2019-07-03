using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebUI.MessageHubs
{
    public class NotificationManager
    {
        public IHubContext HubContext { get; set; }

        public NotificationManager()
        {
            this.HubContext = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
        }

        public void Push(int userId, string title, string message, string imageUrl = null, string redirectUrl = null)
        {
            if (imageUrl == null)
            {
                imageUrl = "/images/default-notify.jpg";
            }
            HubContext.Clients.Group(userId.ToString()).notificationReceive(title, message, imageUrl, redirectUrl);
        }



    }
}
