using Component.Base;
using Domain;
using Manager;
using NHibernate.Criterion;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Component
{
    public class UserInfoComponent : BaseComponent<UserInfo, UserInfoManager>, UserInfoService
    {
        public bool Exist(string userName, int exceptId = 0)
        {
            return _manager.Exist(userName, exceptId: exceptId);
        }

       


    }
}
