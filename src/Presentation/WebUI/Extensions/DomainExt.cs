using Core;
using Domain;
using Domain.Entities;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace WebUI.Extensions
{
    public static class DomainExt
    {
        #region 通过路由获取系统菜单
        public static Sys_Menu GetSysMenuByRoute(string areaName, string controllerName, string actionName)
        {
            Sys_Menu rtn = null;
            //rtn = Container.Instance.Resolve<Sys_MenuService>().Query(new List<ICriterion>
            //{
            //    Expression.Like("AreaName", areaName, MatchMode.Anywhere),
            //    Expression.Like("ControllerName", controllerName, MatchMode.Anywhere),
            //    Expression.Like("ActionName", actionName, MatchMode.Anywhere)
            //}).FirstOrDefault();
            rtn = ContainerManager.Resolve<ISys_MenuService>().Find(m =>
                m.AreaName.Contains(areaName)
                && m.ControllerName.Contains(controllerName)
                && m.ActionName.Contains(actionName)
                && !m.IsDeleted
            );
            if (rtn == null)
            {
                // 如果没有 此 ActionName 对应的系统菜单，则忽视 ActionName 重查
                //rtn = Container.Instance.Resolve<Sys_MenuService>().Query(new List<ICriterion>
                //{
                //    Expression.Like("AreaName", areaName, MatchMode.Anywhere),
                //    Expression.Like("ControllerName", controllerName, MatchMode.Anywhere),
                //}).FirstOrDefault();
                rtn= ContainerManager.Resolve<ISys_MenuService>().Find(m =>
                     m.AreaName.Contains(areaName)
                     && m.ControllerName.Contains(controllerName)
                     && !m.IsDeleted
                );
            }

            return rtn;
        }

        public static Sys_Menu GetSysMenuByRoute(this RouteData routeData)
        {
            Sys_Menu rtn = null;
            string areaName = routeData.DataTokens["area"]?.ToString() ?? "";
            string controllerName = routeData.Values["controller"]?.ToString() ?? "";
            string actionName = routeData.Values["action"]?.ToString() ?? "";
            rtn = GetSysMenuByRoute(areaName, controllerName, actionName);

            return rtn;
        }
        #endregion
    }
}