using Domain.Entities;
using Services.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public partial interface IUserInfoService : IService<UserInfo>
    {
        bool Exists(string userName, int exceptId = 0);

        IList<string> UserHaveAuthKeys(int userId);
    }
}
