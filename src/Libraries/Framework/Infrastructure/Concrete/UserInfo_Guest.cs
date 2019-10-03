using Core;
using Domain;
using Domain.Entities;
using Framework.Factories;
using Framework.Infrastructure.Abstract;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Infrastructure.Concrete
{
    public class UserInfo_Guest
    {
        private static UserInfo _instance;

        private static IDBAccessProvider _dBAccessProvider;

        public static UserInfo Instance
        {
            get
            {
                if (_instance == null)
                {
                    _dBAccessProvider = HttpOneRequestFactory.Get<IDBAccessProvider>();

                    UserInfo guest = new UserInfo
                    {
                        UserName = "游客(未登录)",
                        Avatar = ":WebUISite:/assets/images/guest-avatar.jpg",
                        //RoleInfos = new List<RoleInfo>
                        //{
                        //    //Container.Instance.Resolve<RoleInfoService>().GetEntity(2)
                        //    _dBAccessProvider.GetGuestRoleInfo()
                        //}
                        Role_Users = new List<Role_User>()
                    };
                    guest.Role_Users.Add(new Role_User
                    {
                        RoleInfo = _dBAccessProvider.GetGuestRoleInfo()
                    });

                    _instance = guest;
                }

                return _instance;
            }
        }
    }
}
