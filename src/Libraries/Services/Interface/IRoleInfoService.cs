using Domain.Entities;
using Services.Core;
using System.Collections.Generic;

namespace Services.Interface
{
    public partial interface IRoleInfoService : IService<RoleInfo>
    {
        IList<Sys_Menu> RoleHaveSys_Menus(int roleId);

        IList<FunctionInfo> RoleHaveFunctions(int roleId);

        bool AssignPower(int roleId, IList<int> menuIdList, IList<int> funcIdList, int operatorId);
    }
}
