using Core;
using Domain;
using NHibernate.Criterion;
using PluginHub.Plugins;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RankingChart
{
    public class RankingChartPlugin : BasePlugin
    {
        public RankingChartPlugin()
        {
        }

        public override void Install()
        {
            Sys_MenuService sys_MenuService = Container.Instance.Resolve<Sys_MenuService>();
            Sys_Menu parentMenu = sys_MenuService.Query(new List<ICriterion>
            {
                Expression.Like("Name", "系统管理")
            }).FirstOrDefault();
            try
            {
                sys_MenuService.Create(new Sys_Menu
                {
                    AreaName = "RankingChart",
                    ControllerName = "Home",
                    ActionName = "Index",
                    ParentMenu = parentMenu,
                    SortCode = 10
                });
            }
            catch (Exception ex)
            {
            }

            base.Install();
        }

        public override void Uninstall()
        {
            Sys_MenuService sys_MenuService = Container.Instance.Resolve<Sys_MenuService>();
            Sys_Menu menu = sys_MenuService.Query(new List<ICriterion>
            {
                Expression.Like("AreaName", "RankingChart")
            }).FirstOrDefault();
            if (menu != null)
            {
                sys_MenuService.Delete(menu);
            }

            base.Uninstall();
        }
    }
}