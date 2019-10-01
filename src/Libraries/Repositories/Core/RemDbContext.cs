namespace Repositories.Core
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using Domain.Entities;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.ModelConfiguration.Conventions;

    public partial class RemDbContext : DbContext
    {
        public RemDbContext()
            : base("name=RemDbContext")
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<RemDbContext>());
        }

        #region Tables

        public virtual DbSet<Article> Article { get; set; }
        public virtual DbSet<CardBox> CardBox { get; set; }
        public virtual DbSet<CardInfo> CardInfo { get; set; }
        public virtual DbSet<Comment> Comment { get; set; }
        public virtual DbSet<Comment_Dislike> Comment_Dislike { get; set; }
        public virtual DbSet<Comment_Like> Comment_Like { get; set; }
        public virtual DbSet<CourseBox> CourseBox { get; set; }
        public virtual DbSet<CourseBox_Comment> CourseBox_Comment { get; set; }
        public virtual DbSet<CourseBox_Dislike> CourseBox_Dislike { get; set; }
        public virtual DbSet<CourseBox_Like> CourseBox_Like { get; set; }
        public virtual DbSet<CourseBox_Participant> CourseBox_Participant { get; set; }
        public virtual DbSet<Favorite> Favorite { get; set; }
        public virtual DbSet<Favorite_CourseBox> Favorite_CourseBox { get; set; }
        public virtual DbSet<Follower_Followed> Follower_Followed { get; set; }
        public virtual DbSet<FunctionInfo> FunctionInfo { get; set; }
        public virtual DbSet<Learner_CourseBox> Learner_CourseBox { get; set; }
        public virtual DbSet<Learner_VideoInfo> Learner_VideoInfo { get; set; }
        public virtual DbSet<LogInfo> LogInfo { get; set; }
        public virtual DbSet<ParticipantInfo> ParticipantInfo { get; set; }
        public virtual DbSet<RoleInfo> RoleInfo { get; set; }
        public virtual DbSet<SearchDetail> SearchDetail { get; set; }
        public virtual DbSet<SearchTotal> SearchTotal { get; set; }
        public virtual DbSet<Setting> Settings { get; set; }
        public virtual DbSet<Sys_Menu> Sys_Menu { get; set; }
        public virtual DbSet<ThemeTemplate> ThemeTemplates { get; set; }
        public virtual DbSet<UserInfo> UserInfo { get; set; }
        public virtual DbSet<VideoInfo> VideoInfo { get; set; }
        public virtual DbSet<VideoInfo_Comment> VideoInfo_Comment { get; set; }
        public virtual DbSet<Favorite_CardBox> Favorite_CardBox { get; set; }
        public virtual DbSet<Role_Function> Role_Function { get; set; }
        public virtual DbSet<Role_Menu> Role_Menu { get; set; }
        public virtual DbSet<Role_User> Role_User { get; set; }

        #endregion

        #region OnModelCreating
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // 表名不会自动转换为复数
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            //设置多对多的关系 .Map()配置用于存储关系的外键列和表。
            /*
             Employees  HasMany此实体类型配置一对多关系。对应Orders实体               
            WithMany   将关系配置为 many:many，且在关系的另一端有导航属性。
             * MapLeftKey 配置左外键的列名。左外键指向在 HasMany 调用中指定的导航属性的父实体。
             * MapRightKey 配置右外键的列名。右外键指向在 WithMany 调用中指定的导航属性的父实体。
             */
            // https://www.cnblogs.com/wer-ltm/p/4944745.html
            //this.HasMany(x => x.Orders).
            //    WithMany(x => x.InvolvedEmployees).
            //    Map(m => m.ToTable("EmployeeOrder").
            //        MapLeftKey("EmployeeId").
            //        MapRightKey("OrderId"));


            // 角色 和 操作，菜单，用户 均为 多对多关系
            #region 角色与操作，菜单，用户 多对多
            modelBuilder.Entity<FunctionInfo>()
                   .HasMany(m => m.RoleInfos)
                   .WithMany(m => m.FunctionInfos)
                   .Map(m => m.ToTable("Role_Function")
                   .MapLeftKey("RoleId")
                   .MapRightKey("FunctionId"));

            modelBuilder.Entity<Sys_Menu>()
                .HasMany(m => m.RoleInfos)
                .WithMany(m => m.Sys_Menus)
                .Map(m => m.ToTable("Role_Menu")
                .MapLeftKey("RoleId")
                .MapRightKey("MenuId"));

            modelBuilder.Entity<UserInfo>()
                .HasMany(m => m.RoleInfos)
                .WithMany(m => m.UserInfos)
                .Map(m => m.ToTable("Role_User")
                .MapLeftKey("RoleId")
                .MapRightKey("UserId")); 
            #endregion






        }
        #endregion

    }
}
