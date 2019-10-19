using Domain.Entities;
using Services.Core;
using System.Collections.Generic;

namespace Services.Interface
{
    public partial interface IRole_UserService : IService<Role_User>
    {
        void UserAssignRoles(int userId, IList<int> roleIds, int operatorId);
    }
}
