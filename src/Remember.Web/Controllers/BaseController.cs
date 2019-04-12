using Remember.Domain;
using Remember.Web.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Remember.Web.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string controllerName = filterContext.Controller.ToString();
            string actionName = filterContext.ActionDescriptor.ActionName;

            // 当前请求是否检查权限
            if (filterContext.Controller.GetType().GetCustomAttributes(typeof(NeedlessAuthAttribute), false).Length > 0 || filterContext.ActionDescriptor.GetCustomAttributes(typeof(NeedlessAuthAttribute), false).Length > 0)
            {
                // 不需要权限检查
                base.OnActionExecuting(filterContext);
            }
            else
            {
                // 需要进行权限检查
                // 获取登录用户
                SysUser loginUser = Session["loginUser"] as SysUser;
                // 未登录跳转到登录页面
                if (loginUser == null)
                {
                    if (controllerName.Contains("AdminController"))
                    {
                        // 后台登录
                        filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary
                        {
                            { "Controller", "Admin" },
                            { "Action", "Login" },
                            { "area", "Admin" }
                        });
                    }
                    else
                    {
                        // 前台登录
                        filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary
                        {
                            { "Controller", "Home" },
                            { "Action", "Login" }
                        });
                    }

                    return;
                }

                // 已经登录，判断权限
                if (HasAuth(loginUser, controllerName, actionName))
                {
                    // 有权限
                    base.OnActionExecuting(filterContext);
                }
                else
                {
                    // 无权限, 跳转到登录界面
                    filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary
                    {
                        { "Controller", "Home" },
                        { "Action", "Login" }
                    });
                }
            }
        }

        #region 判断用户对指定控制器与动作路由是否有权限
        /// <summary>
        /// 判断用户对指定控制器与动作路由是否有权限
        /// </summary>
        private bool HasAuth(SysUser loginUser, string controllerName, string actionName)
        {
            //bool isHas = false;
            //foreach (SysRole role in loginUser.SysRoleList)
            //{
            //    foreach (SysMenu menu in role.SysMenuList)
            //    {
            //        if (menu.ClassName != null && menu.ClassName.ToUpper() == controllerName.ToUpper())
            //        {
            //            isHas = true;
            //            return isHas;
            //        }
            //    }
            //}

            //return isHas;
            return true;
        }
        #endregion
    }
}
