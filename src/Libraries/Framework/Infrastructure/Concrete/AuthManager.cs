using Core;
using Domain;
using Framework.Common;
using Framework.Config;
using Framework.Infrastructure.Abstract;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain;
using Domain.Entities;

namespace Framework.Infrastructure.Concrete
{
    public class AuthManager : IAuthManager
    {
        public IDBAccessProvider _dBAccessProvider;

        public AuthManager(IDBAccessProvider dBAccessProvider)
        {
            this._dBAccessProvider = dBAccessProvider;
        }

        public string GetAuthNameByKey(string authKey)
        {
            string authName = null;

            FunctionInfo func = _dBAccessProvider.GetFunctionInfoByAuthKey(authKey);

            if (func != null)
            {
                authName = func.Name;
            }

            return authName;
        }

        public bool HasAuth(UserInfo userInfo, string authKey)
        {
            if (userInfo == null || userInfo.RoleInfos == null || userInfo.RoleInfos.Count == 0)
            {
                return false;
            }
            foreach (RoleInfo role in userInfo.RoleInfos)
            {
                if (role.FunctionInfos == null || role.FunctionInfos.Count == 0)
                {
                    continue;
                }
                // 判断是否具有此具体操作权限
                IList<string> haveAuthKeyList = (from m in role.FunctionInfos
                                                 select m.AuthKey).ToList();
                if (haveAuthKeyList.Contains(authKey, new AuthKeyCompare()))
                {
                    return true;
                }
            }

            return false; ;
        }

        public bool HasAuth(string authKey)
        {
            // 获取当前登录用户
            UserInfo userInfo = AccountManager.GetCurrentUserInfo();

            return HasAuth(userInfo, authKey);
        }

        public bool HasAuth(UserInfo userInfo, string areaName, string controllerName, string actionName)
        {
            string authKey = GetAuthKey(areaName, controllerName, actionName);

            return HasAuth(userInfo, authKey);
        }

        public bool HasAuth(UserInfo userInfo, string controllerName, string actionName)
        {
            return HasAuth(userInfo, null, controllerName, actionName);
        }

        public bool HasAuth(string areaName, string controllerName, string actionName)
        {
            UserInfo userInfo = Tools.GetSession<UserInfo>(AppConfig.LoginAccountSessionKey);

            return HasAuth(userInfo, areaName, controllerName, actionName);
        }

        public bool HasAuth(string controllerName, string actionName)
        {
            return HasAuth("", controllerName, actionName);
        }

        public string GetAuthKey(string areaName, string controllerName, string actionName)
        {
            return areaName + "." + controllerName + "." + actionName;
        }

        public string GetAuthKey(string controllerName, string actionName)
        {
            return GetAuthKey(null, controllerName, actionName);
        }

        public bool NeedAuth(string authKey)
        {
            // 只要此 AuthKey 不存在于 FunctionInfo 则不需要权限认证
            bool needAuth = false;
            // 所有存在于 FunctionInfo 表中的操作都需要权限认证
            IList<string> needAuthAllAuthKeys = AllAuthKey();

            if (needAuthAllAuthKeys.Contains(authKey, new AuthKeyCompare()))
            {
                needAuth = true;
            }

            return needAuth;
        }

        #region 获取所有需要权限验证的操作地址
        /// <summary>
        /// 获取所有需要权限验证的操作地址
        /// </summary>
        public IList<AreaCAItem> AllAreaCAItem()
        {
            IList<AreaCAItem> rtnList = new List<AreaCAItem>();
            //IList<FunctionInfo> allFunction = Container.Instance.Resolve<FunctionInfoService>().GetAll();
            IList<FunctionInfo> allFunction = _dBAccessProvider.GetAllFunctionInfo();

            foreach (FunctionInfo func in allFunction)
            {
                rtnList.Add(func.AreaCAItem);
            }

            return rtnList;
        }
        #endregion

        #region 获取所有需要权限认证的权限(操作)键
        public IList<string> AllAuthKey()
        {
            IList<string> rtnList = new List<string>();
            //var allFunction = Container.Instance.Resolve<FunctionInfoService>().GetAll();
            IList<FunctionInfo> allFunction = _dBAccessProvider.GetAllFunctionInfo();
            rtnList = (from m in allFunction
                       select m.AuthKey.Trim()).ToList();

            return rtnList;
        }
        #endregion

        #region 认证通过--有权限/此请求不需要权限验证
        /// <summary>
        /// 认证通过--有权限/此请求不需要权限验证
        /// </summary>
        public bool CanPass(UserInfo userInfo, string authKey)
        {
            bool isPass = false;
            if (!NeedAuth(authKey) || HasAuth(userInfo, authKey))
            {
                isPass = true;
            }

            return isPass;
        }

        /// <summary>
        /// 认证通过--有权限/此请求不需要权限验证
        /// </summary>
        public bool CanPass(string authKey)
        {
            bool isPass = false;
            UserInfo userInfo = AccountManager.GetCurrentUserInfo();
            isPass = CanPass(userInfo, authKey);

            return isPass;
        }
        #endregion

        #region 获取此用户的系统菜单列表
        public IList<Sys_Menu> GetMenuListByUserInfo(UserInfo userInfo)
        {
            IList<Sys_Menu> menuList = new List<Sys_Menu>();

            foreach (RoleInfo role in userInfo.RoleInfos)
            {
                foreach (Sys_Menu menu in role.Sys_Menus)
                {
                    if (!menuList.Contains(menu, new Sys_Menu_Compare()))
                    {
                        menuList.Add(menu);
                    }
                }
            }

            return menuList;
        }

        /// <summary>
        /// 获取当前用户的系统菜单列表
        /// </summary>
        public IList<Sys_Menu> GetMenuListByUserInfo()
        {
            UserInfo userInfo = AccountManager.GetCurrentUserInfo();

            return GetMenuListByUserInfo(userInfo);
        }
        #endregion

        #region 获取此用户的权限操作列表
        public IList<FunctionInfo> GetFuncListByUserInfo(UserInfo userInfo)
        {
            IList<FunctionInfo> funcList = new List<FunctionInfo>();

            foreach (RoleInfo role in userInfo.RoleInfos)
            {
                foreach (FunctionInfo menu in role.FunctionInfos)
                {
                    if (!funcList.Contains(menu, new FunctionInfo_Compare()))
                    {
                        funcList.Add(menu);
                    }
                }
            }

            return funcList;
        }

        /// <summary>
        /// 获取当前用户的权限操作列表
        /// </summary>
        public IList<FunctionInfo> GetFuncListByUserInfo()
        {
            UserInfo userInfo = AccountManager.GetCurrentUserInfo();

            return GetFuncListByUserInfo(userInfo);
        }
        #endregion

        #region 获取所有菜单
        public IList<Sys_Menu> AllMenuList()
        {
            return _dBAccessProvider.AllMenuList();
        }
        #endregion

        #region 获取所有操作
        public IList<FunctionInfo> AllFuncList()
        {
            return _dBAccessProvider.AllFuncList();
        }
        #endregion

        #region 获取此角色的系统菜单列表
        public IList<Sys_Menu> GetMenuListByRole(RoleInfo roleInfo)
        {
            IList<Sys_Menu> menuList = new List<Sys_Menu>();
            foreach (Sys_Menu menu in roleInfo.Sys_Menus)
            {
                if (!menuList.Contains(menu, new Sys_Menu_Compare()))
                {
                    menuList.Add(menu);
                }
            }

            return menuList;
        }
        #endregion

        #region 获取此角色的权限操作列表
        public IList<FunctionInfo> GetFuncListByRole(RoleInfo roleInfo)
        {
            IList<FunctionInfo> funcList = new List<FunctionInfo>();

            foreach (FunctionInfo menu in roleInfo.FunctionInfos)
            {
                if (!funcList.Contains(menu, new FunctionInfo_Compare()))
                {
                    funcList.Add(menu);
                }
            }

            return funcList;
        }
        #endregion

        #region 分配菜单
        public bool AssignPower(int roleId, IList<int> menuIdList, IList<int> funcIdList)
        {
            bool isSuccess = false;
            RoleInfo roleInfo = _dBAccessProvider.GetRoleInfoById(roleId);
            IList<Sys_Menu> sys_Menus = _dBAccessProvider.GetSys_MenuListByIds(menuIdList.ToArray());
            IList<FunctionInfo> functionInfos = _dBAccessProvider.GetFunctionInfoListByIds(funcIdList.ToArray());
            roleInfo.Sys_Menus = sys_Menus;
            roleInfo.FunctionInfos = functionInfos;
            isSuccess = _dBAccessProvider.EditRoleInfo(roleInfo);

            return isSuccess;
        }
        #endregion
    }
}
