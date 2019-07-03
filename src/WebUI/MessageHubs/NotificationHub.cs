using Domain;
using Framework.Config;
using Framework.Infrastructure.Concrete;
using Framework.Models;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebUI.MessageHubs
{
    [HubName("notificationHub")]
    public class NotificationHub : Hub
    {
        #region 重写父类OnConnected方法：客户端连接上的时候会调用此方法
        public override Task OnConnected()
        {
            Connected();
            return base.OnConnected();
        }
        #endregion

        #region 重写父类OnReconnected方法：客户端可能网络不稳定，造成重新连接，就会调用此方法
        // 注意：客户端（浏览器页面）在刷新的时候不属于重连接，刷新属于先断开连接，然后再连接
        public override Task OnReconnected()
        {
            return base.OnReconnected();
        }
        #endregion

        #region 重写Hub链接断开事件
        /// <summary>
        /// 重写Hub链接断开事件
        /// </summary>
        /// <param name="stopCalled"></param>
        /// <returns></returns>
        public override Task OnDisconnected(bool stopCalled)
        {
            Disconnected();
            return base.OnDisconnected(stopCalled);
        }
        #endregion

        #region 连接时
        private void Connected()
        {
            // 若用户已经登录，则将其 分组到 groupName-key; 登录用户 userId, 值当前 connectionId
            string sessionId = string.Empty;
            if (Context.RequestCookies.ContainsKey("SessionID"))
            {
                sessionId = Context.RequestCookies["SessionID"].Value;
            }
            UserInfo currentUser = (UserInfo)Context.Request.GetHttpContext().Session[AppConfig.LoginAccountSessionKey];
            if (currentUser != null)
            {
                Groups.Add(Context.ConnectionId, currentUser.ID.ToString());
                string accessTime = DateTime.Now.ToString("MM月dd日 hh:mm");
                string tipMessage = $"{currentUser.Name} - {accessTime}";
                Clients.Group(currentUser.ID.ToString()).notificationReceive("欢迎!", tipMessage, "/images/default-notify.jpg");
            }
        }
        #endregion

        #region 断开时
        private void Disconnected()
        {
            // 若用户已经登录，则从 userId 组中移除 当前connectionId
            CurrentAccountModel currentAccount = AccountManager.GetCurrentAccount();
            if (!currentAccount.IsGuest)
            {
                UserInfo currentUser = currentAccount.UserInfo;
                Groups.Remove(Context.ConnectionId, currentUser.ID.ToString());
            }
        }
        #endregion

    }
}
