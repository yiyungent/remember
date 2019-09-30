namespace Repositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Article",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(maxLength: 30, unicode: false),
                        Content = c.String(unicode: false, storeType: "text"),
                        CreateTime = c.DateTime(precision: 0),
                        LastUpdateTime = c.DateTime(precision: 0),
                        CustomUrl = c.String(maxLength: 30, unicode: false),
                        AuthorId = c.Int(),
                        DeletedAt = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.UserInfo", t => t.AuthorId)
                .Index(t => t.AuthorId);
            
            CreateTable(
                "dbo.UserInfo",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false, maxLength: 30, unicode: false),
                        Password = c.String(nullable: false, maxLength: 64, unicode: false),
                        RefreshToken = c.String(maxLength: 255, unicode: false),
                        LastLoginTime = c.DateTime(nullable: false, precision: 0),
                        TemplateName = c.String(maxLength: 20, unicode: false),
                        Avatar = c.String(maxLength: 50, unicode: false),
                        Email = c.String(maxLength: 50, unicode: false),
                        Phone = c.String(maxLength: 50, unicode: false),
                        Description = c.String(unicode: false, storeType: "text"),
                        Coin = c.Long(),
                        RegTime = c.DateTime(nullable: false, precision: 0),
                        Remark = c.String(unicode: false, storeType: "text"),
                        DeletedAt = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.CardBox",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 30, unicode: false),
                        Description = c.String(unicode: false, storeType: "text"),
                        PicUrl = c.String(maxLength: 100, unicode: false),
                        CreateTime = c.DateTime(precision: 0),
                        LastUpdateTime = c.DateTime(precision: 0),
                        IsOpen = c.Boolean(),
                        CreatorId = c.Int(),
                        DeletedAt = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.UserInfo", t => t.CreatorId)
                .Index(t => t.CreatorId);
            
            CreateTable(
                "dbo.CardInfo",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Content = c.String(nullable: false, unicode: false, storeType: "text"),
                        CardBoxId = c.Int(),
                        DeletedAt = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CardBox", t => t.CardBoxId)
                .Index(t => t.CardBoxId);
            
            CreateTable(
                "dbo.Comment",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Content = c.String(unicode: false, storeType: "text"),
                        CreateTime = c.DateTime(precision: 0),
                        LastUpdateTime = c.DateTime(precision: 0),
                        LikeNum = c.Int(),
                        DislikeNum = c.Int(),
                        AuthorId = c.Int(nullable: false),
                        ParentId = c.Int(),
                        DeletedAt = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false),
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
                        CreateTime = c.DateTime(precision: 0),
                        CommentId = c.Int(),
                        UserInfoId = c.Int(),
                        DeletedAt = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Comment", t => t.CommentId)
                .ForeignKey("dbo.UserInfo", t => t.UserInfoId)
                .Index(t => t.CommentId)
                .Index(t => t.UserInfoId);
            
            CreateTable(
                "dbo.Comment_Like",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CreateTime = c.DateTime(precision: 0),
                        CommentId = c.Int(),
                        UserInfoId = c.Int(),
                        DeletedAt = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Comment", t => t.CommentId)
                .ForeignKey("dbo.UserInfo", t => t.UserInfoId)
                .Index(t => t.CommentId)
                .Index(t => t.UserInfoId);
            
            CreateTable(
                "dbo.CourseBox",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 200, unicode: false),
                        Description = c.String(unicode: false, storeType: "text"),
                        PicUrl = c.String(maxLength: 100, unicode: false),
                        CreateTime = c.DateTime(precision: 0),
                        LastUpdateTime = c.DateTime(precision: 0),
                        IsOpen = c.Boolean(),
                        StartTime = c.DateTime(precision: 0),
                        EndTime = c.DateTime(precision: 0),
                        LearnDay = c.Int(),
                        LikeNum = c.Int(),
                        DislikeNum = c.Int(),
                        CommentNum = c.Int(),
                        ShareNum = c.Int(),
                        CreatorId = c.Int(),
                        DeletedAt = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.UserInfo", t => t.CreatorId)
                .Index(t => t.CreatorId);
            
            CreateTable(
                "dbo.CourseBox_Comment",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CourseBoxId = c.Int(),
                        CommentId = c.Int(),
                        DeletedAt = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Comment", t => t.CommentId)
                .ForeignKey("dbo.CourseBox", t => t.CourseBoxId)
                .Index(t => t.CourseBoxId)
                .Index(t => t.CommentId);
            
            CreateTable(
                "dbo.CourseBox_Dislike",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CreateTime = c.DateTime(precision: 0),
                        CourseBoxId = c.Int(),
                        UserInfoId = c.Int(),
                        DeletedAt = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CourseBox", t => t.CourseBoxId)
                .ForeignKey("dbo.UserInfo", t => t.UserInfoId)
                .Index(t => t.CourseBoxId)
                .Index(t => t.UserInfoId);
            
            CreateTable(
                "dbo.CourseBox_Like",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CreateTime = c.DateTime(precision: 0),
                        CourseBoxId = c.Int(),
                        UserInfoId = c.Int(),
                        DeletedAt = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CourseBox", t => t.CourseBoxId)
                .ForeignKey("dbo.UserInfo", t => t.UserInfoId)
                .Index(t => t.CourseBoxId)
                .Index(t => t.UserInfoId);
            
            CreateTable(
                "dbo.CourseBox_Participant",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        IsAgreed = c.Boolean(),
                        AgreeTime = c.DateTime(precision: 0),
                        CreateTime = c.DateTime(precision: 0),
                        CourseBoxId = c.Int(),
                        ParticipantId = c.Int(),
                        ParticipantInfoId = c.Int(),
                        DeletedAt = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CourseBox", t => t.CourseBoxId)
                .ForeignKey("dbo.UserInfo", t => t.ParticipantId)
                .ForeignKey("dbo.ParticipantInfo", t => t.ParticipantInfoId)
                .Index(t => t.CourseBoxId)
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
                "dbo.Favorite",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(unicode: false, storeType: "text"),
                        Description = c.String(unicode: false, storeType: "text"),
                        IsOpen = c.Boolean(),
                        CreateTime = c.DateTime(precision: 0),
                        CreatorId = c.Int(),
                        DeletedAt = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.UserInfo", t => t.CreatorId)
                .Index(t => t.CreatorId);
            
            CreateTable(
                "dbo.Favorite_CardBox",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CreateTime = c.DateTime(precision: 0),
                        CardBoxId = c.Int(nullable: false),
                        FavoriteId = c.Int(nullable: false),
                        DeletedAt = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CardBox", t => t.CardBoxId, cascadeDelete: true)
                .ForeignKey("dbo.Favorite", t => t.FavoriteId, cascadeDelete: true)
                .Index(t => t.CardBoxId)
                .Index(t => t.FavoriteId);
            
            CreateTable(
                "dbo.Favorite_CourseBox",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CreateTime = c.DateTime(precision: 0),
                        FavoriteId = c.Int(),
                        CourseBoxId = c.Int(),
                        DeletedAt = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CourseBox", t => t.CourseBoxId)
                .ForeignKey("dbo.Favorite", t => t.FavoriteId)
                .Index(t => t.FavoriteId)
                .Index(t => t.CourseBoxId);
            
            CreateTable(
                "dbo.Follower_Followed",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CreateTime = c.DateTime(precision: 0),
                        FollowerId = c.Int(),
                        FollowedId = c.Int(),
                        DeletedAt = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.UserInfo", t => t.FollowedId)
                .ForeignKey("dbo.UserInfo", t => t.FollowerId)
                .Index(t => t.FollowerId)
                .Index(t => t.FollowedId);
            
            CreateTable(
                "dbo.FunctionInfo",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        AuthKey = c.String(nullable: false, maxLength: 50, unicode: false),
                        Name = c.String(nullable: false, maxLength: 30, unicode: false),
                        Remark = c.String(unicode: false, storeType: "text"),
                        MenuId = c.Int(),
                        DeletedAt = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Sys_Menu", t => t.MenuId)
                .Index(t => t.MenuId);
            
            CreateTable(
                "dbo.Sys_Menu",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100, unicode: false),
                        ControllerName = c.String(maxLength: 100, unicode: false),
                        ActionName = c.String(maxLength: 100, unicode: false),
                        AreaName = c.String(maxLength: 100, unicode: false),
                        SortCode = c.Int(),
                        ParentId = c.Int(),
                        DeletedAt = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Sys_Menu", t => t.ParentId)
                .Index(t => t.ParentId);
            
            CreateTable(
                "dbo.Learner_CourseBox",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        JoinTime = c.DateTime(precision: 0),
                        SpendTime = c.Long(),
                        LastPlayVideoInfoId = c.Int(),
                        LearnerId = c.Int(),
                        CourseBoxId = c.Int(),
                        DeletedAt = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CourseBox", t => t.CourseBoxId)
                .ForeignKey("dbo.VideoInfo", t => t.LastPlayVideoInfoId)
                .ForeignKey("dbo.UserInfo", t => t.LearnerId)
                .Index(t => t.LastPlayVideoInfoId)
                .Index(t => t.LearnerId)
                .Index(t => t.CourseBoxId);
            
            CreateTable(
                "dbo.VideoInfo",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(maxLength: 200, unicode: false),
                        PlayUrl = c.String(nullable: false, unicode: false, storeType: "text"),
                        SubTitleUrl = c.String(unicode: false, storeType: "text"),
                        Duration = c.Long(),
                        Size = c.Long(),
                        Page = c.Int(),
                        CourseBoxId = c.Int(),
                        DeletedAt = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CourseBox", t => t.CourseBoxId)
                .Index(t => t.CourseBoxId);
            
            CreateTable(
                "dbo.Learner_VideoInfo",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        LastAccessIp = c.String(maxLength: 100, unicode: false),
                        LastPlayTime = c.DateTime(precision: 0),
                        ProgressAt = c.Long(),
                        LastPlayAt = c.Long(),
                        LearnerId = c.Int(),
                        VideoInfoId = c.Int(),
                        DeletedAt = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.UserInfo", t => t.LearnerId)
                .ForeignKey("dbo.VideoInfo", t => t.VideoInfoId)
                .Index(t => t.LearnerId)
                .Index(t => t.VideoInfoId);
            
            CreateTable(
                "dbo.LogInfo",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        AccessUserId = c.Int(),
                        AccessIp = c.String(maxLength: 50, unicode: false),
                        Browser = c.String(maxLength: 100, unicode: false),
                        BrowserEngine = c.String(maxLength: 100, unicode: false),
                        OS = c.String(maxLength: 100, unicode: false),
                        Device = c.String(maxLength: 100, unicode: false),
                        Cpu = c.String(maxLength: 100, unicode: false),
                        AccessTime = c.DateTime(precision: 0),
                        JumpTime = c.DateTime(precision: 0),
                        Duration = c.Long(),
                        AccessUrl = c.String(unicode: false, storeType: "text"),
                        RefererUrl = c.String(unicode: false, storeType: "text"),
                        CreateTime = c.DateTime(precision: 0),
                        DeletedAt = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Role_Function",
                c => new
                    {
                        RoleId = c.Int(nullable: false),
                        FunctionId = c.Int(nullable: false),
                        DeletedAt = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.RoleId, t.FunctionId })
                .ForeignKey("dbo.FunctionInfo", t => t.FunctionId, cascadeDelete: true)
                .ForeignKey("dbo.RoleInfo", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.FunctionId);
            
            CreateTable(
                "dbo.RoleInfo",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 30, unicode: false),
                        IsLog = c.Boolean(),
                        Remark = c.String(maxLength: 255, unicode: false),
                        DeletedAt = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Role_Menu",
                c => new
                    {
                        RoleId = c.Int(nullable: false),
                        MenuId = c.Int(nullable: false),
                        DeletedAt = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.RoleId, t.MenuId })
                .ForeignKey("dbo.RoleInfo", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.Sys_Menu", t => t.MenuId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.MenuId);
            
            CreateTable(
                "dbo.Role_User",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                        DeletedAt = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.RoleInfo", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.UserInfo", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.SearchDetail",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        KeyWord = c.String(maxLength: 50, unicode: false),
                        SearchTime = c.DateTime(precision: 0),
                        DeletedAt = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.SearchTotal",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        KeyWord = c.String(maxLength: 50, unicode: false),
                        SearchCount = c.Int(nullable: false),
                        DeletedAt = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Setting",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SetKey = c.String(nullable: false, maxLength: 100, unicode: false),
                        SetValue = c.String(unicode: false, storeType: "text"),
                        Name = c.String(maxLength: 255, unicode: false),
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
                        TemplateName = c.String(nullable: false, maxLength: 100, unicode: false),
                        Title = c.String(nullable: false, maxLength: 100, unicode: false),
                        IsOpen = c.Int(),
                        DeletedAt = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.VideoInfo_Comment",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        VideoInfoId = c.Int(),
                        CommentId = c.Int(),
                        DeletedAt = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Comment", t => t.CommentId)
                .ForeignKey("dbo.VideoInfo", t => t.VideoInfoId)
                .Index(t => t.VideoInfoId)
                .Index(t => t.CommentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VideoInfo_Comment", "VideoInfoId", "dbo.VideoInfo");
            DropForeignKey("dbo.VideoInfo_Comment", "CommentId", "dbo.Comment");
            DropForeignKey("dbo.Role_User", "UserId", "dbo.UserInfo");
            DropForeignKey("dbo.Role_User", "RoleId", "dbo.RoleInfo");
            DropForeignKey("dbo.Role_Menu", "MenuId", "dbo.Sys_Menu");
            DropForeignKey("dbo.Role_Menu", "RoleId", "dbo.RoleInfo");
            DropForeignKey("dbo.Role_Function", "RoleId", "dbo.RoleInfo");
            DropForeignKey("dbo.Role_Function", "FunctionId", "dbo.FunctionInfo");
            DropForeignKey("dbo.Learner_VideoInfo", "VideoInfoId", "dbo.VideoInfo");
            DropForeignKey("dbo.Learner_VideoInfo", "LearnerId", "dbo.UserInfo");
            DropForeignKey("dbo.Learner_CourseBox", "LearnerId", "dbo.UserInfo");
            DropForeignKey("dbo.Learner_CourseBox", "LastPlayVideoInfoId", "dbo.VideoInfo");
            DropForeignKey("dbo.VideoInfo", "CourseBoxId", "dbo.CourseBox");
            DropForeignKey("dbo.Learner_CourseBox", "CourseBoxId", "dbo.CourseBox");
            DropForeignKey("dbo.FunctionInfo", "MenuId", "dbo.Sys_Menu");
            DropForeignKey("dbo.Sys_Menu", "ParentId", "dbo.Sys_Menu");
            DropForeignKey("dbo.Follower_Followed", "FollowerId", "dbo.UserInfo");
            DropForeignKey("dbo.Follower_Followed", "FollowedId", "dbo.UserInfo");
            DropForeignKey("dbo.Favorite_CourseBox", "FavoriteId", "dbo.Favorite");
            DropForeignKey("dbo.Favorite_CourseBox", "CourseBoxId", "dbo.CourseBox");
            DropForeignKey("dbo.Favorite_CardBox", "FavoriteId", "dbo.Favorite");
            DropForeignKey("dbo.Favorite_CardBox", "CardBoxId", "dbo.CardBox");
            DropForeignKey("dbo.Favorite", "CreatorId", "dbo.UserInfo");
            DropForeignKey("dbo.CourseBox_Participant", "ParticipantInfoId", "dbo.ParticipantInfo");
            DropForeignKey("dbo.CourseBox_Participant", "ParticipantId", "dbo.UserInfo");
            DropForeignKey("dbo.CourseBox_Participant", "CourseBoxId", "dbo.CourseBox");
            DropForeignKey("dbo.CourseBox_Like", "UserInfoId", "dbo.UserInfo");
            DropForeignKey("dbo.CourseBox_Like", "CourseBoxId", "dbo.CourseBox");
            DropForeignKey("dbo.CourseBox_Dislike", "UserInfoId", "dbo.UserInfo");
            DropForeignKey("dbo.CourseBox_Dislike", "CourseBoxId", "dbo.CourseBox");
            DropForeignKey("dbo.CourseBox_Comment", "CourseBoxId", "dbo.CourseBox");
            DropForeignKey("dbo.CourseBox_Comment", "CommentId", "dbo.Comment");
            DropForeignKey("dbo.CourseBox", "CreatorId", "dbo.UserInfo");
            DropForeignKey("dbo.Comment_Like", "UserInfoId", "dbo.UserInfo");
            DropForeignKey("dbo.Comment_Like", "CommentId", "dbo.Comment");
            DropForeignKey("dbo.Comment_Dislike", "UserInfoId", "dbo.UserInfo");
            DropForeignKey("dbo.Comment_Dislike", "CommentId", "dbo.Comment");
            DropForeignKey("dbo.Comment", "ParentId", "dbo.Comment");
            DropForeignKey("dbo.Comment", "AuthorId", "dbo.UserInfo");
            DropForeignKey("dbo.CardInfo", "CardBoxId", "dbo.CardBox");
            DropForeignKey("dbo.CardBox", "CreatorId", "dbo.UserInfo");
            DropForeignKey("dbo.Article", "AuthorId", "dbo.UserInfo");
            DropIndex("dbo.VideoInfo_Comment", new[] { "CommentId" });
            DropIndex("dbo.VideoInfo_Comment", new[] { "VideoInfoId" });
            DropIndex("dbo.Role_User", new[] { "RoleId" });
            DropIndex("dbo.Role_User", new[] { "UserId" });
            DropIndex("dbo.Role_Menu", new[] { "MenuId" });
            DropIndex("dbo.Role_Menu", new[] { "RoleId" });
            DropIndex("dbo.Role_Function", new[] { "FunctionId" });
            DropIndex("dbo.Role_Function", new[] { "RoleId" });
            DropIndex("dbo.Learner_VideoInfo", new[] { "VideoInfoId" });
            DropIndex("dbo.Learner_VideoInfo", new[] { "LearnerId" });
            DropIndex("dbo.VideoInfo", new[] { "CourseBoxId" });
            DropIndex("dbo.Learner_CourseBox", new[] { "CourseBoxId" });
            DropIndex("dbo.Learner_CourseBox", new[] { "LearnerId" });
            DropIndex("dbo.Learner_CourseBox", new[] { "LastPlayVideoInfoId" });
            DropIndex("dbo.Sys_Menu", new[] { "ParentId" });
            DropIndex("dbo.FunctionInfo", new[] { "MenuId" });
            DropIndex("dbo.Follower_Followed", new[] { "FollowedId" });
            DropIndex("dbo.Follower_Followed", new[] { "FollowerId" });
            DropIndex("dbo.Favorite_CourseBox", new[] { "CourseBoxId" });
            DropIndex("dbo.Favorite_CourseBox", new[] { "FavoriteId" });
            DropIndex("dbo.Favorite_CardBox", new[] { "FavoriteId" });
            DropIndex("dbo.Favorite_CardBox", new[] { "CardBoxId" });
            DropIndex("dbo.Favorite", new[] { "CreatorId" });
            DropIndex("dbo.CourseBox_Participant", new[] { "ParticipantInfoId" });
            DropIndex("dbo.CourseBox_Participant", new[] { "ParticipantId" });
            DropIndex("dbo.CourseBox_Participant", new[] { "CourseBoxId" });
            DropIndex("dbo.CourseBox_Like", new[] { "UserInfoId" });
            DropIndex("dbo.CourseBox_Like", new[] { "CourseBoxId" });
            DropIndex("dbo.CourseBox_Dislike", new[] { "UserInfoId" });
            DropIndex("dbo.CourseBox_Dislike", new[] { "CourseBoxId" });
            DropIndex("dbo.CourseBox_Comment", new[] { "CommentId" });
            DropIndex("dbo.CourseBox_Comment", new[] { "CourseBoxId" });
            DropIndex("dbo.CourseBox", new[] { "CreatorId" });
            DropIndex("dbo.Comment_Like", new[] { "UserInfoId" });
            DropIndex("dbo.Comment_Like", new[] { "CommentId" });
            DropIndex("dbo.Comment_Dislike", new[] { "UserInfoId" });
            DropIndex("dbo.Comment_Dislike", new[] { "CommentId" });
            DropIndex("dbo.Comment", new[] { "ParentId" });
            DropIndex("dbo.Comment", new[] { "AuthorId" });
            DropIndex("dbo.CardInfo", new[] { "CardBoxId" });
            DropIndex("dbo.CardBox", new[] { "CreatorId" });
            DropIndex("dbo.Article", new[] { "AuthorId" });
            DropTable("dbo.VideoInfo_Comment");
            DropTable("dbo.ThemeTemplate");
            DropTable("dbo.Setting");
            DropTable("dbo.SearchTotal");
            DropTable("dbo.SearchDetail");
            DropTable("dbo.Role_User");
            DropTable("dbo.Role_Menu");
            DropTable("dbo.RoleInfo");
            DropTable("dbo.Role_Function");
            DropTable("dbo.LogInfo");
            DropTable("dbo.Learner_VideoInfo");
            DropTable("dbo.VideoInfo");
            DropTable("dbo.Learner_CourseBox");
            DropTable("dbo.Sys_Menu");
            DropTable("dbo.FunctionInfo");
            DropTable("dbo.Follower_Followed");
            DropTable("dbo.Favorite_CourseBox");
            DropTable("dbo.Favorite_CardBox");
            DropTable("dbo.Favorite");
            DropTable("dbo.ParticipantInfo");
            DropTable("dbo.CourseBox_Participant");
            DropTable("dbo.CourseBox_Like");
            DropTable("dbo.CourseBox_Dislike");
            DropTable("dbo.CourseBox_Comment");
            DropTable("dbo.CourseBox");
            DropTable("dbo.Comment_Like");
            DropTable("dbo.Comment_Dislike");
            DropTable("dbo.Comment");
            DropTable("dbo.CardInfo");
            DropTable("dbo.CardBox");
            DropTable("dbo.UserInfo");
            DropTable("dbo.Article");
        }
    }
}
