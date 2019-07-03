using Core;
using Dapper;
using Domain;
using NHibernate.Criterion;
using PluginHub.Plugins;
using Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace ArticleContent
{
    public class ArticleContentPlugin : BasePlugin
    {
        public ArticleContentPlugin()
        {
        }

        public override void Install()
        {
            Sys_MenuService sys_MenuService = Container.Instance.Resolve<Sys_MenuService>();
            Sys_Menu parentMenu = sys_MenuService.Query(new List<ICriterion>
            {
                Expression.Like("Name", "业务管理")
            }).FirstOrDefault();
            sys_MenuService.Create(new Sys_Menu
            {
                AreaName = "ArticleContent",
                ControllerName = "Home",
                ActionName = "Index",
                ParentMenu = parentMenu,
                SortCode = 10
            });
            // 增加文章表
            var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlDiagnosticsDb"].ConnectionString);
            string sqlFilePath = HostingEnvironment.MapPath("~/plugins/ArticleContent/install.sql");
            string createTableSql = File.ReadAllText(sqlFilePath);
            conn.Execute(createTableSql);

            base.Install();
        }

        public override void Uninstall()
        {
            Sys_MenuService sys_MenuService = Container.Instance.Resolve<Sys_MenuService>();
            Sys_Menu menu = sys_MenuService.Query(new List<ICriterion>
            {
                Expression.Like("AreaName", "ArticleContent")
            }).FirstOrDefault();
            sys_MenuService.Delete(menu);
            // 删除文章表
            var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlDiagnosticsDb"].ConnectionString);
            string dropTableSql = "drop table Plugin_ArticleContent;";
            conn.Execute(dropTableSql);

            base.Uninstall();
        }
    }
}