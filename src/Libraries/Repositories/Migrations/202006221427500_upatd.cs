namespace Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class upatd : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Article",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(maxLength: 30, storeType: "nvarchar"),
                        Description = c.String(unicode: false, storeType: "text"),
                        PicUrl = c.String(unicode: false, storeType: "text"),
                        Content = c.String(unicode: false, storeType: "text"),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        LastUpdateTime = c.DateTime(nullable: false, precision: 0),
                        CustomUrl = c.String(unicode: false, storeType: "text"),
                        LikeNum = c.Int(nullable: false),
                        DislikeNum = c.Int(nullable: false),
                        ShareNum = c.Int(nullable: false),
                        CommentNum = c.Int(nullable: false),
                        ArticleStatus = c.Int(nullable: false),
                        CommentStatus = c.Int(nullable: false),
                        OpenStatus = c.Int(nullable: false),
                        AuthorId = c.Int(nullable: false),
                        DeletedAt = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.UserInfo", t => t.AuthorId, cascadeDelete: true)
                .Index(t => t.AuthorId);
            
            CreateTable(
                "dbo.UserInfo",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false, maxLength: 30, storeType: "nvarchar"),
                        Password = c.String(nullable: false, maxLength: 60, storeType: "nvarchar"),
                        LastLoginTime = c.DateTime(nullable: false, precision: 0),
                        LastLoginIp = c.String(maxLength: 60, storeType: "nvarchar"),
                        LastLoginAddress = c.String(maxLength: 60, storeType: "nvarchar"),
                        TemplateName = c.String(unicode: false, storeType: "text"),
                        Avatar = c.String(unicode: false, storeType: "text"),
                        Email = c.String(maxLength: 30, storeType: "nvarchar"),
                        Phone = c.String(maxLength: 30, storeType: "nvarchar"),
                        Description = c.String(unicode: false, storeType: "text"),
                        Credit = c.Int(nullable: false),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        Remark = c.String(unicode: false, storeType: "text"),
                        UserStatus = c.Int(nullable: false),
                        DeletedAt = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Role_User",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CreateTime = c.DateTime(precision: 0),
                        DeletedAt = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false),
                        OperatorId = c.Int(),
                        UserInfoId = c.Int(nullable: false),
                        RoleInfoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.UserInfo", t => t.OperatorId)
                .ForeignKey("dbo.RoleInfo", t => t.RoleInfoId, cascadeDelete: true)
                .ForeignKey("dbo.UserInfo", t => t.UserInfoId, cascadeDelete: true)
                .Index(t => t.OperatorId)
                .Index(t => t.UserInfoId)
                .Index(t => t.RoleInfoId);
            
            CreateTable(
                "dbo.RoleInfo",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 60, storeType: "nvarchar"),
                        Remark = c.String(unicode: false, storeType: "text"),
                        DeletedAt = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Role_Function",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CreateTime = c.DateTime(precision: 0),
                        DeletedAt = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false),
                        OperatorId = c.Int(),
                        RoleInfoId = c.Int(nullable: false),
                        FunctionInfoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.FunctionInfo", t => t.FunctionInfoId, cascadeDelete: true)
                .ForeignKey("dbo.UserInfo", t => t.OperatorId)
                .ForeignKey("dbo.RoleInfo", t => t.RoleInfoId, cascadeDelete: true)
                .Index(t => t.OperatorId)
                .Index(t => t.RoleInfoId)
                .Index(t => t.FunctionInfoId);
            
            CreateTable(
                "dbo.FunctionInfo",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        AuthKey = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        Name = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        Remark = c.String(unicode: false, storeType: "text"),
                        DeletedAt = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false),
                        Sys_MenuId = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Sys_Menu", t => t.Sys_MenuId)
                .Index(t => t.Sys_MenuId);
            
            CreateTable(
                "dbo.Sys_Menu",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, unicode: false, storeType: "text"),
                        Description = c.String(unicode: false, storeType: "text"),
                        Icon = c.String(unicode: false, storeType: "text"),
                        ControllerName = c.String(unicode: false, storeType: "text"),
                        ActionName = c.String(unicode: false, storeType: "text"),
                        AreaName = c.String(unicode: false, storeType: "text"),
                        SortCode = c.Int(nullable: false),
                        ParentId = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Sys_Menu", t => t.ParentId)
                .Index(t => t.ParentId);
            
            CreateTable(
                "dbo.Role_Menu",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CreateTime = c.DateTime(precision: 0),
                        DeletedAt = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false),
                        OperatorId = c.Int(),
                        RoleInfoId = c.Int(nullable: false),
                        Sys_MenuId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.UserInfo", t => t.OperatorId)
                .ForeignKey("dbo.Sys_Menu", t => t.Sys_MenuId, cascadeDelete: true)
                .ForeignKey("dbo.RoleInfo", t => t.RoleInfoId, cascadeDelete: true)
                .Index(t => t.OperatorId)
                .Index(t => t.RoleInfoId)
                .Index(t => t.Sys_MenuId);
            
            CreateTable(
                "dbo.Favorite_Article",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        DeletedAt = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false),
                        FavoriteId = c.Int(nullable: false),
                        ArticleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Article", t => t.ArticleId, cascadeDelete: true)
                .ForeignKey("dbo.Favorite", t => t.FavoriteId, cascadeDelete: true)
                .Index(t => t.FavoriteId)
                .Index(t => t.ArticleId);
            
            CreateTable(
                "dbo.Favorite",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 30, storeType: "nvarchar"),
                        Description = c.String(unicode: false, storeType: "text"),
                        IsOpen = c.Boolean(nullable: false),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        DeletedAt = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.UserInfo", t => t.CreatorId, cascadeDelete: true)
                .Index(t => t.CreatorId);
            
            CreateTable(
                "dbo.Article_Participant",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        IsAgreed = c.Boolean(nullable: false),
                        AgreeTime = c.DateTime(precision: 0),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        DeletedAt = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false),
                        ArticleId = c.Int(nullable: false),
                        ParticipantId = c.Int(nullable: false),
                        ParticipantInfoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Article", t => t.ArticleId, cascadeDelete: true)
                .ForeignKey("dbo.UserInfo", t => t.ParticipantId, cascadeDelete: true)
                .ForeignKey("dbo.ParticipantInfo", t => t.ParticipantInfoId, cascadeDelete: true)
                .Index(t => t.ArticleId)
                .Index(t => t.ParticipantId)
                .Index(t => t.ParticipantInfoId);
            
            CreateTable(
                "dbo.ParticipantInfo",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        RoleNames = c.String(unicode: false, storeType: "text"),
                        Description = c.String(unicode: false, storeType: "text"),
                        DeletedAt = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Comment",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Content = c.String(unicode: false, storeType: "text"),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        LastUpdateTime = c.DateTime(nullable: false, precision: 0),
                        LikeNum = c.Int(nullable: false),
                        DislikeNum = c.Int(nullable: false),
                        DeletedAt = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false),
                        AuthorId = c.Int(nullable: false),
                        ParentId = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.UserInfo", t => t.AuthorId, cascadeDelete: true)
                .ForeignKey("dbo.Comment", t => t.ParentId)
                .Index(t => t.AuthorId)
                .Index(t => t.ParentId);
            
            CreateTable(
                "dbo.Comment_Dislike",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        DeletedAt = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false),
                        CommentId = c.Int(nullable: false),
                        UserInfoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Comment", t => t.CommentId, cascadeDelete: true)
                .ForeignKey("dbo.UserInfo", t => t.UserInfoId, cascadeDelete: true)
                .Index(t => t.CommentId)
                .Index(t => t.UserInfoId);
            
            CreateTable(
                "dbo.Comment_Like",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        DeletedAt = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false),
                        CommentId = c.Int(nullable: false),
                        UserInfoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Comment", t => t.CommentId, cascadeDelete: true)
                .ForeignKey("dbo.UserInfo", t => t.UserInfoId, cascadeDelete: true)
                .Index(t => t.CommentId)
                .Index(t => t.UserInfoId);
            
            CreateTable(
                "dbo.Follower_Followed",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        DeletedAt = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false),
                        FollowerId = c.Int(nullable: false),
                        FollowedId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.UserInfo", t => t.FollowedId, cascadeDelete: true)
                .ForeignKey("dbo.UserInfo", t => t.FollowerId, cascadeDelete: true)
                .Index(t => t.FollowerId)
                .Index(t => t.FollowedId);
            
            CreateTable(
                "dbo.LogInfo",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        AccessUserId = c.Int(nullable: false),
                        IdCode = c.String(unicode: false, storeType: "text"),
                        AccessIp = c.String(maxLength: 30, storeType: "nvarchar"),
                        AccessCity = c.String(unicode: false, storeType: "text"),
                        UserAgent = c.String(unicode: false, storeType: "text"),
                        Browser = c.String(maxLength: 30, storeType: "nvarchar"),
                        BrowserEngine = c.String(maxLength: 30, storeType: "nvarchar"),
                        OS = c.String(maxLength: 30, storeType: "nvarchar"),
                        Device = c.String(maxLength: 30, storeType: "nvarchar"),
                        Cpu = c.String(maxLength: 30, storeType: "nvarchar"),
                        VisitorInfo = c.String(unicode: false, storeType: "text"),
                        ClickCount = c.Int(nullable: false),
                        AccessTime = c.DateTime(nullable: false, precision: 0),
                        JumpTime = c.DateTime(nullable: false, precision: 0),
                        Duration = c.Long(nullable: false),
                        AccessUrl = c.String(unicode: false, storeType: "text"),
                        RefererUrl = c.String(unicode: false, storeType: "text"),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        DeletedAt = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.SearchDetail",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        KeyWord = c.String(unicode: false, storeType: "text"),
                        SearchTime = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.SearchTotal",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        KeyWord = c.String(unicode: false, storeType: "text"),
                        SearchCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Setting",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SetKey = c.String(nullable: false, unicode: false, storeType: "text"),
                        SetValue = c.String(unicode: false, storeType: "text"),
                        Name = c.String(unicode: false, storeType: "text"),
                        Remark = c.String(unicode: false, storeType: "text"),
                        DeletedAt = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ThemeTemplate",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TemplateName = c.String(nullable: false, unicode: false, storeType: "text"),
                        Title = c.String(nullable: false, unicode: false, storeType: "text"),
                        IsOpen = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Follower_Followed", "FollowerId", "dbo.UserInfo");
            DropForeignKey("dbo.Follower_Followed", "FollowedId", "dbo.UserInfo");
            DropForeignKey("dbo.Comment_Like", "UserInfoId", "dbo.UserInfo");
            DropForeignKey("dbo.Comment_Like", "CommentId", "dbo.Comment");
            DropForeignKey("dbo.Comment_Dislike", "UserInfoId", "dbo.UserInfo");
            DropForeignKey("dbo.Comment_Dislike", "CommentId", "dbo.Comment");
            DropForeignKey("dbo.Comment", "ParentId", "dbo.Comment");
            DropForeignKey("dbo.Comment", "AuthorId", "dbo.UserInfo");
            DropForeignKey("dbo.Article_Participant", "ParticipantInfoId", "dbo.ParticipantInfo");
            DropForeignKey("dbo.Article_Participant", "ParticipantId", "dbo.UserInfo");
            DropForeignKey("dbo.Article_Participant", "ArticleId", "dbo.Article");
            DropForeignKey("dbo.Favorite_Article", "FavoriteId", "dbo.Favorite");
            DropForeignKey("dbo.Favorite", "CreatorId", "dbo.UserInfo");
            DropForeignKey("dbo.Favorite_Article", "ArticleId", "dbo.Article");
            DropForeignKey("dbo.Article", "AuthorId", "dbo.UserInfo");
            DropForeignKey("dbo.Role_User", "UserInfoId", "dbo.UserInfo");
            DropForeignKey("dbo.Role_User", "RoleInfoId", "dbo.RoleInfo");
            DropForeignKey("dbo.Role_Menu", "RoleInfoId", "dbo.RoleInfo");
            DropForeignKey("dbo.Role_Function", "RoleInfoId", "dbo.RoleInfo");
            DropForeignKey("dbo.Role_Function", "OperatorId", "dbo.UserInfo");
            DropForeignKey("dbo.FunctionInfo", "Sys_MenuId", "dbo.Sys_Menu");
            DropForeignKey("dbo.Role_Menu", "Sys_MenuId", "dbo.Sys_Menu");
            DropForeignKey("dbo.Role_Menu", "OperatorId", "dbo.UserInfo");
            DropForeignKey("dbo.Sys_Menu", "ParentId", "dbo.Sys_Menu");
            DropForeignKey("dbo.Role_Function", "FunctionInfoId", "dbo.FunctionInfo");
            DropForeignKey("dbo.Role_User", "OperatorId", "dbo.UserInfo");
            DropIndex("dbo.Follower_Followed", new[] { "FollowedId" });
            DropIndex("dbo.Follower_Followed", new[] { "FollowerId" });
            DropIndex("dbo.Comment_Like", new[] { "UserInfoId" });
            DropIndex("dbo.Comment_Like", new[] { "CommentId" });
            DropIndex("dbo.Comment_Dislike", new[] { "UserInfoId" });
            DropIndex("dbo.Comment_Dislike", new[] { "CommentId" });
            DropIndex("dbo.Comment", new[] { "ParentId" });
            DropIndex("dbo.Comment", new[] { "AuthorId" });
            DropIndex("dbo.Article_Participant", new[] { "ParticipantInfoId" });
            DropIndex("dbo.Article_Participant", new[] { "ParticipantId" });
            DropIndex("dbo.Article_Participant", new[] { "ArticleId" });
            DropIndex("dbo.Favorite", new[] { "CreatorId" });
            DropIndex("dbo.Favorite_Article", new[] { "ArticleId" });
            DropIndex("dbo.Favorite_Article", new[] { "FavoriteId" });
            DropIndex("dbo.Role_Menu", new[] { "Sys_MenuId" });
            DropIndex("dbo.Role_Menu", new[] { "RoleInfoId" });
            DropIndex("dbo.Role_Menu", new[] { "OperatorId" });
            DropIndex("dbo.Sys_Menu", new[] { "ParentId" });
            DropIndex("dbo.FunctionInfo", new[] { "Sys_MenuId" });
            DropIndex("dbo.Role_Function", new[] { "FunctionInfoId" });
            DropIndex("dbo.Role_Function", new[] { "RoleInfoId" });
            DropIndex("dbo.Role_Function", new[] { "OperatorId" });
            DropIndex("dbo.Role_User", new[] { "RoleInfoId" });
            DropIndex("dbo.Role_User", new[] { "UserInfoId" });
            DropIndex("dbo.Role_User", new[] { "OperatorId" });
            DropIndex("dbo.Article", new[] { "AuthorId" });
            DropTable("dbo.ThemeTemplate");
            DropTable("dbo.Setting");
            DropTable("dbo.SearchTotal");
            DropTable("dbo.SearchDetail");
            DropTable("dbo.LogInfo");
            DropTable("dbo.Follower_Followed");
            DropTable("dbo.Comment_Like");
            DropTable("dbo.Comment_Dislike");
            DropTable("dbo.Comment");
            DropTable("dbo.ParticipantInfo");
            DropTable("dbo.Article_Participant");
            DropTable("dbo.Favorite");
            DropTable("dbo.Favorite_Article");
            DropTable("dbo.Role_Menu");
            DropTable("dbo.Sys_Menu");
            DropTable("dbo.FunctionInfo");
            DropTable("dbo.Role_Function");
            DropTable("dbo.RoleInfo");
            DropTable("dbo.Role_User");
            DropTable("dbo.UserInfo");
            DropTable("dbo.Article");
        }
    }
}
