using Autofac;
using Autofac.Features.ResolveAnything;
using Autofac.Integration.Mvc;
using AutoMapperConfig;
using Domain;
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

namespace WebUI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            AutofacRegister();
            AutoMapperRegister();
        }

        #region Autofac
        private void AutofacRegister()
        {
            var builder = new ContainerBuilder();

            // 注册MvcApplication程序集中所有的控制器
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // 注册仓储层服务
            builder.RegisterType<ArticleRepository>().As<IArticleRepository>();
            builder.RegisterType<UserInfoRepository>().As<IUserInfoRepository>();

            // 注册服务层服务
            builder.RegisterType<ArticleService>().As<IArticleService>();
            builder.RegisterType<UserInfoService>().As<IUserInfoService>();

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
            new AutoMapperStartupTask().Execute();
        } 
        #endregion
    }
}
