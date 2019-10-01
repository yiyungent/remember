using Domain.Entities;
using Services.Core;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implement
{
    public partial class UserInfoService : BaseService<UserInfo>, IUserInfoService
    {
        public bool Exists(string userName, int exceptId = 0)
        {
            bool isExist = this._repository.Count(
                m => m.UserName == userName
                && m.ID != exceptId
                && !m.IsDeleted
            ) > 0;

            return isExist;
        }
    }
}
