using Core;
using Domain;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Infrastructure
{
    public class ApiAccountManager
    {
        public static UserInfo GetCurrentUserInfo()
        {
            UserInfo userInfo = Container.Instance.Resolve<UserInfoService>().GetEntity(1);

            return userInfo;
        }
    }
}