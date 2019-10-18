using Domain;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Models
{
    public class CurrentAccountModel
    {
        public int UserId { get; set; }

        public bool IsGuest
        {
            get
            {
                bool isGuest = false;
                switch (this.LoginStatus)
                {
                    case LoginStatus.IsLogin:
                        isGuest = false;
                        break;
                    case LoginStatus.WithoutLogin:
                        isGuest = true;
                        break;
                    case LoginStatus.LoginTimeOut:
                        isGuest = true;
                        break;
                    default:
                        isGuest = true;
                        break;
                }
                return isGuest;
            }
        }

        public LoginStatus LoginStatus { get; set; }
    }

    public enum LoginStatus
    {
        /// <summary>
        /// 已登录
        /// </summary>
        IsLogin,

        /// <summary>
        /// 未登录
        /// </summary>
        WithoutLogin,

        /// <summary>
        /// 登录超时
        /// <para>使用"记住我", 但 已经过期</para>
        /// </summary>
        LoginTimeOut
    }
}
