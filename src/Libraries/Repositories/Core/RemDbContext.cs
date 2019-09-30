namespace Repositories.Core
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using Domain.Entities;

    public partial class RemDbContext : DbContext
    {
        public RemDbContext()
            : base("name=RemDbContext")
        {
        }

        public virtual DbSet<Article> Articles { get; set; }
        public virtual DbSet<Cardbox> Cardboxes { get; set; }
        public virtual DbSet<Cardinfo> Cardinfoes { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Comment_Dislike> Comment_Dislike { get; set; }
        public virtual DbSet<Comment_Like> Comment_Like { get; set; }
        public virtual DbSet<CourseBox> CourseBoxes { get; set; }
        public virtual DbSet<CourseBox_Comment> CourseBox_Comment { get; set; }
        public virtual DbSet<CourseBox_Dislike> CourseBox_Dislike { get; set; }
        public virtual DbSet<CourseBox_Like> CourseBox_Like { get; set; }
        public virtual DbSet<CourseBox_Participant> CourseBox_Participant { get; set; }
        public virtual DbSet<Favorite> Favorites { get; set; }
        public virtual DbSet<Favorite_CourseBox> Favorite_CourseBox { get; set; }
        public virtual DbSet<Follower_Followed> Follower_Followed { get; set; }
        public virtual DbSet<FunctionInfo> FunctionInfoes { get; set; }
        public virtual DbSet<Learner_CourseBox> Learner_CourseBox { get; set; }
        public virtual DbSet<Learner_VideoInfo> Learner_VideoInfo { get; set; }
        public virtual DbSet<LogInfo> LogInfoes { get; set; }
        public virtual DbSet<ParticipantInfo> ParticipantInfoes { get; set; }
        public virtual DbSet<RoleInfo> RoleInfoes { get; set; }
        public virtual DbSet<SearchDetail> SearchDetails { get; set; }
        public virtual DbSet<SearchTotal> SearchTotals { get; set; }
        public virtual DbSet<Setting> Settings { get; set; }
        public virtual DbSet<Sys_Menu> Sys_Menu { get; set; }
        public virtual DbSet<ThemeTemplate> ThemeTemplates { get; set; }
        public virtual DbSet<UserInfo> UserInfoes { get; set; }
        public virtual DbSet<VideoInfo> VideoInfoes { get; set; }
        public virtual DbSet<VideoInfo_Comment> VideoInfo_Comment { get; set; }
        public virtual DbSet<Favorite_CardBox> Favorite_CardBox { get; set; }
        public virtual DbSet<Role_Function> Role_Function { get; set; }
        public virtual DbSet<Role_Menu> Role_Menu { get; set; }
        public virtual DbSet<Role_User> Role_User { get; set; }
        public virtual DbSet<User_CardBox_Forread> User_CardBox_Forread { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Article>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<Article>()
                .Property(e => e.Content)
                .IsUnicode(false);

            modelBuilder.Entity<Article>()
                .Property(e => e.CustomUrl)
                .IsUnicode(false);

            modelBuilder.Entity<Cardbox>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Cardbox>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Cardbox>()
                .Property(e => e.PicUrl)
                .IsUnicode(false);

            modelBuilder.Entity<Cardinfo>()
                .Property(e => e.Content)
                .IsUnicode(false);

            modelBuilder.Entity<Comment>()
                .Property(e => e.Content)
                .IsUnicode(false);

            modelBuilder.Entity<CourseBox>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<CourseBox>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<CourseBox>()
                .Property(e => e.PicUrl)
                .IsUnicode(false);

            modelBuilder.Entity<Favorite>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Favorite>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<FunctionInfo>()
                .Property(e => e.AuthKey)
                .IsUnicode(false);

            modelBuilder.Entity<FunctionInfo>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<FunctionInfo>()
                .Property(e => e.Remark)
                .IsUnicode(false);

            modelBuilder.Entity<Learner_VideoInfo>()
                .Property(e => e.LastAccessIp)
                .IsUnicode(false);

            modelBuilder.Entity<LogInfo>()
                .Property(e => e.AccessIp)
                .IsUnicode(false);

            modelBuilder.Entity<LogInfo>()
                .Property(e => e.Browser)
                .IsUnicode(false);

            modelBuilder.Entity<LogInfo>()
                .Property(e => e.BrowserEngine)
                .IsUnicode(false);

            modelBuilder.Entity<LogInfo>()
                .Property(e => e.OS)
                .IsUnicode(false);

            modelBuilder.Entity<LogInfo>()
                .Property(e => e.Device)
                .IsUnicode(false);

            modelBuilder.Entity<LogInfo>()
                .Property(e => e.Cpu)
                .IsUnicode(false);

            modelBuilder.Entity<LogInfo>()
                .Property(e => e.AccessUrl)
                .IsUnicode(false);

            modelBuilder.Entity<LogInfo>()
                .Property(e => e.RefererUrl)
                .IsUnicode(false);

            modelBuilder.Entity<ParticipantInfo>()
                .Property(e => e.RoleNames)
                .IsUnicode(false);

            modelBuilder.Entity<ParticipantInfo>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<RoleInfo>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<RoleInfo>()
                .Property(e => e.Remark)
                .IsUnicode(false);

            modelBuilder.Entity<SearchDetail>()
                .Property(e => e.KeyWord)
                .IsUnicode(false);

            modelBuilder.Entity<SearchTotal>()
                .Property(e => e.KeyWord)
                .IsUnicode(false);

            modelBuilder.Entity<Setting>()
                .Property(e => e.SetKey)
                .IsUnicode(false);

            modelBuilder.Entity<Setting>()
                .Property(e => e.SetValue)
                .IsUnicode(false);

            modelBuilder.Entity<Setting>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Setting>()
                .Property(e => e.Remark)
                .IsUnicode(false);

            modelBuilder.Entity<Sys_Menu>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Sys_Menu>()
                .Property(e => e.ControllerName)
                .IsUnicode(false);

            modelBuilder.Entity<Sys_Menu>()
                .Property(e => e.ActionName)
                .IsUnicode(false);

            modelBuilder.Entity<Sys_Menu>()
                .Property(e => e.AreaName)
                .IsUnicode(false);

            modelBuilder.Entity<ThemeTemplate>()
                .Property(e => e.TemplateName)
                .IsUnicode(false);

            modelBuilder.Entity<ThemeTemplate>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<UserInfo>()
                .Property(e => e.UserName)
                .IsUnicode(false);

            modelBuilder.Entity<UserInfo>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<UserInfo>()
                .Property(e => e.RefreshToken)
                .IsUnicode(false);

            modelBuilder.Entity<UserInfo>()
                .Property(e => e.TemplateName)
                .IsUnicode(false);

            modelBuilder.Entity<UserInfo>()
                .Property(e => e.Avatar)
                .IsUnicode(false);

            modelBuilder.Entity<UserInfo>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<UserInfo>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<UserInfo>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<UserInfo>()
                .Property(e => e.Remark)
                .IsUnicode(false);

            modelBuilder.Entity<VideoInfo>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<VideoInfo>()
                .Property(e => e.PlayUrl)
                .IsUnicode(false);

            modelBuilder.Entity<VideoInfo>()
                .Property(e => e.SubTitleUrl)
                .IsUnicode(false);
        }
    }
}
