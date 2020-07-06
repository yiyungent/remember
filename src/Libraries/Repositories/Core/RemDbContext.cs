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
            // ��Entity Framework��������Ч�� __MigrationHistory ��
            // ����ÿ��Ч��/��ѯ����Ҫȥ���� __MigrationHistory ���� �˱� �� ContextKey�ֶ�varchar(300) �������Ƶ���
            // �����Specified key was too long; max key length is 767 bytes
            Database.SetInitializer<RemDbContext>(null);

            this.Configuration.AutoDetectChangesEnabled = true;//�Զ�Զ࣬һ�Զ����curd����ʱ��ҪΪtrue

            //this.Configuration.LazyLoadingEnabled = false;

            // ��¼ EF ���ɵ� SQL
            Database.Log = (str) =>
            {
                System.Diagnostics.Debug.WriteLine(str);
            };
        }

        #region Tables

        public virtual DbSet<Article> Article { get; set; }
        public virtual DbSet<Comment> Comment { get; set; }
        public virtual DbSet<Comment_Dislike> Comment_Dislike { get; set; }
        public virtual DbSet<Comment_Like> Comment_Like { get; set; }
        public virtual DbSet<Article_Participant> Article_Participant { get; set; }
        public virtual DbSet<Favorite> Favorite { get; set; }
        public virtual DbSet<Favorite_Article> Favorite_BookInfo { get; set; }
        public virtual DbSet<Follower_Followed> Follower_Followed { get; set; }
        public virtual DbSet<FunctionInfo> FunctionInfo { get; set; }
        public virtual DbSet<LogInfo> LogInfo { get; set; }
        public virtual DbSet<ParticipantInfo> ParticipantInfo { get; set; }
        public virtual DbSet<RoleInfo> RoleInfo { get; set; }
        public virtual DbSet<Setting> Setting { get; set; }
        public virtual DbSet<Sys_Menu> Sys_Menu { get; set; }
        public virtual DbSet<ThemeTemplate> ThemeTemplate { get; set; }
        public virtual DbSet<UserInfo> UserInfo { get; set; }
        public virtual DbSet<Role_Function> Role_Function { get; set; }
        public virtual DbSet<Role_Menu> Role_Menu { get; set; }
        public virtual DbSet<Role_User> Role_User { get; set; }

        #endregion

        #region OnModelCreating
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // ���������Զ�ת��Ϊ����
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            //���ö�Զ�Ĺ�ϵ .Map()�������ڴ洢��ϵ������кͱ�
            /*
             Employees  HasMany��ʵ����������һ�Զ��ϵ����ӦOrdersʵ��               
            WithMany   ����ϵ����Ϊ many:many�����ڹ�ϵ����һ���е������ԡ�
             * MapLeftKey ����������������������ָ���� HasMany ������ָ���ĵ������Եĸ�ʵ�塣
             * MapRightKey ����������������������ָ���� WithMany ������ָ���ĵ������Եĸ�ʵ�塣
             */
            // https://www.cnblogs.com/wer-ltm/p/4944745.html
            //this.HasMany(x => x.Orders).
            //    WithMany(x => x.InvolvedEmployees).
            //    Map(m => m.ToTable("EmployeeOrder").
            //        MapLeftKey("EmployeeId").
            //        MapRightKey("OrderId"));


            // ����һ�Զ࣬�γɶ�Զ࣬���ڵ����Ź�ϵ���и����ֶ�
            modelBuilder.Entity<RoleInfo>()
                .HasMany(m => m.Role_Users)
                .WithRequired(m => m.RoleInfo)
                .HasForeignKey(m => m.RoleInfoId);
            modelBuilder.Entity<UserInfo>()
                .HasMany(m => m.Role_Users)
                .WithRequired(m => m.UserInfo)
                .HasForeignKey(m => m.UserInfoId);

            modelBuilder.Entity<RoleInfo>()
                .HasMany(m => m.Role_Functions)
                .WithRequired(m => m.RoleInfo)
                .HasForeignKey(m => m.RoleInfoId);
            modelBuilder.Entity<FunctionInfo>()
                .HasMany(m => m.Role_Functions)
                .WithRequired(m => m.FunctionInfo)
                .HasForeignKey(m => m.FunctionInfoId);

            modelBuilder.Entity<RoleInfo>()
                .HasMany(m => m.Role_Menus)
                .WithRequired(m => m.RoleInfo)
                .HasForeignKey(m => m.RoleInfoId);
            modelBuilder.Entity<Sys_Menu>()
                .HasMany(m => m.Role_Menus)
                .WithRequired(m => m.Sys_Menu)
                .HasForeignKey(m => m.Sys_MenuId);


            modelBuilder.Entity<Sys_Menu>()
                .HasMany(m => m.Children)
                .WithOptional(m => m.Parent)
                .HasForeignKey(m => m.ParentId);

            modelBuilder.Entity<Comment>()
                .HasMany(m => m.Children)
                .WithOptional(m => m.Parent)
                .HasForeignKey(m => m.ParentId);



            // ������ͨ����
        }
        #endregion

    }
}
