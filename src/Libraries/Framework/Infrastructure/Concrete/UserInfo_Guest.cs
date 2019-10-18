using Core;
using Domain;
using Domain.Entities;
using Services;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Infrastructure.Concrete
{
    public class UserInfo_Guest
    {
        private static UserInfo _instance;

        public static UserInfo Instance
        {
            get
            {
                if (_instance == null)
                {
                    UserInfo guest = new UserInfo
                    {
                        UserName = "游客(未登录)",
                        Avatar = ":WebUISite:/assets/images/guest-avatar.jpg",
                        Role_Users = new List<Role_User>()
                    };
                    guest.Role_Users.Add(new Role_User
                    {
                        RoleInfo = ContainerManager.Resolve<IRoleInfoService>().Find(2)
                    });

                    _instance = guest;
                }

                return _instance;
            }
        }
    }
}
