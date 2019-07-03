using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Attributes
{
    using Domain;
    using Domain.Base;
    using Framework.Common;
    using Framework.Config;
    using Framework.Factories;
    using Framework.Infrastructure;
    using Framework.Infrastructure.Abstract;
    using Framework.Infrastructure.Concrete;
    using Framework.Models;
    using Framework.RequestResult;
    using System.Web.Mvc;

    public class AuthFilterAttribute : ActionFilterAttribute
    {
        private IAuthManager _authManger = HttpOneRequestFactory.Get<IAuthManager>();

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
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            this.CheckLoginAccount(filterContext);
            stopwatch.Stop();
            TimeSpan t1 = stopwatch.Elapsed;// TotalSeconds	0.08653849999999999	double

            base.OnActionExecuting(filterContext);
        }

        #region 检查登录用户--并作出处理
        private void CheckLoginAccount(ActionExecutingContext filterContext)
        {
            //  检查当前请求会话是否需要权限认证
            if (CheckRequestNeedAuth(filterContext))
            {
                CurrentAccountModel currentAccount = new CurrentAccountModel();

                #region 废弃
                //if (CheckLoginStatus(filterContext))
                //{
                //    // 已登录--则查询当前登录用户-角色 是否拥有此请求会话权限
                //    currentAccount.UserInfo = Tools.GetSession<UserInfo>(AppConfig.LoginAccountSessionKey);
                //    currentAccount.IsGuest = false;
                //}
                //else
                //{
                //    // 未登录--则为默认游客角色--查询游客角色是否拥有此会话权限
                //    currentAccount.UserInfo = UserInfo_Guest.Instance;
                //    currentAccount.IsGuest = true;
                //} 
                #endregion

                currentAccount = AccountManager.GetCurrentAccount();

                CheckAuthSufficientAndProcess(currentAccount, filterContext);
            }
        }
        #endregion

        #region 检查当前请求是否需要权限认证
        private bool CheckRequestNeedAuth(ActionExecutingContext filterContext)
        {
            bool needAuth = false;

            // 获取当前请求会话 对应的 AuthKey
            string areaName, controllerName, actionName;
            GetAreaControllerActionName(filterContext, out areaName, out controllerName, out actionName);

            string currentAuthKey = this._authManger.GetAuthKey(areaName, controllerName, actionName);

            if (this._authManger.NeedAuth(currentAuthKey))
            {
                needAuth = true;
            }

            return needAuth;
        }
        #endregion

        #region 检查此用户是否拥有当前会话请求(操作)权限
        private bool CheckAuthSufficient(UserInfo userInfo, ActionExecutingContext filterContext)
        {
            // 根据当前会话请求，拼接 AuthKey
            string areaName, controllerName, actionName;
            GetAreaControllerActionName(filterContext, out areaName, out controllerName, out actionName);

            string authKey = this._authManger.GetAuthKey(areaName, controllerName, actionName);

            return this._authManger.HasAuth(userInfo, authKey);
        }
        #endregion

        #region 检查此账号(含游客)是否拥有当前会话请求(操作)权限--并作出提示处理
        private void CheckAuthSufficientAndProcess(CurrentAccountModel currentAccount, ActionExecutingContext filterContext)
        {
            string returnUrl = filterContext.HttpContext.Request.Url.AbsoluteUri;
            if (!CheckAuthSufficient(currentAccount.UserInfo, filterContext))
            {
                // 无权限
                if (currentAccount.IsGuest)
                {
                    // 游客--游客无权限，则让其登录
                    if (AccountManager.CheckLoginStatus() == LoginStatus.LoginTimeOut)
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

        #region 废弃
        //#region 检查登录状态-已登录/未登录(登录超时)
        //private bool CheckLoginStatus(ActionExecutingContext filterContext)
        //{
        //    bool isLogin = true;
        //    UserInfo userInfo = Tools.GetSession<UserInfo>(AppConfig.LoginAccountSessionKey);
        //    if (userInfo == null)
        //    {
        //        isLogin = false;
        //    }

        //    return isLogin;
        //}
        //#endregion 
        #endregion

        #region 处理需要登录（默认游客角色权限不足）的情况---转向登录处理
        private void RedirectToLogin(ActionExecutingContext filterContext)
        {
            filterContext.Result = NeedLoginResultProvider.Get(filterContext.HttpContext.Request);
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
