using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using System.Web.Routing;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework;
using Core;
using Domain;
using Framework.Config;
using Framework.Common;
using WebUI.Controllers;
using PluginHub.Infrastructure;
using PluginHub.Web.Mvc.Routes;
using WebUI.Infrastructure;
using System.Threading;
using WebUI.Attributes;
using log4net;
using WebUI.Infrastructure.Search;
using System.Configuration;

namespace WebUI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private Container _container;

        protected void Application_Start()
        {
            #region 容器初始化
            try
            {
                // 获取 web.config 配置
                IConfigurationSource source = (IConfigurationSource)System.Configuration.ConfigurationManager.GetSection("ActiveRecord");
                ActiveRecordStarter.Initialize(typeof(UserInfo).Assembly, source);
                _container = Container.Instance;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            #endregion

            RegisterRoutes(RouteTable.Routes);

            //initialize engine context
            EngineContext.Initialize(false);

            // Web Api
            GlobalConfiguration.Configure(WebApiConfig.Register);

            FrameworkConfig.Register();

            // 开启线程扫描队列将数据取出来写到Lucene.NET中。
            SearchIndexManager.GetInstance().StartThread();

            #region log4net
            bool enableLog4Net = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableLog4Net"]);
            if (enableLog4Net)
            {
                log4net.Config.XmlConfigurator.Configure();
                GlobalFilters.Filters.Add(new LogErrorAttribute());
                ThreadPool.QueueUserWorkItem(o =>
                {
                    while (true)
                    {
                        if (LogErrorAttribute.ExceptionQueue.Count > 0)
                        {
                            Exception ex = LogErrorAttribute.ExceptionQueue.Dequeue();
                            if (ex != null)
                            {
                                ILog logger = LogManager.GetLogger("testError");
                                logger.Error(ex.ToString()); //将异常信息写入Log4Net中  
                        }
                            else
                            {
                                Thread.Sleep(50);
                            }
                        }
                        else
                        {
                            Thread.Sleep(50);
                        }
                    }
                });
            }
            #endregion
        }

        #region 注册路由
        public static void RegisterRoutes(RouteCollection routes)
        {
            AreaRegistration.RegisterAllAreas();

            routes.MapRoute(
                name: "Install",
                url: "install/{action}",
                defaults: new { controller = "Install", action = "Index" },
                namespaces: new string[] { "WebUI.Controllers" }
            );

            // register custom routes (plugins, etc)
            var routePublisher = EngineContext.Current.Resolve<IRoutePublisher>();
            routePublisher.RegisterRoutes(routes);

            // 注册文章页的自定义路由
            var route = routes.MapRoute(
                  name: "ArticleCustomUrl",
                  url: "{*cmsurl}",
                  defaults: new { controller = "Article", action = "Page" },
                  constraints: new { cmsurl = new CmsUrlConstraint() },
                  namespaces: new string[] { "WebUI.Areas.Admin.Controllers" }
             );
            route.DataTokens["area"] = "Admin";

            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
        #endregion

        #region 错误处理
        protected void Application_Error(Object sender, EventArgs e)
        {
            var exception = Server.GetLastError();

            //process 404 HTTP errors
            var httpException = exception as HttpException;
            if (httpException != null && httpException.GetHttpCode() == 404)
            {
                var webHelper = new WebHelper(new HttpContextWrapper(Context));
                if (!webHelper.IsStaticResource(this.Request))
                {
                    Response.Clear();
                    Server.ClearError();
                    Response.TrySkipIisCustomErrors = true;

                    // Call target Controller and pass the routeData.
                    IController errorController = new ErrorsController();

                    var routeData = new RouteData();
                    routeData.Values.Add("controller", "Errors");
                    routeData.Values.Add("action", "PageNotFound");

                    errorController.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
                }
            }
        }
        #endregion
    }
}
