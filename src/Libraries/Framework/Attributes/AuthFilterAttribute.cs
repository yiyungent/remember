using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Attributes
{
    using Domain;
    using Domain.Entities;
    using Framework.Common;
    using Framework.Config;
    using Framework.Infrastructure;
    using Framework.Infrastructure.Concrete;
    using Framework.Models;
    using Framework.RequestResult;
    using System.Web.Mvc;

    public class AuthFilterAttribute : ActionFilterAttribute
    {
        private string _authKey;

        public AuthFilterAttribute()
        {
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string areaName = filterContext.RouteData.DataTokens["area"] == null ? "" : filterContext.RouteData.DataTokens["area"].ToString();
            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;

            // hack install url
            if (areaName == "" && controllerName == "Install")
            {
                base.OnActionExecuting(filterContext);
                return;
            }
            this._authKey = GetCurrentAuthKey(filterContext);
            this.CheckLoginAccount(filterContext);

            base.OnActionExecuting(filterContext);
        }

        #region 检查登录用户--并作出处理
        private void CheckLoginAccount(ActionExecutingContext filterContext)
        {
            CurrentAccountModel currentAccountModel = AccountManager.GetCurrentAccount();
            //  检查当前请求会话是否需要权限认证
            if (CheckRequestNeedAuth(filterContext))
            {
                CheckAuthSufficientAndProcess(currentAccountModel, filterContext);
            }
        }
        #endregion

        #region 检查当前请求是否需要权限认证
        private bool CheckRequestNeedAuth(ActionExecutingContext filterContext)
        {
            bool needAuth = false;

            // 获取当前请求会话 对应的 AuthKey
            string currentAuthKey = this._authKey;
            AuthManager authManager = new AuthManager();
            if (authManager.NeedAuth(currentAuthKey))
            {
                needAuth = true;
            }

            return needAuth;
        }
        #endregion

        #region 检查此用户是否拥有当前会话请求(操作)权限
        private bool CheckAuthSufficient(int userId, ActionExecutingContext filterContext)
        {
            AuthManager authManager = new AuthManager();
            string authKey = this._authKey;

            return authManager.HasAuth(userId, authKey);
        }
        #endregion

        #region 检查此账号(含游客)是否拥有当前会话请求(操作)权限--并作出提示处理
        private void CheckAuthSufficientAndProcess(CurrentAccountModel currentAccountModel, ActionExecutingContext filterContext)
        {
            if (!CheckAuthSufficient(currentAccountModel.UserId, filterContext))
            {
                // 无权限
                if (currentAccountModel.IsGuest)
                {
                    // 游客--游客无权限，则让其登录
                    if (currentAccountModel.LoginStatus == LoginStatus.LoginTimeOut)
                    {
                        filterContext.Result = LoginTimeOutResultProvider.Get(filterContext.HttpContext.Request);
                    }
                    else
                    {
                        filterContext.Result = NeedLoginResultProvider.Get(filterContext.HttpContext.Request);
                    }
                }
                else
                {
                    // 非游客---已登录用户无权限则 就为 无权限
                    filterContext.Result = WithoutAuthResultProvider.Get(filterContext.HttpContext.Request);
                }
            }
        }
        #endregion









        #region 根据会话获取AuthKey
        private string GetCurrentAuthKey(ActionExecutingContext filterContext)
        {
            string authKey = string.Empty;
            // 1.首先尝试从 Attribute 中获取 AuthKey
            object authKeyAttrObj = filterContext.ActionDescriptor.GetCustomAttributes(typeof(AuthKeyAttribute), true).FirstOrDefault();
            if (authKeyAttrObj != null && authKeyAttrObj is AuthKeyAttribute)
            {
                AuthKeyAttribute authKeyAttr = authKeyAttrObj as AuthKeyAttribute;
                authKey = authKeyAttr.AuthKey;
            }
            else
            {
                // 2.如果没有使用 AuthkeyAttribute 指定 AuthKey 则使用 out areaName, out controllerName, out actionName 生成AuthKey
                string areaName, controllerName, actionName;
                GetAreaControllerActionName(filterContext, out areaName, out controllerName, out actionName);
                authKey = areaName + "." + controllerName + "." + actionName;
            }

            return authKey;
        }
        #endregion

        #region 根据当前会话请求获取 AreaName, ControllerName, ActionName
        private void GetAreaControllerActionName(ActionExecutingContext filterContext, out string areaName, out string controllerName, out string actionName)
        {
            controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            actionName = filterContext.ActionDescriptor.ActionName;

            areaName = filterContext.RouteData.DataTokens["area"] == null ? "" : filterContext.RouteData.DataTokens["area"].ToString();
        }
        #endregion
    }
}
