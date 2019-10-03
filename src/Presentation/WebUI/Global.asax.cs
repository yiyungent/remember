using Autofac;
using Autofac.Features.ResolveAnything;
using Autofac.Integration.Mvc;
using AutoMapperConfig;
using Domain;
using Framework.Infrastructure.Abstract;
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

            // 注意 Framework 所需数据库访问者
            builder.RegisterType<DBAccessProvider>().As<IDBAccessProvider>();

            // 注册仓储层服务
            builder.RegisterType<ArticleRepository>().As<IArticleRepository>();
            builder.RegisterType<CardBoxRepository>().As<ICardBoxRepository>();
            builder.RegisterType<CardInfoRepository>().As<ICardInfoRepository>();
            builder.RegisterType<CommentRepository>().As<ICommentRepository>();
            builder.RegisterType<Comment_DislikeRepository>().As<IComment_DislikeRepository>();
            builder.RegisterType<Comment_LikeRepository>().As<IComment_LikeRepository>();
            builder.RegisterType<CourseBoxRepository>().As<ICourseBoxRepository>();
            builder.RegisterType<CourseBox_CommentRepository>().As<ICourseBox_CommentRepository>();
            builder.RegisterType<CourseBox_DislikeRepository>().As<ICourseBox_DislikeRepository>();
            builder.RegisterType<CourseBox_LikeRepository>().As<ICourseBox_LikeRepository>();
            builder.RegisterType<CourseBox_ParticipantRepository>().As<ICourseBox_ParticipantRepository>();
            builder.RegisterType<FavoriteRepository>().As<IFavoriteRepository>();
            builder.RegisterType<Favorite_CardBoxRepository>().As<IFavorite_CardBoxRepository>();
            builder.RegisterType<Favorite_CourseBoxRepository>().As<IFavorite_CourseBoxRepository>();
            builder.RegisterType<Follower_FollowedRepository>().As<IFollower_FollowedRepository>();
            builder.RegisterType<FunctionInfoRepository>().As<IFunctionInfoRepository>();
            builder.RegisterType<Learner_CourseBoxRepository>().As<ILearner_CourseBoxRepository>();
            builder.RegisterType<Learner_VideoInfoRepository>().As<ILearner_VideoInfoRepository>();
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
            builder.RegisterType<VideoInfoRepository>().As<IVideoInfoRepository>();
            builder.RegisterType<VideoInfo_CommentRepository>().As<IVideoInfo_CommentRepository>();

            // 注册服务层服务
            builder.RegisterType<ArticleService>().As<IArticleService>();
            builder.RegisterType<CardBoxService>().As<ICardBoxService>();
            builder.RegisterType<CardInfoService>().As<ICardInfoService>();
            builder.RegisterType<CommentService>().As<ICommentService>();
            builder.RegisterType<Comment_DislikeService>().As<IComment_DislikeService>();
            builder.RegisterType<Comment_LikeService>().As<IComment_LikeService>();
            builder.RegisterType<CourseBoxService>().As<ICourseBoxService>();
            builder.RegisterType<CourseBox_CommentService>().As<ICourseBox_CommentService>();
            builder.RegisterType<CourseBox_DislikeService>().As<ICourseBox_DislikeService>();
            builder.RegisterType<CourseBox_LikeService>().As<ICourseBox_LikeService>();
            builder.RegisterType<CourseBox_ParticipantService>().As<ICourseBox_ParticipantService>();
            builder.RegisterType<FavoriteService>().As<IFavoriteService>();
            builder.RegisterType<Favorite_CardBoxService>().As<IFavorite_CardBoxService>();
            builder.RegisterType<Favorite_CourseBoxService>().As<IFavorite_CourseBoxService>();
            builder.RegisterType<Follower_FollowedService>().As<IFollower_FollowedService>();
            builder.RegisterType<FunctionInfoService>().As<IFunctionInfoService>();
            builder.RegisterType<Learner_CourseBoxService>().As<ILearner_CourseBoxService>();
            builder.RegisterType<Learner_VideoInfoService>().As<ILearner_VideoInfoService>();
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
            builder.RegisterType<VideoInfoService>().As<IVideoInfoService>();
            builder.RegisterType<VideoInfo_CommentService>().As<IVideoInfo_CommentService>();

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
