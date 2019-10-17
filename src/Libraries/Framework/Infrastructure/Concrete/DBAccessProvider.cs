using Core;
using Domain;
using Framework.Infrastructure.Abstract;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Framework.Config;
using Framework.Models;
using Domain.Entities;
using Services.Implement;
using Services.Interface;

namespace Framework.Infrastructure.Concrete
{
    public class DBAccessProvider : IDBAccessProvider
    {
        #region Fields

        private readonly IFunctionInfoService _functionInfoService;

        private readonly IRoleInfoService _roleInfoService;

        private readonly IUserInfoService _userInfoService;

        private readonly ISys_MenuService _sys_MenuService;

        private readonly ISettingService _settingService;

        private readonly IRole_MenuService _role_MenuService;

        private readonly IRole_FunctionService _role_FunctionService;

        #endregion

        #region Ctor

        public DBAccessProvider(IFunctionInfoService functionInfoService, IRoleInfoService roleInfoService, IUserInfoService userInfoService, ISys_MenuService sys_MenuService, ISettingService settingService, IRole_MenuService role_MenuService, IRole_FunctionService role_FunctionService)
        {
            this._functionInfoService = functionInfoService;
            this._roleInfoService = roleInfoService;
            this._userInfoService = userInfoService;
            this._sys_MenuService = sys_MenuService;
            this._settingService = settingService;
            this._role_MenuService = role_MenuService;
            this._role_FunctionService = role_FunctionService;
        }

        #endregion

        #region Methods

        public IList<FunctionInfo> GetAllFunctionInfo()
        {
            #region 废弃
            //IList<FunctionInfo> allFunction = Container.Instance.Resolve<FunctionInfoService>().GetAll(); 
            #endregion
            IList<FunctionInfo> allFunction = this._functionInfoService.All()?.ToList() ?? new List<FunctionInfo>();

            return allFunction;
        }

        public FunctionInfo GetFunctionInfoByAuthKey(string authKey)
        {
            #region 废弃
            //FunctionInfo func = Container.Instance.Resolve<FunctionInfoService>().Query(new List<ICriterion>
            //{
            //    Expression.Eq("AuthKey", authKey)
            //}).FirstOrDefault(); 
            #endregion
            FunctionInfo func = this._functionInfoService.Find(m => m.AuthKey == authKey && !m.IsDeleted);

            return func;
        }

        public RoleInfo GetGuestRoleInfo()
        {
            #region 废弃
            //return Container.Instance.Resolve<RoleInfoService>().GetEntity(2); 
            #endregion
            // 注意：游客角色一开始就初始化，不可删除，ID 永远为 2
            return this._roleInfoService.Find(2);
        }

        public UserInfo GetUserInfoById(int id)
        {
            UserInfo rtnUserInfo = null;
            #region 废弃
            //UserInfoService userInfoService = Container.Instance.Resolve<UserInfoService>();
            //if (userInfoService.Exist(id))
            //{
            //    rtnUserInfo = userInfoService.GetEntity(id);
            //} 
            #endregion
            if (this._userInfoService.Contains(m => m.ID == id))
            {
                rtnUserInfo = this._userInfoService.Find(id);
            }

            return rtnUserInfo;
        }


        #region 获取所有菜单
        public IList<Sys_Menu> AllMenuList()
        {
            #region 废弃
            //menuList = Container.Instance.Resolve<Sys_MenuService>().GetAll(); 
            #endregion
            IList<Sys_Menu> menuList = this._sys_MenuService.All()?.ToList() ?? new List<Sys_Menu>();

            return menuList;
        }
        #endregion

        #region 获取所有操作
        public IList<FunctionInfo> AllFuncList()
        {
            #region 废弃
            //funcList = Container.Instance.Resolve<FunctionInfoService>().GetAll(); 
            #endregion
            IList<FunctionInfo> funcList = this._functionInfoService.All()?.ToList() ?? new List<FunctionInfo>();

            return funcList;
        }
        #endregion

        #region 编辑角色
        public bool EditRoleInfo(RoleInfo roleInfo)
        {
            bool isSuccess = false;
            try
            {
                #region 废弃
                //Container.Instance.Resolve<RoleInfoService>().Edit(roleInfo); 
                #endregion
                this._roleInfoService.Update(roleInfo);

                isSuccess = true;
            }
            catch (Exception ex)
            {
                isSuccess = false;
            }

            return isSuccess;
        }
        #endregion

        #region 编辑用户
        public bool EditUserInfo(UserInfo userInfo)
        {
            bool isSuccess = false;
            try
            {
                #region 废弃
                //Container.Instance.Resolve<UserInfoService>().Edit(userInfo); 
                #endregion
                this._userInfoService.Update(userInfo);

                isSuccess = true;
            }
            catch (Exception ex)
            {
                isSuccess = false;
            }

            return isSuccess;
        }
        #endregion

        public RoleInfo GetRoleInfoById(int id)
        {
            RoleInfo rtn = null;
            #region 废弃
            //rtn = Container.Instance.Resolve<RoleInfoService>().GetEntity(id); 
            #endregion
            rtn = this._roleInfoService.Find(id);

            return rtn;
        }

        public Sys_Menu GetSys_MenuById(int id)
        {
            Sys_Menu rtn = null;
            #region 废弃
            //rtn = Container.Instance.Resolve<Sys_MenuService>().GetEntity(id); 
            #endregion
            rtn = this._sys_MenuService.Find(id);

            return rtn;
        }

        public FunctionInfo GetFunctionInfoById(int id)
        {
            FunctionInfo rtn = null;
            #region 废弃
            //rtn = Container.Instance.Resolve<FunctionInfoService>().GetEntity(id); 
            #endregion
            rtn = this._functionInfoService.Find(id);

            return rtn;
        }

        public IList<Sys_Menu> GetSys_MenuListByIds(params int[] ids)
        {
            IList<Sys_Menu> rtn = new List<Sys_Menu>();
            #region 废弃
            //rtn = Container.Instance.Resolve<Sys_MenuService>().Query(new List<ICriterion>
            //{
            //    Expression.In("ID", ids.ToArray())
            //}); 
            #endregion
            if (ids != null && ids.Length >= 1)
            {
                rtn = this._sys_MenuService.Filter(m => ids.Contains(m.ID)).ToList();
            }

            return rtn;
        }

        public IList<FunctionInfo> GetFunctionInfoListByIds(params int[] ids)
        {
            IList<FunctionInfo> rtn = new List<FunctionInfo>();
            #region 废弃
            //rtn = Container.Instance.Resolve<FunctionInfoService>().Query(new List<ICriterion>
            //{
            //    Expression.In("ID", ids.ToArray())
            //}); 
            #endregion
            if (ids != null && ids.Length >= 1)
            {
                rtn = this._functionInfoService.Filter(m => ids.Contains(m.ID)).ToList();
            }

            return rtn;
        }

        public UserInfo GetUserInfoByUserName(string userName)
        {
            UserInfo rtn = null;
            #region 废弃
            //rtn = Container.Instance.Resolve<UserInfoService>().Query(new List<ICriterion>
            //{
            //    Expression.Eq("UserName", userName)
            //}).FirstOrDefault(); 
            #endregion
            rtn = this._userInfoService.Find(m => m.UserName == userName);

            return rtn;
        }

        public IList<FunctionInfo> GetFunctionListBySys_MenuId(int sys_menuId)
        {
            IList<FunctionInfo> rtn = null;
            #region 废弃
            //rtn = Container.Instance.Resolve<FunctionInfoService>().Query(new List<ICriterion>
            //{
            //    Expression.Eq("Sys_Menu.ID", sys_menuId)
            //}); 
            #endregion
            rtn = this._functionInfoService.Filter(m => m.Sys_MenuId == sys_menuId).ToList();

            return rtn;
        }

        public string GetSet(string key)
        {
            string rtn = null;
            #region 废弃
            //rtn = Container.Instance.Resolve<SettingService>().GetSet(key); 
            #endregion
            rtn = this._settingService.GetSet(key);

            return rtn;
        }

        public void Set(string key, string value)
        {
            this._settingService.Set(key, value);
        }

        public void SaveUserTemplateName(string templateName)
        {
            CurrentAccountModel currentAccount = AccountManager.GetCurrentAccount();
            if (!currentAccount.IsGuest)
            {
                currentAccount.UserInfo.TemplateName = templateName;
                #region 废弃
                //Container.Instance.Resolve<UserInfoService>().Edit(currentAccount.UserInfo); 
                #endregion
                this._userInfoService.Update(currentAccount.UserInfo);
            }
        }

        /// <summary>
        /// 清空此角色的所有系统菜单以及权限
        /// </summary>
        /// <param name="roleId"></param>
        public void ClearPower(int roleId)
        {
            RoleInfo roleInfo = this._roleInfoService.Find(m => m.ID == roleId && !m.IsDeleted);
            IList<Role_Menu> role_Menus = roleInfo.Role_Menus.ToList();
            foreach (var item in role_Menus)
            {
                item.IsDeleted = true;
                item.DeletedAt = DateTime.Now;
                this._role_MenuService.Update(item);
            }
            IList<Role_Function> role_Functions = roleInfo.Role_Functions.ToList();
            foreach (var item in role_Functions)
            {
                item.IsDeleted = true;
                item.DeletedAt = DateTime.Now;
                this._role_FunctionService.Update(item);
            }
        }

        #endregion
    }
}