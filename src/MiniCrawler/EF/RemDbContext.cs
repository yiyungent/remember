namespace MiniCrawler.EF
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class RemDbContext : DbContext
    {
        public RemDbContext()
            : base("name=RemModel")
        {
        }

        public virtual DbSet<C__migrationhistory> C__migrationhistory { get; set; }
        public virtual DbSet<article> articles { get; set; }
        public virtual DbSet<cardbox> cardboxes { get; set; }
        public virtual DbSet<cardinfo> cardinfoes { get; set; }
        public virtual DbSet<comment> comments { get; set; }
        public virtual DbSet<comment_dislike> comment_dislike { get; set; }
        public virtual DbSet<comment_like> comment_like { get; set; }
        public virtual DbSet<coursebox> courseboxes { get; set; }
        public virtual DbSet<coursebox_comment> coursebox_comment { get; set; }
        public virtual DbSet<coursebox_dislike> coursebox_dislike { get; set; }
        public virtual DbSet<coursebox_like> coursebox_like { get; set; }
        public virtual DbSet<coursebox_participant> coursebox_participant { get; set; }
        public virtual DbSet<favorite> favorites { get; set; }
        public virtual DbSet<follower_followed> follower_followed { get; set; }
        public virtual DbSet<functioninfo> functioninfoes { get; set; }
        public virtual DbSet<learner_coursebox> learner_coursebox { get; set; }
        public virtual DbSet<learner_videoinfo> learner_videoinfo { get; set; }
        public virtual DbSet<participantinfo> participantinfoes { get; set; }
        public virtual DbSet<roleinfo> roleinfoes { get; set; }
        public virtual DbSet<searchdetail> searchdetails { get; set; }
        public virtual DbSet<searchtotal> searchtotals { get; set; }
        public virtual DbSet<setting> settings { get; set; }
        public virtual DbSet<sys_menu> sys_menu { get; set; }
        public virtual DbSet<themetemplate> themetemplates { get; set; }
        public virtual DbSet<userinfo> userinfoes { get; set; }
        public virtual DbSet<videoinfo> videoinfoes { get; set; }
        public virtual DbSet<videoinfo_comment> videoinfo_comment { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<C__migrationhistory>()
                .Property(e => e.MigrationId)
                .IsUnicode(false);

            modelBuilder.Entity<C__migrationhistory>()
                .Property(e => e.ContextKey)
                .IsUnicode(false);

            modelBuilder.Entity<C__migrationhistory>()
                .Property(e => e.ProductVersion)
                .IsUnicode(false);

            modelBuilder.Entity<article>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<article>()
                .Property(e => e.Content)
                .IsUnicode(false);

            modelBuilder.Entity<article>()
                .Property(e => e.CustomUrl)
                .IsUnicode(false);

            modelBuilder.Entity<cardbox>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<cardbox>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<cardbox>()
                .Property(e => e.PicUrl)
                .IsUnicode(false);

            modelBuilder.Entity<cardbox>()
                .HasMany(e => e.favorites)
                .WithMany(e => e.cardboxes)
                .Map(m => m.ToTable("favorite_cardbox").MapLeftKey("CardBoxId").MapRightKey("FavoriteId"));

            modelBuilder.Entity<cardinfo>()
                .Property(e => e.Content)
                .IsUnicode(false);

            modelBuilder.Entity<comment>()
                .Property(e => e.Content)
                .IsUnicode(false);

            modelBuilder.Entity<comment>()
                .HasMany(e => e.comment1)
                .WithOptional(e => e.comment2)
                .HasForeignKey(e => e.ParentId);

            modelBuilder.Entity<coursebox>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<coursebox>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<coursebox>()
                .Property(e => e.PicUrl)
                .IsUnicode(false);

            modelBuilder.Entity<favorite>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<favorite>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<favorite>()
                .HasMany(e => e.courseboxes)
                .WithMany(e => e.favorites)
                .Map(m => m.ToTable("favorite_coursebox").MapLeftKey("FavoriteId").MapRightKey("CourseBoxId"));

            modelBuilder.Entity<functioninfo>()
                .Property(e => e.AuthKey)
                .IsUnicode(false);

            modelBuilder.Entity<functioninfo>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<functioninfo>()
                .Property(e => e.Remark)
                .IsUnicode(false);

            modelBuilder.Entity<learner_videoinfo>()
                .Property(e => e.LastAccessIp)
                .IsUnicode(false);

            modelBuilder.Entity<participantinfo>()
                .Property(e => e.RoleNames)
                .IsUnicode(false);

            modelBuilder.Entity<participantinfo>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<roleinfo>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<roleinfo>()
                .Property(e => e.Remark)
                .IsUnicode(false);

            modelBuilder.Entity<roleinfo>()
                .HasMany(e => e.functioninfoes)
                .WithMany(e => e.roleinfoes)
                .Map(m => m.ToTable("role_function").MapLeftKey("RoleId").MapRightKey("FunctionId"));

            modelBuilder.Entity<roleinfo>()
                .HasMany(e => e.sys_menu)
                .WithMany(e => e.roleinfoes)
                .Map(m => m.ToTable("role_menu").MapLeftKey("RoleId").MapRightKey("MenuId"));

            modelBuilder.Entity<roleinfo>()
                .HasMany(e => e.userinfoes)
                .WithMany(e => e.roleinfoes)
                .Map(m => m.ToTable("role_user").MapLeftKey("RoleId").MapRightKey("UserId"));

            modelBuilder.Entity<searchdetail>()
                .Property(e => e.KeyWord)
                .IsUnicode(false);

            modelBuilder.Entity<searchtotal>()
                .Property(e => e.KeyWord)
                .IsUnicode(false);

            modelBuilder.Entity<setting>()
                .Property(e => e.SetKey)
                .IsUnicode(false);

            modelBuilder.Entity<setting>()
                .Property(e => e.SetValue)
                .IsUnicode(false);

            modelBuilder.Entity<setting>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<setting>()
                .Property(e => e.Remark)
                .IsUnicode(false);

            modelBuilder.Entity<sys_menu>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<sys_menu>()
                .Property(e => e.ControllerName)
                .IsUnicode(false);

            modelBuilder.Entity<sys_menu>()
                .Property(e => e.ActionName)
                .IsUnicode(false);

            modelBuilder.Entity<sys_menu>()
                .Property(e => e.AreaName)
                .IsUnicode(false);

            modelBuilder.Entity<sys_menu>()
                .HasMany(e => e.functioninfoes)
                .WithOptional(e => e.sys_menu)
                .HasForeignKey(e => e.MenuId);

            modelBuilder.Entity<sys_menu>()
                .HasMany(e => e.sys_menu1)
                .WithOptional(e => e.sys_menu2)
                .HasForeignKey(e => e.ParentId);

            modelBuilder.Entity<themetemplate>()
                .Property(e => e.TemplateName)
                .IsUnicode(false);

            modelBuilder.Entity<themetemplate>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<userinfo>()
                .Property(e => e.UserName)
                .IsUnicode(false);

            modelBuilder.Entity<userinfo>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<userinfo>()
                .Property(e => e.RefreshToken)
                .IsUnicode(false);

            modelBuilder.Entity<userinfo>()
                .Property(e => e.TemplateName)
                .IsUnicode(false);

            modelBuilder.Entity<userinfo>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<userinfo>()
                .Property(e => e.Avatar)
                .IsUnicode(false);

            modelBuilder.Entity<userinfo>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<userinfo>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<userinfo>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<userinfo>()
                .Property(e => e.Remark)
                .IsUnicode(false);

            modelBuilder.Entity<userinfo>()
                .HasMany(e => e.articles)
                .WithOptional(e => e.userinfo)
                .HasForeignKey(e => e.AuthorId);

            modelBuilder.Entity<userinfo>()
                .HasMany(e => e.cardboxes)
                .WithOptional(e => e.userinfo)
                .HasForeignKey(e => e.CreatorId);

            modelBuilder.Entity<userinfo>()
                .HasMany(e => e.comments)
                .WithRequired(e => e.userinfo)
                .HasForeignKey(e => e.AuthorId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<userinfo>()
                .HasMany(e => e.courseboxes)
                .WithOptional(e => e.userinfo)
                .HasForeignKey(e => e.CreatorId);

            modelBuilder.Entity<userinfo>()
                .HasMany(e => e.coursebox_participant)
                .WithOptional(e => e.userinfo)
                .HasForeignKey(e => e.ParticipantId);

            modelBuilder.Entity<userinfo>()
                .HasMany(e => e.favorites)
                .WithOptional(e => e.userinfo)
                .HasForeignKey(e => e.CreatorId);

            modelBuilder.Entity<userinfo>()
                .HasMany(e => e.follower_followed)
                .WithOptional(e => e.userinfo)
                .HasForeignKey(e => e.FollowerId);

            modelBuilder.Entity<userinfo>()
                .HasMany(e => e.follower_followed1)
                .WithOptional(e => e.userinfo1)
                .HasForeignKey(e => e.FollowedId);

            modelBuilder.Entity<userinfo>()
                .HasMany(e => e.learner_coursebox)
                .WithOptional(e => e.userinfo)
                .HasForeignKey(e => e.LearnerId);

            modelBuilder.Entity<userinfo>()
                .HasMany(e => e.learner_videoinfo)
                .WithOptional(e => e.userinfo)
                .HasForeignKey(e => e.LearnerId);

            modelBuilder.Entity<videoinfo>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<videoinfo>()
                .Property(e => e.PlayUrl)
                .IsUnicode(false);

            modelBuilder.Entity<videoinfo>()
                .Property(e => e.SubTitleUrl)
                .IsUnicode(false);

            modelBuilder.Entity<videoinfo>()
                .HasMany(e => e.learner_coursebox)
                .WithOptional(e => e.videoinfo)
                .HasForeignKey(e => e.LastPlayVideoInfoId);
        }
    }
}
