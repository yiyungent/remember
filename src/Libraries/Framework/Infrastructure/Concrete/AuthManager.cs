using Core;
using Domain;
using Framework.Common;
using Framework.Config;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Entities;
using Services.Interface;
using System.Data.Entity;

namespace Framework.Infrastructure.Concrete
{
    public class AuthManager
    {
        public AuthManager()
        {
        }

        public string GetAuthNameByKey(string authKey)
        {
            string authName = null;
            IFunctionInfoService functionInfoService = ContainerManager.Resolve<IFunctionInfoService>();

            FunctionInfo func = functionInfoService.Find(m => m.AuthKey == authKey && !m.IsDeleted);

            if (func != null)
            {
                authName = func.Name;
            }

            return authName;
        }

        public bool HasAuth(int userId, string authKey)
        {
            IList<string> haveAuthKeyList = UserHaveAuthKeys(userId);
            if (haveAuthKeyList.Contains(authKey, new AuthKeyCompare()))
            {
                return true;
            }

            return false; ;
        }

        public bool HasAuth(string authKey)
        {
            // 获取当前登录用户
            IList<string> haveAuthKeyList = CurrentUserHaveAuthKeys();
            if (haveAuthKeyList.Contains(authKey, new AuthKeyCompare()))
            {
                return true;
            }

            return false; ;
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

        #region 获取所有需要权限认证的权限(操作)键
        public IList<string> AllAuthKey()
        {
            IList<string> rtnList = new List<string>();

            IFunctionInfoService functionInfoService = ContainerManager.Resolve<IFunctionInfoService>();
            rtnList = functionInfoService.AllAuthKey();

            return rtnList;
        }
        #endregion

        #region 获取当前用户拥有的操作键
        public IList<string> CurrentUserHaveAuthKeys()
        {
            int currentUserId = AccountManager.GetCurrentAccount().UserId;
            IList<string> authKeys = UserHaveAuthKeys(currentUserId);

            return authKeys;
        }
        #endregion

        #region 获取此用户拥有的权限键
        public IList<string> UserHaveAuthKeys(int userId)
        {
            IList<string> authKeys = HttpSingleRequestStore.GetData($"UserHaveAuthKeys({userId})") as IList<string>;
            if (authKeys == null)
            {
                authKeys = new List<string>();
                var authKeyCompare = new AuthKeyCompare();
                if (userId != 0)
                {
                    // 非游客
                    UserInfo userInfo = ContainerManager.Resolve<IUserInfoService>().Find(m => m.ID == userId && !m.IsDeleted);
                    if (userInfo.Role_Users != null && userInfo.Role_Users.Count >= 1)
                    {
                        var roleInfos = userInfo.Role_Users.Select(m => m.RoleInfo);
                        foreach (var role in roleInfos)
                        {
                            var funcs = role.FunctionInfos;
                            foreach (var func in funcs)
                            {
                                if (!authKeys.Contains(func.AuthKey, authKeyCompare))
                                {
                                    authKeys.Add(func.AuthKey);
                                }
                            }
                        }
                    }
                }
                else
                {
                    // 游客
                    RoleInfo roleInfo = ContainerManager.Resolve<IRoleInfoService>().Find(m => m.ID == 2);
                    foreach (var func in roleInfo.FunctionInfos)
                    {
                        if (!authKeys.Contains(func.AuthKey, authKeyCompare))
                        {
                            authKeys.Add(func.AuthKey);
                        }
                    }
                }

                HttpSingleRequestStore.SetData($"UserHaveAuthKeys({userId})", authKeys);
            }

            return authKeys;
        }
        #endregion

        #region 认证通过--有权限/此请求不需要权限验证
        /// <summary>
        /// 认证通过--有权限/此请求不需要权限验证
        /// </summary>
        public bool CanPass(int userId, string authKey)
        {
            bool isPass = false;
            if (!NeedAuth(authKey) || HasAuth(userId, authKey))
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
            int userId = AccountManager.GetCurrentAccount().UserId;
            isPass = CanPass(userId, authKey);

            return isPass;
        }
        #endregion

    }
}
