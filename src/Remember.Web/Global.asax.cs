using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework;
using Remember.Core;
using Remember.Domain;
using NHibernate;

namespace Remember.Web
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        private Container _container;

        protected void Application_Start()
        {
            // 容器初始化
            try
            {
                // 获取 Web.config 配置
                IConfigurationSource source = (IConfigurationSource)System.Configuration.ConfigurationManager.GetSection("ActiveRecord");
                ActiveRecordStarter.Initialize(typeof(SysUser).Assembly, source);
                _container = Container.Instance;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}