using Autofac;
using Autofac.Features.ResolveAnything;
using Autofac.Integration.Mvc;
using AutoMapperConfig;
using Domain;
using Framework.Infrastructure.Concrete;
using Repositories.Core;
using Repositories.Implement;
using Repositories.Interface;
using Services.Implement;
using Services.Interface;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web.Compilation;
using System.Web.Mvc;
using System.Web.Routing;
using PluginHub.Infrastructure;
using PluginHub.Web.Mvc.Routes;
using Framework.Config;
using Framework.Common;
using log4net;
using System.Configuration;
using Framework.Extensions;
using WebUI.Attributes;
using System;
using System.Threading;
using WebUI.Infrastructure;
using WebUI.Controllers;
using System.Web;
using Framework.Mvc.ViewEngines.Templates;

namespace WebUI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AutofacRegister();
            AutoMapperRegister();

            RegisterRoutes(RouteTable.Routes);

            //initialize engine context
            // TODO: The requested service 'PluginHub.Infrastructure.ITypeFinder' has not been registered
            //EngineContext.Initialize(false);

            FrameworkConfig.Register();

            // 开启线程扫描队列将数据取出来写到Lucene.NET中。
            //SearchIndexManager.GetInstance().StartThread();

            #region log4net

            bool enableLog4Net = true;
            // 先尝试查询数据库
            try
            {
                enableLog4Net = Convert.ToInt32(WebSetting.Get("EnableLog")) == 1;
            }
            catch (Exception ex)
            { }
            // 再以配置文件
            // 启用 = 数据库配置启用 且 配置文件启用
            if (enableLog4Net)
            {
                enableLog4Net = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableLog"]);
            }
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

        #region Autofac
        private void AutofacRegister()
        {
            var builder = new ContainerBuilder();

            // 注册MvcApplication程序集中所有的控制器
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // 注意 Framework 所需数据库访问者
            //builder.RegisterType<DBAccessProvider>().As<IDBAccessProvider>();
            //builder.RegisterType<AuthManager>().As<IAuthManager>();
            builder.RegisterType<WorkContext>().As<IWorkContext>();
            builder.RegisterType<TemplateContext>().As<ITemplateContext>();
            builder.RegisterType<TemplateProvider>().As<ITemplateProvider>();
            builder.RegisterType<WebHelper>().As<IWebHelper>().WithParameter("httpContext", HttpContext.Current);
            builder.RegisterType<WebHelper>().As<WebHelper>().WithParameter("httpContext", HttpContext.Current);

            // 注册仓储层服务
            builder.RegisterType<ArticleRepository>().As<IArticleRepository>();
            builder.RegisterType<CommentRepository>().As<ICommentRepository>();
            builder.RegisterType<Comment_DislikeRepository>().As<IComment_DislikeRepository>();
            builder.RegisterType<Comment_LikeRepository>().As<IComment_LikeRepository>();
            builder.RegisterType<BookInfoRepository>().As<IBookInfoRepository>();
            builder.RegisterType<BookInfo_CommentRepository>().As<IBookInfo_CommentRepository>();
            builder.RegisterType<BookInfo_DislikeRepository>().As<IBookInfo_DislikeRepository>();
            builder.RegisterType<BookInfo_LikeRepository>().As<IBookInfo_LikeRepository>();
            builder.RegisterType<BookInfo_ParticipantRepository>().As<IBookInfo_ParticipantRepository>();
            builder.RegisterType<FavoriteRepository>().As<IFavoriteRepository>();
            builder.RegisterType<Favorite_BookInfoRepository>().As<IFavorite_BookInfoRepository>();
            builder.RegisterType<Follower_FollowedRepository>().As<IFollower_FollowedRepository>();
            builder.RegisterType<FunctionInfoRepository>().As<IFunctionInfoRepository>();
            builder.RegisterType<User_BookInfoRepository>().As<IUser_BookInfoRepository>();
            builder.RegisterType<User_BookSectionRepository>().As<IUser_BookSectionRepository>();
            builder.RegisterType<LogInfoRepository>().As<ILogInfoRepository>();
            builder.RegisterType<ParticipantInfoRepository>().As<IParticipantInfoRepository>();
            builder.RegisterType<Role_FunctionRepository>().As<IRole_FunctionRepository>();
            builder.RegisterType<Role_MenuRepository>().As<IRole_MenuRepository>();
            builder.RegisterType<Role_UserRepository>().As<IRole_UserRepository>();
            builder.RegisterType<RoleInfoRepository>().As<IRoleInfoRepository>();
            builder.RegisterType<SearchDetailRepository>().As<ISearchDetailRepository>();
            builder.RegisterType<SearchTotalRepository>().As<ISearchTotalRepository>();
            builder.RegisterType<SettingRepository>().As<ISettingRepository>();
            builder.RegisterType<Sys_MenuRepository>().As<ISys_MenuRepository>();
            builder.RegisterType<ThemeTemplateRepository>().As<IThemeTemplateRepository>();
            builder.RegisterType<UserInfoRepository>().As<IUserInfoRepository>();
            builder.RegisterType<BookSectionRepository>().As<IBookSectionRepository>();
            builder.RegisterType<BookSection_CommentRepository>().As<IBookSection_CommentRepository>();

            // 注册服务层服务
            builder.RegisterType<ArticleService>().As<IArticleService>();
            builder.RegisterType<CommentService>().As<ICommentService>();
            builder.RegisterType<Comment_DislikeService>().As<IComment_DislikeService>();
            builder.RegisterType<Comment_LikeService>().As<IComment_LikeService>();
            builder.RegisterType<BookInfoService>().As<IBookInfoService>();
            builder.RegisterType<BookInfo_CommentService>().As<IBookInfo_CommentService>();
            builder.RegisterType<BookInfo_DislikeService>().As<IBookInfo_DislikeService>();
            builder.RegisterType<BookInfo_LikeService>().As<IBookInfo_LikeService>();
            builder.RegisterType<BookInfo_ParticipantService>().As<IBookInfo_ParticipantService>();
            builder.RegisterType<FavoriteService>().As<IFavoriteService>();
            builder.RegisterType<Favorite_BookInfoService>().As<IFavorite_BookInfoService>();
            builder.RegisterType<Follower_FollowedService>().As<IFollower_FollowedService>();
            builder.RegisterType<FunctionInfoService>().As<IFunctionInfoService>();
            builder.RegisterType<User_BookInfoService>().As<IUser_BookInfoService>();
            builder.RegisterType<User_BookSectionService>().As<IUser_BookSectionService>();
            builder.RegisterType<LogInfoService>().As<ILogInfoService>();
            builder.RegisterType<ParticipantInfoService>().As<IParticipantInfoService>();
            builder.RegisterType<Role_FunctionService>().As<IRole_FunctionService>();
            builder.RegisterType<Role_MenuService>().As<IRole_MenuService>();
            builder.RegisterType<Role_UserService>().As<IRole_UserService>();
            builder.RegisterType<RoleInfoService>().As<IRoleInfoService>();
            builder.RegisterType<SearchDetailService>().As<ISearchDetailService>();
            builder.RegisterType<SearchTotalService>().As<ISearchTotalService>();
            builder.RegisterType<SettingService>().As<ISettingService>();
            builder.RegisterType<Sys_MenuService>().As<ISys_MenuService>();
            builder.RegisterType<ThemeTemplateService>().As<IThemeTemplateService>();
            builder.RegisterType<UserInfoService>().As<IUserInfoService>();
            builder.RegisterType<BookSectionService>().As<IBookSectionService>();
            builder.RegisterType<BookSection_CommentService>().As<IBookSection_CommentService>();

            // TODO: 注册基于接口约束的实体，不知道为什么，改为部分类后就失败了，以前还测试成功
            // 注册基于接口约束的实体
            //var assemblies = BuildManager.GetReferencedAssemblies().Cast<Assembly>()
            //    .Where(
            //        assembly =>
            //            assembly.GetTypes().FirstOrDefault(type => type.GetInterfaces().Contains(typeof(IDependency))) !=
            //            null
            //    );
            //builder.RegisterAssemblyTypes(assemblies.ToArray())
            //    .AsImplementedInterfaces()
            //    .InstancePerDependency();

            // add the Entity Framework context to make sure only one context per request
            //builder.RegisterType<RemDbContext>().InstancePerRequest();
            builder.RegisterType<RemDbContext>().InstancePerRequest();
            builder.Register(c => c.Resolve<RemDbContext>()).As<DbContext>().InstancePerRequest();

            //注册过滤器
            builder.RegisterFilterProvider();

            var container = builder.Build();

            //设置依赖注入解析器
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

        }
        #endregion

        #region AutoMapper
        /// <summary>
        /// AutoMapper的配置初始化
        /// </summary>
        private void AutoMapperRegister()
        {
            new AutoMapperConfig.AutoMapperStartupTask().Execute();
        }
        #endregion

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
            // TODO: Could not load file or assembly 'Autofac.Integration.Mvc, Version=3.3.0.0, Culture=neutral,
            //var routePublisher = EngineContext.Current.Resolve<IRoutePublisher>();
            //routePublisher.RegisterRoutes(routes);

            // 注册文章页的自定义路由
            var route = routes.MapRoute(
                  name: "ArticleCustomUrl",
                  url: "{*cmsurl}",
                  defaults: new { controller = "Article", action = "Index" },
                  constraints: new { cmsurl = new CmsUrlConstraint() },
                  namespaces: new string[] { "WebUI.Controllers" }
             );
            route.DataTokens["area"] = "";

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
