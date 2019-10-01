using Domain;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Infrastructure.Abstract
{
    public interface IAuthManager
    {
        bool CanPass(UserInfo userInfo, string authKey);

        bool CanPass(string authKey);

        bool HasAuth(UserInfo userInfo, string authKey);

        bool HasAuth(UserInfo userInfo, string areaName, string controllerName, string actionName);

        bool HasAuth(UserInfo userInfo, string controllerName, string actionName);

        bool HasAuth(string authKey);

        bool HasAuth(string areaName, string controllerName, string actionName);

        bool HasAuth(string controllerName, string actionName);

        string GetAuthNameByKey(string authKey);

        string GetAuthKey(string areaName, string controllerName, string actionName);

        string GetAuthKey(string controllerName, string actionName);

        bool NeedAuth(string authKey);

        IList<AreaCAItem> AllAreaCAItem();

        IList<string> AllAuthKey();

        IList<Sys_Menu> GetMenuListByUserInfo(UserInfo userInfo);

        IList<Sys_Menu> GetMenuListByUserInfo();

        IList<FunctionInfo> GetFuncListByUserInfo(UserInfo userInfo);

        IList<FunctionInfo> GetFuncListByUserInfo();

        IList<Sys_Menu> AllMenuList();

        IList<FunctionInfo> AllFuncList();

        /// <summary>
        /// 获取此角色的系统菜单列表
        /// </summary>
        IList<Sys_Menu> GetMenuListByRole(RoleInfo roleInfo);

        /// <summary>
        /// 获取此角色的权限操作列表
        /// </summary>
        IList<FunctionInfo> GetFuncListByRole(RoleInfo roleInfo);

        bool AssignPower(int roleId, IList<int> menuIdList, IList<int> funcIdList);
    }
}
