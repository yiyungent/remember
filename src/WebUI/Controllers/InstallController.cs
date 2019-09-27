using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Castle.ActiveRecord;
using Core;
using Domain;
using Service;
using NHibernate.Criterion;
using Framework.Common;
using WebUI.Extensions;
using Jdenticon;
using Common;

namespace WebUI.Controllers
{
    public class InstallController : Controller
    {
        #region 安装首页
        public ActionResult Index()
        {
            return View();
        }
        #endregion

        #region 输出消息
        private void ShowMessage(string message)
        {
            Response.Write(message + "<br>");
            Response.Flush();
        }
        #endregion

        #region 开始安装
        public ViewResult StartInstall()
        {
            CreateDB();

            return View("Index");
        }
        #endregion

        #region 创建数据库表结构
        private void CreateSchema()
        {
            try
            {
                ShowMessage("开始创建数据库表结构");
                ActiveRecordStarter.CreateSchema();
                ShowMessage("成功");
            }
            catch (Exception ex)
            {
                ShowMessage("失败");
                ShowMessage(ex.Message);
            }
        }
        #endregion

        #region 创建数据库
        private void CreateDB()
        {
            CreateSchema();

            InitSetting();
            InitThemeTemplate();
            InitSys_Menu();
            InitFunction();
            InitRole();
            InitUserInfo();
            InitArticle();

            InitFollower_Followed();
            InitFavorite();
            InitCourseBox();
            InitVideoInfo();
            InitLearner_CourseBox();
            InitLearner_VideoInfo();
            InitComment();
        }
        #endregion


        #region 初始化设置
        private void InitSetting()
        {
            try
            {
                ShowMessage("初始化设置");

                string findPwd_MailContent = "";
                findPwd_MailContent += "<p>";
                findPwd_MailContent += "&nbsp; &nbsp;您正在进行找回登录密码的重置操作，本次请求的邮件验证码是：";
                findPwd_MailContent += "<strong>{{VCode}}</strong>";
                findPwd_MailContent += "(为了保证你账号的安全性，请在5分钟内完成设置)。本验证码5分钟内有效，请及时输入。";
                findPwd_MailContent += "<br><br>";
                findPwd_MailContent += "&nbsp; &nbsp;为保证账号安全，请勿泄漏此验证码。";
                findPwd_MailContent += "<br>";
                findPwd_MailContent += "&nbsp; &nbsp;如非本人操作，及时检查账号或";
                findPwd_MailContent += "<a href='#' target='_blank'>联系在线客服</a>";
                findPwd_MailContent += "<br>";
                findPwd_MailContent += "&nbsp; &nbsp;祝在【TES】收获愉快！";
                findPwd_MailContent += "<br><br>";
                findPwd_MailContent += "&nbsp; &nbsp;（这是一封自动发送的邮件，请不要直接回复）";
                findPwd_MailContent += "</p>";

                Dictionary<string, string> dic = new Dictionary<string, string>()
                {
                    { "DefaultTemplateName", "Blue" },

                    { "WebApiSite", "http://api.tikotiko.fun/" },
                    { "WebApiTitle","remember" },
                    { "WebApiDesc", "remember是xx推出的专业在线教育平台，聚合大量优质教育机构和名师，下设职业培训、公务员考试、托福雅思、考证考级、英语口语、中小学教育等众多在线学习精品课程，打造老师在线上课教学、学生及时互动学习的课堂。"},
                    { "WebApiKeywords", "" },
                    { "WebApiStat", "" },

                    { "WebUISite", "http://www.tikotiko.fun/" },
                    { "WebUITitle", "remember - 在线学习" },
                    { "WebUIDesc", "remember是xx推出的专业在线教育平台，聚合大量优质教育机构和名师，下设职业培训、公务员考试、托福雅思、考证考级、英语口语、中小学教育等众多在线学习精品课程，打造老师在线上课教学、学生及时互动学习的课堂。"},
                    { "WebUIKeywords", "" },
                    { "WebUIStat", "" },

                     { "MailUserName", "" },
                    { "MailPassword", "" },
                     { "MailDisplayName", "" },
                    { "MailDisplayAddress", "" },
                    { "SmtpHost", "smtp.qq.com" },
                    { "SmtpPort", "25" },
                    { "SmtpEnableSsl", "1" },

                    { "EnableRedisSession", "1" },
                    { "EnableLog", "0" },

                    { "FindPwd_MailSubject", "【{{WebUITitle}}】账号安全中心-找回登录密码-{{ReceiveMail}}正在尝试找回密码"},
                    { "FindPwd_MailContent", findPwd_MailContent }
                };

                foreach (var keyValue in dic)
                {
                    Container.Instance.Resolve<SettingService>().Create(new Setting()
                    {
                        SetKey = keyValue.Key,
                        SetValue = keyValue.Value
                    });
                }

                ShowMessage("成功");
            }
            catch (Exception ex)
            {
                ShowMessage("失败");
            }
        }
        #endregion

        #region 初始化主题模板
        private void InitThemeTemplate()
        {
            try
            {
                ShowMessage("初始化主题模板");

                Container.Instance.Resolve<ThemeTemplateService>().Create(new ThemeTemplate
                {
                    TemplateName = "Blue",
                    Title = "经典蓝",
                    IsOpen = 1
                });

                ShowMessage("成功");
            }
            catch (Exception ex)
            {
                ShowMessage("失败");
            }
        }
        #endregion

        #region 初始化系统菜单表
        private void InitSys_Menu()
        {
            try
            {
                ShowMessage("开始初始化系统菜单表");

                #region 一级菜单
                Container.Instance.Resolve<Sys_MenuService>().Create(new Sys_Menu()
                {
                    Name = "全局",
                    SortCode = 10,
                });
                Container.Instance.Resolve<Sys_MenuService>().Create(new Sys_Menu()
                {
                    Name = "界面",
                    SortCode = 20,
                });
                Container.Instance.Resolve<Sys_MenuService>().Create(new Sys_Menu()
                {
                    Name = "用户",
                    SortCode = 30,
                });
                Container.Instance.Resolve<Sys_MenuService>().Create(new Sys_Menu()
                {
                    Name = "运营",
                    SortCode = 40,
                });
                Container.Instance.Resolve<Sys_MenuService>().Create(new Sys_Menu()
                {
                    Name = "应用",
                    SortCode = 50,
                });
                Container.Instance.Resolve<Sys_MenuService>().Create(new Sys_Menu()
                {
                    Name = "课程",
                    ControllerName = "CourseBox",
                    ActionName = "Index",
                    AreaName = "Admin",
                    SortCode = 60,
                });
                #endregion

                #region 二级菜单

                // 一级菜单项---二级菜单的父菜单项
                Sys_Menu parentMenu = null;

                #region 全局的二级菜单
                parentMenu = Container.Instance.Resolve<Sys_MenuService>().Query(new List<ICriterion>
                {
                    Expression.Eq("Name", "全局")
                }).FirstOrDefault();

                Container.Instance.Resolve<Sys_MenuService>().Create(new Sys_Menu()
                {
                    Name = "站点设置",
                    ControllerName = "Setting",
                    ActionName = "Index",
                    AreaName = "Admin",
                    ParentMenu = parentMenu,
                    SortCode = 10,
                });
                #endregion

                #region 界面的二级菜单
                parentMenu = Container.Instance.Resolve<Sys_MenuService>().Query(new List<ICriterion>
                {
                    Expression.Eq("Name", "界面")
                }).FirstOrDefault();

                Container.Instance.Resolve<Sys_MenuService>().Create(new Sys_Menu()
                {
                    Name = "菜单管理",
                    ControllerName = "SysMenu",
                    ActionName = "Index",
                    AreaName = "Admin",
                    ParentMenu = parentMenu,
                    SortCode = 10,
                });
                Container.Instance.Resolve<Sys_MenuService>().Create(new Sys_Menu()
                {
                    Name = "主题模板",
                    ControllerName = "ThemeTemplate",
                    ActionName = "Index",
                    AreaName = "Admin",
                    ParentMenu = parentMenu,
                    SortCode = 20,
                });
                #endregion

                #region 用户的二级菜单
                parentMenu = Container.Instance.Resolve<Sys_MenuService>().Query(new List<ICriterion>
                {
                    Expression.Eq("Name", "用户")
                }).FirstOrDefault();

                Container.Instance.Resolve<Sys_MenuService>().Create(new Sys_Menu()
                {
                    Name = "用户管理",
                    ControllerName = "UserInfo",
                    ActionName = "Index",
                    AreaName = "Admin",
                    ParentMenu = parentMenu,
                    SortCode = 10,
                });
                Container.Instance.Resolve<Sys_MenuService>().Create(new Sys_Menu()
                {
                    Name = "角色管理",
                    ControllerName = "RoleInfo",
                    ActionName = "Index",
                    AreaName = "Admin",
                    ParentMenu = parentMenu,
                    SortCode = 20,
                });
                #endregion

                #region 运营的二级菜单
                parentMenu = Container.Instance.Resolve<Sys_MenuService>().Query(new List<ICriterion>
                {
                    Expression.Eq("Name", "运营")
                }).FirstOrDefault();

                Container.Instance.Resolve<Sys_MenuService>().Create(new Sys_Menu()
                {
                    Name = "站点公告",
                    ControllerName = "Article",
                    ActionName = "Index",
                    AreaName = "Admin",
                    ParentMenu = parentMenu,
                    SortCode = 10,
                });
                #endregion

                #region 应用的二级菜单
                parentMenu = Container.Instance.Resolve<Sys_MenuService>().Query(new List<ICriterion>
                {
                    Expression.Eq("Name", "应用")
                }).FirstOrDefault();

                Container.Instance.Resolve<Sys_MenuService>().Create(new Sys_Menu()
                {
                    Name = "插件管理",
                    ControllerName = "Plugin",
                    ActionName = "Index",
                    AreaName = "Admin",
                    ParentMenu = parentMenu,
                    SortCode = 10,
                });
                #endregion

                #endregion

                ShowMessage("成功");
            }
            catch (Exception ex)
            {
                ShowMessage("失败");
                ShowMessage(ex.Message);
            }
        }
        #endregion

        #region 初始化操作表
        /// <summary>
        /// 放入此表，即此action需要权限验证
        /// </summary>
        private void InitFunction()
        {
            try
            {
                ShowMessage("开始初始化操作表");
                // ID: 1
                // 特殊抽象操作---决定是否能进入管理中心
                // 只要拥有系统菜单下的任一操作权限 --> 就会拥有此对应系统菜单项 --> 就会拥有进入管理中心，即拥有此抽象的特殊操作权限(Admin.Home.Index  (后台)管理中心(框架))
                Container.Instance.Resolve<FunctionInfoService>().Create(new FunctionInfo
                {
                    AuthKey = "Admin.Home.Index",
                    Name = "(后台)管理中心(框架)"
                });

                IList<ICriterion> qryWhere = new List<ICriterion>();

                #region 这些 都具有 "新增", "修改", "删除", "查看"
                string[] names = { "用户管理", "角色管理", "菜单管理", "操作管理" };
                IList<Sys_Menu> allMenu = Container.Instance.Resolve<Sys_MenuService>().GetAll();
                IList<int> idList = allMenu.Where(m => names.Contains(m.Name)).Select(m => m.ID).ToList();
                qryWhere.Add(Expression.In("ID", idList.ToArray()));
                IList<Sys_Menu> findMenuList = Container.Instance.Resolve<Sys_MenuService>().Query(qryWhere);
                string[] funcNames = { "新增", "修改", "删除", "查看" };
                string[] actionNames = { "Create", "Edit", "Delete", "Detail", "Index" };
                foreach (var menu in findMenuList)
                {
                    for (int i = 0; i < actionNames.Length; i++)
                    {
                        if (i == actionNames.Length - 1)
                        {
                            Container.Instance.Resolve<FunctionInfoService>().Create(new FunctionInfo()
                            {
                                Name = menu.Name + "-列表",
                                AuthKey = menu.AreaName + "." + menu.ControllerName + "." + actionNames[i],
                                Sys_Menu = menu
                            });
                        }
                        else
                        {
                            Container.Instance.Resolve<FunctionInfoService>().Create(new FunctionInfo()
                            {
                                Name = menu.Name + "-" + funcNames[i],
                                AuthKey = menu.AreaName + "." + menu.ControllerName + "." + actionNames[i],
                                Sys_Menu = menu
                            });
                        }
                    }
                }
                #endregion

                #region 文章管理
                Sys_Menu article_Sys_Menu = Container.Instance.Resolve<Sys_MenuService>().Query(new List<ICriterion> { Expression.Eq("ControllerName", "Article") }).FirstOrDefault();
                Container.Instance.Resolve<FunctionInfoService>().Create(new FunctionInfo
                {
                    AuthKey = "Admin.Article.Create",
                    Name = "文章管理-创建",
                    Sys_Menu = article_Sys_Menu
                });
                Container.Instance.Resolve<FunctionInfoService>().Create(new FunctionInfo
                {
                    AuthKey = "Admin.Article.Index",
                    Name = "文章管理-列表",
                    Sys_Menu = article_Sys_Menu
                });
                Container.Instance.Resolve<FunctionInfoService>().Create(new FunctionInfo
                {
                    AuthKey = "Admin.Article.Edit",
                    Name = "文章管理-编辑",
                    Sys_Menu = article_Sys_Menu
                });
                Container.Instance.Resolve<FunctionInfoService>().Create(new FunctionInfo
                {
                    AuthKey = "Admin.Article.Delete",
                    Name = "文章管理-删除",
                    Sys_Menu = article_Sys_Menu
                });
                #endregion

                #region 角色管理
                // 角色RoleInfo菜单 增加授权操作
                Sys_Menu roleInfo_Sys_Menu = Container.Instance.Resolve<Sys_MenuService>().Query(new List<ICriterion> { Expression.Eq("ControllerName", "RoleInfo") }).FirstOrDefault();
                Container.Instance.Resolve<FunctionInfoService>().Create(new FunctionInfo
                {
                    AuthKey = "Admin.RoleInfo.AssignPower",
                    Name = "角色管理-授权",
                    Sys_Menu = roleInfo_Sys_Menu
                });
                #endregion

                #region 主题模板 ThemeTemplate
                Sys_Menu themeTemplate_Sys_Menu = Container.Instance.Resolve<Sys_MenuService>().Query(new List<ICriterion> { Expression.Eq("ControllerName", "ThemeTemplate") }).FirstOrDefault();
                Container.Instance.Resolve<FunctionInfoService>().Create(new FunctionInfo
                {
                    AuthKey = "Admin.ThemeTemplate.Index",
                    Name = "主题模板-列表",
                    Sys_Menu = themeTemplate_Sys_Menu
                });
                Container.Instance.Resolve<FunctionInfoService>().Create(new FunctionInfo
                {
                    AuthKey = "Admin.ThemeTemplate.UploadTemplate",
                    Name = "主题模板-上传本地主题模板页面",
                    Sys_Menu = themeTemplate_Sys_Menu
                });
                Container.Instance.Resolve<FunctionInfoService>().Create(new FunctionInfo
                {
                    AuthKey = "Admin.ThemeTemplate.UploadTemplateFile",
                    Name = "主题模板-上传本地主题模板",
                    Sys_Menu = themeTemplate_Sys_Menu
                });
                Container.Instance.Resolve<FunctionInfoService>().Create(new FunctionInfo
                {
                    AuthKey = "Admin.ThemeTemplate.InstallZip",
                    Name = "主题模板-安装",
                    Sys_Menu = themeTemplate_Sys_Menu
                });
                Container.Instance.Resolve<FunctionInfoService>().Create(new FunctionInfo
                {
                    AuthKey = "Admin.ThemeTemplate.Uninstall",
                    Name = "主题模板-卸载",
                    Sys_Menu = themeTemplate_Sys_Menu
                });
                Container.Instance.Resolve<FunctionInfoService>().Create(new FunctionInfo
                {
                    AuthKey = "Admin.ThemeTemplate.DeleteInstallZip",
                    Name = "主题模板-删除安装包",
                    Sys_Menu = themeTemplate_Sys_Menu
                });
                Container.Instance.Resolve<FunctionInfoService>().Create(new FunctionInfo
                {
                    AuthKey = "Admin.ThemeTemplate.OpenClose",
                    Name = "主题模板-启用禁用",
                    Sys_Menu = themeTemplate_Sys_Menu
                });
                Container.Instance.Resolve<FunctionInfoService>().Create(new FunctionInfo
                {
                    AuthKey = "Admin.ThemeTemplate.SetDefault",
                    Name = "主题模板-设置为默认模板",
                    Sys_Menu = themeTemplate_Sys_Menu
                });
                #endregion

                #region 单独操作（不放在系统菜单下）
                Container.Instance.Resolve<FunctionInfoService>().Create(new FunctionInfo
                {
                    AuthKey = "Admin.ThemeTemplate.SelectTemplate",
                    Name = "主题模板-选择模板",
                    Sys_Menu = null
                });
                #endregion

                #region 全局-站点设置
                Sys_Menu setting_Sys_Menu = Container.Instance.Resolve<Sys_MenuService>().Query(new List<ICriterion> { Expression.Eq("ControllerName", "Setting") }).FirstOrDefault();
                Container.Instance.Resolve<FunctionInfoService>().Create(new FunctionInfo
                {
                    AuthKey = "Admin.Setting.Index",
                    Name = "站点设置-常规",
                    Sys_Menu = setting_Sys_Menu
                });
                Container.Instance.Resolve<FunctionInfoService>().Create(new FunctionInfo
                {
                    AuthKey = "Admin.Setting.WebApi",
                    Name = "站点设置-WebApi",
                    Sys_Menu = setting_Sys_Menu
                });
                Container.Instance.Resolve<FunctionInfoService>().Create(new FunctionInfo
                {
                    AuthKey = "Admin.Setting.FindPwd",
                    Name = "站点设置-找回密码",
                    Sys_Menu = setting_Sys_Menu
                });
                Container.Instance.Resolve<FunctionInfoService>().Create(new FunctionInfo
                {
                    AuthKey = "Admin.Setting.SysEmail",
                    Name = "站点设置-系统邮箱",
                    Sys_Menu = setting_Sys_Menu
                });
                Container.Instance.Resolve<FunctionInfoService>().Create(new FunctionInfo
                {
                    AuthKey = "Admin.Setting.Advanced",
                    Name = "站点设置-高级",
                    Sys_Menu = setting_Sys_Menu
                });
                #endregion

                #region 课程
                Sys_Menu courseBox_Sys_Menu = Container.Instance.Resolve<Sys_MenuService>().Query(new List<ICriterion> { Expression.Eq("ControllerName", "CourseBox") }).FirstOrDefault();
                Container.Instance.Resolve<FunctionInfoService>().Create(new FunctionInfo
                {
                    AuthKey = "Admin.CourseBox.Index",
                    Name = "课程-列表",
                    Sys_Menu = courseBox_Sys_Menu
                });
                Container.Instance.Resolve<FunctionInfoService>().Create(new FunctionInfo
                {
                    AuthKey = "Admin.CourseBox.Edit",
                    Name = "课程-编辑",
                    Sys_Menu = courseBox_Sys_Menu
                });
                Container.Instance.Resolve<FunctionInfoService>().Create(new FunctionInfo
                {
                    AuthKey = "Admin.CourseBox.UploadVideo",
                    Name = "课程-上传视频",
                    Sys_Menu = courseBox_Sys_Menu
                });
                Container.Instance.Resolve<FunctionInfoService>().Create(new FunctionInfo
                {
                    AuthKey = "Admin.CourseBox.Delete",
                    Name = "课程-删除",
                    Sys_Menu = courseBox_Sys_Menu
                });
                Container.Instance.Resolve<FunctionInfoService>().Create(new FunctionInfo
                {
                    AuthKey = "Admin.CourseBox.Create",
                    Name = "课程-创建",
                    Sys_Menu = courseBox_Sys_Menu
                });
                Container.Instance.Resolve<FunctionInfoService>().Create(new FunctionInfo
                {
                    AuthKey = "Admin.CourseBox.AddVideo",
                    Name = "视频课件-添加",
                    Sys_Menu = courseBox_Sys_Menu
                });
                Container.Instance.Resolve<FunctionInfoService>().Create(new FunctionInfo
                {
                    AuthKey = "Admin.CourseBox.DeleteVideo",
                    Name = "视频课件-删除",
                    Sys_Menu = courseBox_Sys_Menu
                });
                Container.Instance.Resolve<FunctionInfoService>().Create(new FunctionInfo
                {
                    AuthKey = "Admin.CourseBox.EditVideo",
                    Name = "视频课件-编辑",
                    Sys_Menu = courseBox_Sys_Menu
                });
                #endregion

                ShowMessage("成功");
            }
            catch (Exception ex)
            {
                ShowMessage("失败");
                ShowMessage(ex.Message);
            }
        }
        #endregion

        #region 初始化角色表
        private void InitRole()
        {
            try
            {
                ShowMessage("开始初始化角色表");

                var allMenu = Container.Instance.Resolve<Sys_MenuService>().GetAll();
                var allFunction = Container.Instance.Resolve<FunctionInfoService>().GetAll();

                // 系统组
                #region 系统组
                // 1
                Container.Instance.Resolve<RoleInfoService>().Create(new RoleInfo
                {
                    Name = "超级管理员",
                    Status = 0,
                    Sys_MenuList = allMenu,
                    FunctionInfoList = allFunction
                });
                // 2
                Container.Instance.Resolve<RoleInfoService>().Create(new RoleInfo
                {
                    Name = "站长",
                    Status = 0,
                    Sys_MenuList = allMenu,
                    FunctionInfoList = allFunction
                });
                // 3
                Container.Instance.Resolve<RoleInfoService>().Create(new RoleInfo
                {
                    Name = "副站长",
                    Status = 0,
                    Sys_MenuList = allMenu,
                    FunctionInfoList = allFunction
                });
                // 4
                Container.Instance.Resolve<RoleInfoService>().Create(new RoleInfo
                {
                    Name = "运营",
                    Status = 0,
                    Sys_MenuList = allMenu,
                    FunctionInfoList = allFunction
                });
                // 5
                Container.Instance.Resolve<RoleInfoService>().Create(new RoleInfo
                {
                    Name = "游客",
                    Status = 0,
                    Sys_MenuList = null,
                    FunctionInfoList = null
                });
                #endregion

                // 自定义用户组
                #region 自定义用户组
                // 6
                Container.Instance.Resolve<RoleInfoService>().Create(new RoleInfo
                {
                    Name = "学生",
                    Status = 0,
                    Sys_MenuList = null,
                    FunctionInfoList = null
                });
                // 7
                Container.Instance.Resolve<RoleInfoService>().Create(new RoleInfo
                {
                    Name = "教师",
                    Status = 0,
                    Sys_MenuList = null,
                    FunctionInfoList = null
                });
                #endregion

                ShowMessage("成功");
            }
            catch (Exception ex)
            {
                ShowMessage("失败");
                ShowMessage(ex.Message);
            }
        }
        #endregion

        #region 初始化用户表
        private void InitUserInfo()
        {
            try
            {
                ShowMessage("开始初始化用户表");

                UserInfoService userInfoService = Container.Instance.Resolve<UserInfoService>();

                var allRole = Container.Instance.Resolve<RoleInfoService>().GetAll();

                // 超级管理员 1
                userInfoService.Create(new UserInfo()
                {
                    UserName = "admin",
                    Avatar = ":WebUISite:/assets/images/default-avatar.jpg",
                    Password = EncryptHelper.MD5Encrypt32("admin"),
                    Email = "yiyungent@126.com",
                    Description = "我是超级管理员",
                    Status = 0,
                    RoleInfoList = (from m in allRole where m.ID == 1 select m).ToList(),
                    RegTime = DateTime.Now
                });

                UserInfo admin = userInfoService.Query(new List<ICriterion>
                {
                    Expression.Eq("UserName", "admin")
                }).FirstOrDefault();

                // 正式会员 50
                string userName = "", avatar = "";
                int randomNum = 6;
                for (int i = 0; i < 50; i++)
                {
                    userName = GetRandom.GetRandomName();

                    Identicon
                   .FromValue(EncryptHelper.MD5Encrypt32(userName), size: 100)
                   .SaveAsPng(Server.MapPath("/upload/images/avatars/" + (i + 2).ToString() + ".png"));

                    randomNum = new Random().Next(6, 7);
                    userInfoService.Create(new UserInfo
                    {
                        UserName = userName,
                        Description = $"我是会员-{i + 1}",
                        Avatar = $":WebUISite:/upload/images/avatars/{(i + 2)}.png",
                        Password = EncryptHelper.MD5Encrypt32(userName),
                        Email = "acc" + (i + 1) + "@qq.com",
                        Status = 0,
                        RoleInfoList = (from m in allRole where m.ID == randomNum select m).ToList(),
                        RegTime = DateTime.Now.AddDays(i).AddMinutes(i + 1)
                    });
                }

                ShowMessage("成功");
            }
            catch (Exception ex)
            {
                ShowMessage("失败");
                ShowMessage(ex.Message);
            }
        }
        #endregion

        #region 初始化公告
        private void InitArticle()
        {
            try
            {
                ShowMessage("开始初始化公告");

                ArticleService articleService = Container.Instance.Resolve<ArticleService>();
                for (int i = 0; i < 10; i++)
                {
                    articleService.Create(new Article
                    {
                        CreateTime = DateTime.Now,
                        LastUpdateTime = DateTime.Now,
                        Author = new UserInfo { ID = 1 },
                        Title = "测试公告" + (i + 1),
                        Content = "测试内容" + (i + 1),
                        CustomUrl = $"article-{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day}-{i + 1}"
                    });
                }

                ShowMessage("成功");
            }
            catch (Exception)
            {
                ShowMessage("失败");
            }
        }
        #endregion




        #region 初始化关注者被关注者表
        private void InitFollower_Followed()
        {
            try
            {
                ShowMessage("初始化关注者被关注者表");



                ShowMessage("成功");
            }
            catch (Exception ex)
            {
                ShowMessage("失败");
                ShowMessage(ex.Message);
            }
        }
        #endregion

        #region 初始化收藏夹表
        private void InitFavorite()
        {
            try
            {
                ShowMessage("开始初始化收藏夹表");

                IList<UserInfo> allUserInfo = Container.Instance.Resolve<UserInfoService>().GetAll();

                // 每个用户都有一个默认收藏夹（不可删除）
                foreach (var user in allUserInfo)
                {
                    Container.Instance.Resolve<FavoriteService>().Create(new Favorite
                    {
                        Name = "默认收藏夹",
                        Description = "默认自带收藏夹，不可删除，不可编辑",
                        IsOpen = false,
                        CreateTime = DateTime.Now,
                        Creator = user
                    });
                }


                ShowMessage("成功");
            }
            catch (Exception ex)
            {
                ShowMessage("失败");
                ShowMessage(ex.Message);
            }
        }
        #endregion

        #region 初始化课程
        private void InitCourseBox()
        {
            try
            {
                ShowMessage("开始初始化课程");
                string[] names = {
                    "高等数学（一）",
                    "Python语言程序设计 ",
                    "沟通心理学",
                    "程序设计入门——C语言",
                    "金融学"
                };
                string[] descs = {
                    "高等数学是以微积分为主要内容的课程，它不但是理工类各专业，也是其他众多专业最重要的基础课程之一。我们的工作、科研以及生活中的很多例子，如：卫星成功驶进预定轨道，火车在弯道上飞驰而过，经济金融、天气预报和深海下潜，都与数学有着深深的联系。现在就让我们一起去高等数学的殿堂探索吧！",

                    "计算机是运算工具，更是创新平台，高效有趣地利用计算机需要更简洁实用的编程语言。Python简洁却强大、简单却专业，它是当今世界最受欢迎的编程语言，学好它终身受用。请跟随我们，学习并掌握Python语言，一起动起来，站在风口、享受创新！",

                    "视角独特的“主持型”心理课，正能量的“烧脑”之旅。言语犀利，案例丰富，思维训练，实用性强，主讲人裴秋宇老师凝聚19年心理医生“心战”经历、15年世界五百强员工心理内训精华，获得2016最受欢迎慕课top1、2017十大最受欢迎国家精品在线课程。",

                    "C语言是古老而长青的编程语言，它具备了现代程序设计的基础要求，它的语法是很多其他编程语言的基础，在系统程序、嵌入式系统等领域依然是无可替代的编程语言，在各类编程语言排行榜上常年占据前两名的位置。 本课程是零基础的编程入门课，是后续的操作系统、编译原理、体系结构等课程的基石。",

                    "本课程是金融专业统帅性基础理论课。教学资源丰富，教学理念先进，教学方法多元。采用宽口径的范畴，涵盖货币、信用、金融资产与价格、金融市场、金融机构、金融总量与均衡、调控与监管、金融发展等所有金融活动的集合。有利于认识金融原理，了解金融现状，掌握分析方法，培养解决金融问题的能力。"
                };
                string[] picUrls = {
                    ":WebUISite:/upload/images/courseBoxPics/1.jpeg",
                    ":WebUISite:/upload/images/courseBoxPics/2.png",
                    ":WebUISite:/upload/images/courseBoxPics/3.jpg",
                    ":WebUISite:/upload/images/courseBoxPics/4.jpg",
                    ":WebUISite:/upload/images/courseBoxPics/5.jpg",
                };

                for (int i = 0; i < names.Length; i++)
                {
                    Container.Instance.Resolve<CourseBoxService>().Create(new CourseBox
                    {
                        CreateTime = DateTime.Now.AddDays(i),
                        LastUpdateTime = DateTime.Now.AddDays(i),
                        Creator = new UserInfo { ID = 1 },
                        StartTime = DateTime.Now.AddDays(i),
                        EndTime = DateTime.Now.AddMonths(7).AddDays(i),
                        Name = names[i],
                        Description = descs[i],
                        PicUrl = picUrls[i]
                    });
                }

                ShowMessage("成功");
            }
            catch (Exception)
            {
                ShowMessage("失败");
            }
        }
        #endregion

        #region 初始化视频课件
        private void InitVideoInfo()
        {
            try
            {
                ShowMessage("开始初始化视频课件");

                string[] playUrls = {
                    "https://remstatic.oss-cn-beijing.aliyuncs.com/videos/01-%E7%A7%BB%E5%8A%A8web%E5%BC%80%E5%8F%91_01/1%E3%80%81%E5%A7%94%E6%89%98%E5%A4%8D%E4%B9%A0.mp4",
                    "https://remstatic.oss-cn-beijing.aliyuncs.com/videos/01-%E7%A7%BB%E5%8A%A8web%E5%BC%80%E5%8F%91_01/2%E3%80%81%E6%B3%9B%E5%9E%8B%E5%A7%94%E6%89%98.mp4",
                    "https://remstatic.oss-cn-beijing.aliyuncs.com/videos/01-%E7%A7%BB%E5%8A%A8web%E5%BC%80%E5%8F%91_01/3%E3%80%81%E5%A4%9A%E6%92%AD%E5%A7%94%E6%89%98.avi",
                    "https://remstatic.oss-cn-beijing.aliyuncs.com/videos/01-%E7%A7%BB%E5%8A%A8web%E5%BC%80%E5%8F%91_01/4%E3%80%81%E4%BD%BF%E7%94%A8%E5%A7%94%E6%89%98%E8%BF%9B%E8%A1%8C%E7%AA%97%E4%BD%93%E4%BC%A0%E5%80%BC.avi",
                    "https://remstatic.oss-cn-beijing.aliyuncs.com/videos/01-%E7%A7%BB%E5%8A%A8web%E5%BC%80%E5%8F%91_01/5%E3%80%81%E4%BA%8B%E4%BB%B6.avi",
                    "https://remstatic.oss-cn-beijing.aliyuncs.com/videos/01-%E7%A7%BB%E5%8A%A8web%E5%BC%80%E5%8F%91_01/6%E3%80%81%E4%BA%8B%E4%BB%B6%E7%BB%93%E6%9D%9F.avi",
                    "https://remstatic.oss-cn-beijing.aliyuncs.com/videos/01-%E7%A7%BB%E5%8A%A8web%E5%BC%80%E5%8F%91_01/6%E3%80%81%E4%BA%8B%E4%BB%B6%E7%BB%93%E6%9D%9F.avi",
                    "https://remstatic.oss-cn-beijing.aliyuncs.com/videos/01-%E7%A7%BB%E5%8A%A8web%E5%BC%80%E5%8F%91_01/7%E3%80%81%E5%8F%8D%E5%B0%84.avi",
                    "https://remstatic.oss-cn-beijing.aliyuncs.com/videos/01-%E7%A7%BB%E5%8A%A8web%E5%BC%80%E5%8F%91_01/8%E3%80%81%E7%A8%8B%E5%BA%8F%E9%9B%86%E4%B8%AD%E7%9A%843%E4%B8%AA%E5%B8%B8%E7%94%A8%E5%87%BD%E6%95%B0.mp4",
                    "https://remstatic.oss-cn-beijing.aliyuncs.com/videos/01-%E7%A7%BB%E5%8A%A8web%E5%BC%80%E5%8F%91_01/9%E3%80%81%E4%BD%BF%E7%94%A8%E5%8F%8D%E5%B0%84%E5%88%B6%E4%BD%9C%E8%AE%A1%E7%AE%97%E5%99%A8.mp4",
                    "https://remstatic.oss-cn-beijing.aliyuncs.com/videos/01-%E7%A7%BB%E5%8A%A8web%E5%BC%80%E5%8F%91_01/%E4%B8%8A%E5%8D%8802-%E6%96%87%E6%A1%A3%E7%BB%93%E6%9E%841.avi",
                    "https://remstatic.oss-cn-beijing.aliyuncs.com/videos/01-%E7%A7%BB%E5%8A%A8web%E5%BC%80%E5%8F%91_01/%E4%B8%8A%E5%8D%8803-%E8%A7%86%E5%8F%A3.avi",
                    "https://remstatic.oss-cn-beijing.aliyuncs.com/videos/01-%E7%A7%BB%E5%8A%A8web%E5%BC%80%E5%8F%91_01/%E4%B8%8A%E5%8D%8804-Bootstrap%E9%BB%98%E8%AE%A4%E6%A8%A1%E7%89%88.avi",
                    "https://remstatic.oss-cn-beijing.aliyuncs.com/videos/01-%E7%A7%BB%E5%8A%A8web%E5%BC%80%E5%8F%91_01/%E4%B8%8A%E5%8D%8805-%E5%85%A8%E5%B1%80CSS%E6%A0%B7%E5%BC%8F.avi",
                    "https://remstatic.oss-cn-beijing.aliyuncs.com/videos/01-%E7%A7%BB%E5%8A%A8web%E5%BC%80%E5%8F%91_01/%E4%B8%8A%E5%8D%8806-Bootstrap%E7%BB%84%E4%BB%B6%E5%BF%AB%E9%80%9F%E8%BF%87.avi",
                    "https://remstatic.oss-cn-beijing.aliyuncs.com/videos/01-%E7%A7%BB%E5%8A%A8web%E5%BC%80%E5%8F%91_01/%E4%B8%8B%E5%8D%8802-%E9%A1%B6%E9%83%A8%E9%80%9A%E6%A0%8F%EF%BC%88%E5%AD%97%E4%BD%93%E5%9B%BE%E6%A0%87%EF%BC%89.avi",
                    "https://remstatic.oss-cn-beijing.aliyuncs.com/videos/01-%E7%A7%BB%E5%8A%A8web%E5%BC%80%E5%8F%91_01/%E4%B8%8B%E5%8D%8804-%E5%8A%A0%E5%8F%B7%E9%80%89%E6%8B%A9%E5%99%A8.avi",
                    "https://remstatic.oss-cn-beijing.aliyuncs.com/videos/01-%E7%A7%BB%E5%8A%A8web%E5%BC%80%E5%8F%91_01/%E4%B8%8B%E5%8D%8806-%E5%9B%BE%E6%A0%87%E5%AD%97%E4%BD%93%E5%9B%9E%E9%A1%BE.avi",
                    "https://remstatic.oss-cn-beijing.aliyuncs.com/videos/01-%E7%A7%BB%E5%8A%A8web%E5%BC%80%E5%8F%91_01/%E4%B8%8B%E5%8D%8808-%E5%AF%BC%E8%88%AA%E6%A0%B7%E5%BC%8F.avi",
                    "https://remstatic.oss-cn-beijing.aliyuncs.com/videos/01-%E7%A7%BB%E5%8A%A8web%E5%BC%80%E5%8F%91_01/%E4%B8%8B%E5%8D%8810-%E5%93%8D%E5%BA%94%E5%BC%8F%E8%8F%9C%E5%8D%95.avi",
                    "https://remstatic.oss-cn-beijing.aliyuncs.com/videos/5%E3%80%81%E5%AF%B9XML%E6%96%87%E6%A1%A3%E5%A2%9E%E5%88%A0%E6%94%B9%E6%9F%A5%282%29.mp4",
                    "https://remstatic.oss-cn-beijing.aliyuncs.com/videos/%E4%B8%8A%E5%8D%881-ASPX%E9%80%92%E5%BD%92%E5%88%86%E7%B1%BB.wmv",
                    "https://remstatic.oss-cn-beijing.aliyuncs.com/videos/%E4%B8%8A%E5%8D%882-FlipView.avi",
                    "https://remstatic.oss-cn-beijing.aliyuncs.com/videos/%E4%B8%8A%E5%8D%883-Hub%2BPivot.wmv",
                    "https://remstatic.oss-cn-beijing.aliyuncs.com/videos/%E4%B8%8A%E5%8D%884-%E5%B9%BF%E5%91%8A%E5%B9%B3%E5%8F%B0.avi",
                    "https://remstatic.oss-cn-beijing.aliyuncs.com/videos/%E4%B8%8A%E5%8D%886-%E5%BA%94%E7%94%A8%E7%A8%8B%E5%BA%8F%E6%A0%8F%E5%92%8CWebView.avi",
                    "https://remstatic.oss-cn-beijing.aliyuncs.com/videos/%E4%B8%8A%E5%8D%887-%E6%8E%A7%E4%BB%B6%E8%A1%A5%E5%85%85.wmv",
                    "https://remstatic.oss-cn-beijing.aliyuncs.com/videos/%E4%B8%8B%E5%8D%882-Style%E7%9A%84%E7%BB%A7%E6%89%BF.wmv",
                    "https://remstatic.oss-cn-beijing.aliyuncs.com/videos/%E4%B8%8B%E5%8D%884-Timer%E6%A8%A1%E6%8B%9F%E5%8A%A8%E7%94%BB.wmv",
                    "https://remstatic.oss-cn-beijing.aliyuncs.com/videos/%E4%B8%8B%E5%8D%886-%E5%85%B3%E9%94%AE%E5%B8%A7%E5%8A%A8%E7%94%BB%E5%92%8C%E4%BE%9D%E8%B5%96%E5%B1%9E%E6%80%A7%E5%8A%A8%E7%94%BB%E8%A1%A5%E5%85%85.wmv",
                    "https://remstatic.oss-cn-beijing.aliyuncs.com/videos/%E4%B8%8B%E5%8D%887-%E5%8A%A8%E7%94%BB%E6%93%8D%E4%BD%9C%E8%A1%A5%E5%85%85.avi",
                    "https://remstatic.oss-cn-beijing.aliyuncs.com/videos/%E4%B8%8B%E5%8D%888-%E5%89%8D%E7%AB%AF%E5%B0%8F%E7%9F%A5%E8%AF%86%E5%88%86%E4%BA%AB.wmv",
                    "https://remstatic.oss-cn-beijing.aliyuncs.com/videos/%E4%B8%8B%E5%8D%889-%E5%9F%BA%E6%9C%AC%E7%9A%84%E7%BB%98%E5%9B%BEAPI.avi"
                };


                // 共5门课程
                int playIndex = 0;
                for (int i = 0; i < 5; i++)
                {
                    // 每门课程 3个课件
                    for (int j = 0; j < 3; j++)
                    {
                        Container.Instance.Resolve<VideoInfoService>().Create(new VideoInfo
                        {
                            CourseBox = new CourseBox { ID = i + 1 },
                            Page = j + 1,
                            PlayUrl = playUrls[playIndex],
                            Title = Server.UrlDecode(playUrls[playIndex]).Substring(Server.UrlDecode(playUrls[playIndex]).LastIndexOf("/") + 1),
                            Size = 141344,
                        });
                        playIndex++;
                    }
                }


                ShowMessage("成功");
            }
            catch (Exception)
            {
                ShowMessage("失败");
            }
        }
        #endregion

        #region 初始化Learner_CourseBox表
        private void InitLearner_CourseBox()
        {
            try
            {
                ShowMessage("初始化Learner_CourseBox表");



                ShowMessage("成功");
            }
            catch (Exception)
            {
                ShowMessage("失败");
            }
        }
        #endregion

        #region 初始化Learner_VideoInfo表
        private void InitLearner_VideoInfo()
        {
            try
            {
                ShowMessage("初始化Learner_VideoInfo表");




                ShowMessage("成功");
            }
            catch (Exception)
            {
                ShowMessage("失败");
            }
        }
        #endregion

        #region 初始化评论
        private void InitComment()
        {
            try
            {
                ShowMessage("初始化评论");



                ShowMessage("成功");
            }
            catch (Exception)
            {
                ShowMessage("失败");
            }
        }
        #endregion






















        #region 初始化卡片盒
        private void InitCardBox()
        {
            try
            {
                ShowMessage("开始初始化卡片盒");

                CardBoxService cardBoxService = Container.Instance.Resolve<CardBoxService>();

                for (int i = 0; i < 10; i++)
                {
                    UserInfo userInfo = Container.Instance.Resolve<UserInfoService>().GetEntity(1);
                    CardBox cardBox = new CardBox();
                    cardBox.Name = "测试卡片盒-" + (i + 1);
                    cardBox.Description = $"这是测试卡片盒-{(i + 1)}的描述";
                    //cardBox.ReaderList = new List<UserInfo>();
                    //cardBox.ReaderList.Add(userInfo);
                    cardBox.Creator = userInfo;
                    cardBoxService.Create(cardBox);
                }

                ShowMessage("成功");
            }
            catch (Exception)
            {
                ShowMessage("失败");
            }
        }
        #endregion

        #region 初始化卡片
        private void InitCardInfo()
        {
            try
            {
                ShowMessage("开始初始化卡片");

                CardInfoService cardInfoService = Container.Instance.Resolve<CardInfoService>();

                for (int i = 0; i < 10; i++)
                {
                    CardInfo cardInfo = new CardInfo();
                    cardInfo.Content = $"测试内容{(1 + i)}";
                    cardInfo.CardBox = new CardBox { ID = (1 + i) };
                    cardInfoService.Create(cardInfo);
                }

                ShowMessage("成功");
            }
            catch (Exception)
            {
                ShowMessage("失败");
            }
        }
        #endregion

    }
}