using Domain.Entities;
using Framework.Common;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class InstallController : Controller
    {
        #region Fields
        private readonly IUserInfoService _userInfoService;
        private readonly ISettingService _settingService;
        private readonly IThemeTemplateService _themeTemplateService;
        private readonly ISys_MenuService _sys_MenuService;
        private readonly IFunctionInfoService _functionInfoService;
        private readonly IRoleInfoService _roleInfoService;
        private readonly IArticleService _articleService;
        private readonly IFavoriteService _favoriteService;
        private readonly IRole_MenuService _role_MenuService;
        private readonly IRole_FunctionService _role_FunctionService;
        #endregion

        #region Ctor
        public InstallController(IUserInfoService userInfoService,
                                 ISettingService settingService,
                                 IThemeTemplateService themeTemplateService,
                                 ISys_MenuService sys_MenuService,
                                 IFunctionInfoService functionInfoService,
                                 IRoleInfoService roleInfoService,
                                 IArticleService articleService,
                                 IFavoriteService favoriteService,
                                 IRole_MenuService role_MenuService,
                                 IRole_FunctionService role_FunctionService)
        {
            this._userInfoService = userInfoService;
            this._settingService = settingService;
            this._themeTemplateService = themeTemplateService;
            this._sys_MenuService = sys_MenuService;
            this._functionInfoService = functionInfoService;
            this._roleInfoService = roleInfoService;
            this._articleService = articleService;
            this._favoriteService = favoriteService;
            this._role_MenuService = role_MenuService;
            this._role_FunctionService = role_FunctionService;
        }
        #endregion

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
            InitFunctionInfo();
            InitRoleInfo();
            InitUserInfo();

            InitFavorite();
            InitArticle();
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
                findPwd_MailContent += "&nbsp; &nbsp;祝在【remember】收获愉快！";
                findPwd_MailContent += "<br><br>";
                findPwd_MailContent += "&nbsp; &nbsp;（这是一封自动发送的邮件，请不要直接回复）";
                findPwd_MailContent += "</p>";

                Dictionary<string, string> dic = new Dictionary<string, string>()
                {
                    { "DefaultTemplateName", "Blue" },

                    { "WebApiSite", "http://localhost:4530/" },
                    { "WebApiTitle","remember" },
                    { "WebApiDesc", "remember是xx推出的专业在线教育平台，聚合大量优质教育机构和名师，下设职业培训、公务员考试、托福雅思、考证考级、英语口语、中小学教育等众多在线学习精品课程，打造老师在线上课教学、学生及时互动学习的课堂。"},
                    { "WebApiKeywords", "" },
                    { "WebApiStat", "" },

                    { "WebUISite", "http://localhost:4483/" },
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
                    { "FindPwd_MailContent", findPwd_MailContent },

                    { "CorsWhiteList", "* live.a.com m.a.com"}
                };

                foreach (var keyValue in dic)
                {
                    this._settingService.Create(new Setting()
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

                this._themeTemplateService.Create(new ThemeTemplate
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

                // TODO: 系统菜单图标初始化
                #region 一级菜单
                string[] firstLevel_names = {
                       "首页",
                       "全局",
                       "界面",
                       "内容",
                       "用户",
                       "门户",
                       "防灌水",
                       "运营",
                       "应用",
                       "工具",
                       "站长",
                       "UCenter"
                };
                string[] firstLevel_icons = {
                    "glyphicon glyphicon-home",
                    "glyphicon glyphicon-globe",
                    "glyphicon glyphicon-blackboard",
                    "glyphicon glyphicon-th-large",
                    "fa fa-folder",
                    "fa fa-folder",
                    "fa fa-folder",
                    "fa fa-folder",
                    "fa fa-folder",
                    "fa fa-folder",
                    "fa fa-folder",
                    "fa fa-folder",
                    "fa fa-folder",
                    "fa fa-folder",
                    "fa fa-folder",
                    "fa fa-folder",
                    "fa fa-folder",
                    "fa fa-folder",
                    "fa fa-folder",
                    "fa fa-folder",
                    "fa fa-folder",
                    "fa fa-folder",
                    "fa fa-folder",
                    "fa fa-folder",
                    "fa fa-folder",
                };
                for (int i = 0; i < firstLevel_names.Length; i++)
                {
                    this._sys_MenuService.Create(new Sys_Menu()
                    {
                        Name = firstLevel_names[i],
                        Icon = firstLevel_icons[i],
                        SortCode = i + 1,
                    });

                }
                #endregion

                #region 二级菜单

                // 一级菜单项---二级菜单的父菜单项
                Sys_Menu parentMenu = null;

                #region 首页
                parentMenu = this._sys_MenuService.Find(m => m.Name == "首页");

                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "常用操作管理",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 10,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "仪表盘-1",
                    Description = "站点每日基本信息",
                    ControllerName = "Dashboard",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 10,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "仪表盘-2",
                    ControllerName = "Dashboard",
                    ActionName = "IndexTwo",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 10,
                });
                #endregion

                #region 全局
                parentMenu = this._sys_MenuService.Find(m => m.Name == "全局");

                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "站点设置",
                    ControllerName = "Setting",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 10,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "注册与访问控制",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 10,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "站点功能",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 10,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "性能优化",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 10,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "SEO设置",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 10,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "域名设置",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 10,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "空间设置",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 10,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "用户权限",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 10,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "积分设置",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 10,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "时间设置",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 10,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "上传设置",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 10,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "水印设置",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 10,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "附件类型尺寸",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 10,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "搜索设置",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 10,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "地区设置",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 10,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "排行榜设置",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 10,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "手机版访问设置",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 10,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "防采集设置",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 10,
                });
                #endregion

                #region 界面
                parentMenu = this._sys_MenuService.Find(m => m.Name == "界面");

                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "导航设置",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 10,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "界面设置",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 10,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "菜单管理",
                    ControllerName = "SysMenu",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 10,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "模板管理",
                    ControllerName = "ThemeTemplate",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 20,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "风格管理",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 20,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "表情管理",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 20,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "编辑器设置",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 20,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "在线列表图标",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 20,
                });
                #endregion

                #region 内容
                parentMenu = this._sys_MenuService.Find(m => m.Name == "内容");

                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "文章管理",
                    ControllerName = "Article",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 10,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "文库管理",
                    ControllerName = "BookInfo",
                    ActionName = "Index",
                    AreaName = "Admin",
                    SortCode = 10,
                    Parent = parentMenu,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "词语过滤",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    SortCode = 10,
                    Parent = parentMenu,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "用户举报",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    SortCode = 10,
                    Parent = parentMenu,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "标签管理",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    SortCode = 10,
                    Parent = parentMenu,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "回收站",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    SortCode = 10,
                    Parent = parentMenu,
                });
                #endregion

                #region 用户
                parentMenu = this._sys_MenuService.Find(m => m.Name == "用户");

                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "用户管理",
                    ControllerName = "UserInfo",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 10,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "角色管理",
                    ControllerName = "RoleInfo",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 20,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "资料统计",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 20,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "发送通知",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 20,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "用户标签",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 20,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "禁止用户",
                    ControllerName = "BanUser",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 20,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "禁止IP",
                    ControllerName = "BanIP",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 20,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "积分奖惩",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 20,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "审核用户",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 20,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "管理组",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 20,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "用户组",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 20,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "推荐关注",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 20,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "推荐好友",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 20,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "资料审核",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 20,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "认证设置",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 20,
                });
                #endregion

                #region 门户
                parentMenu = this._sys_MenuService.Find(m => m.Name == "门户");

                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "频道栏目",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 10,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "分区管理",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 10,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "专题管理",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 10,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "HTML管理",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 10,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "页面管理",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 10,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "模块管理",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 10,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "模块模板",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 10,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "第三方模块",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 10,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "权限列表",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 10,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "日志列表",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 10,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "相册分类",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 10,
                });
                #endregion

                #region 防灌水
                parentMenu = this._sys_MenuService.Find(m => m.Name == "防灌水");

                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "基本设置",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 10,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "验证设置",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 20,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "安全大师",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 20,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "账号保镖",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 20,
                });
                #endregion

                #region 运营
                parentMenu = this._sys_MenuService.Find(m => m.Name == "运营");

                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "访问日志",
                    Description = "站点访问统计",
                    ControllerName = "LogInfo",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 20,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "站点任务",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 10,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "道具中心",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 10,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "勋章中心",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 10,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "站点帮助",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 10,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "电子商务",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 10,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "友情链接",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 10,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "站长推荐",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 10,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "关联链接",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 10,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "充值卡密",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 10,
                });
                #endregion

                #region 应用
                parentMenu = this._sys_MenuService.Find(m => m.Name == "应用");

                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "插件管理",
                    ControllerName = "Plugin",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 10,
                });
                #endregion

                #region 工具
                parentMenu = this._sys_MenuService.Find(m => m.Name == "工具");

                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "更新缓存",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 10,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "更新统计",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 20,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "运行记录",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 10,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "计划任务",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 10,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "文件权限检查",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 10,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "文件效验",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 10,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "嵌入点效验",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 10,
                });
                #endregion

                #region 站长
                parentMenu = this._sys_MenuService.Find(m => m.Name == "站长");

                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "后台管理团队",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 10,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "邮件设置",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 20,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "UCenter 设置",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 10,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "数据库",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 10,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "用户表优化",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 10,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "文章分表",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 10,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "优化大师",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    Parent = parentMenu,
                    SortCode = 10,
                });
                #endregion

                #endregion

                #region 三级菜单

                #region 内容-回收站
                parentMenu = this._sys_MenuService.Find(m => m.Name == "回收站");

                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "文章回收站",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    SortCode = 10,
                    Parent = parentMenu,
                });
                this._sys_MenuService.Create(new Sys_Menu()
                {
                    Name = "文库回收站",
                    ControllerName = "Home",
                    ActionName = "Index",
                    AreaName = "Admin",
                    SortCode = 10,
                    Parent = parentMenu,
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
        private void InitFunctionInfo()
        {
            try
            {
                ShowMessage("开始初始化操作表");
                // ID: 1
                // 特殊抽象操作---决定是否能进入管理中心
                // 只要拥有系统菜单下的任一操作权限 --> 就会拥有此对应系统菜单项 --> 就会拥有进入管理中心，即拥有此抽象的特殊操作权限(Admin.Home.Index  (后台)管理中心(框架))
                this._functionInfoService.Create(new FunctionInfo
                {
                    AuthKey = "Admin.Home.Index",
                    Name = "(后台)管理中心(框架)"
                });

                //IList<ICriterion> qryWhere = new List<ICriterion>();

                #region 这些 都具有 "新增", "修改", "删除", "查看"
                string[] names = { "用户管理", "角色管理", "菜单管理", "操作管理" };
                IList<Sys_Menu> allMenu = this._sys_MenuService.All().ToList();
                IList<int> idList = allMenu.Where(m => names.Contains(m.Name)).Select(m => m.ID).ToList();
                //qryWhere.Add(Expression.In("ID", idList.ToArray()));
                //IList<Sys_Menu> findMenuList = Container.Instance.Resolve<Sys_MenuService>().Query(qryWhere);
                IList<Sys_Menu> findMenuList = this._sys_MenuService.Filter(m => idList.Contains(m.ID)).ToList();
                string[] funcNames = { "新增", "编辑", "删除", "查看" };
                string[] actionNames = { "Create", "Edit", "Delete", "Details", "Index" };
                foreach (var menu in findMenuList)
                {
                    for (int i = 0; i < actionNames.Length; i++)
                    {
                        if (i == actionNames.Length - 1)
                        {
                            this._functionInfoService.Create(new FunctionInfo()
                            {
                                Name = menu.Name + "-主页",
                                AuthKey = menu.AreaName + "." + menu.ControllerName + "." + actionNames[i],
                                Sys_Menu = menu
                            });
                        }
                        else
                        {
                            this._functionInfoService.Create(new FunctionInfo()
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
                Sys_Menu article_Sys_Menu = this._sys_MenuService.Find(m => m.ControllerName == "Article");
                this._functionInfoService.Create(new FunctionInfo
                {
                    AuthKey = "Admin.Article.Create",
                    Name = "文章管理-创建",
                    Sys_Menu = article_Sys_Menu
                });
                this._functionInfoService.Create(new FunctionInfo
                {
                    AuthKey = "Admin.Article.Index",
                    Name = "文章管理-列表",
                    Sys_Menu = article_Sys_Menu
                });
                this._functionInfoService.Create(new FunctionInfo
                {
                    AuthKey = "Admin.Article.Edit",
                    Name = "文章管理-编辑",
                    Sys_Menu = article_Sys_Menu
                });
                this._functionInfoService.Create(new FunctionInfo
                {
                    AuthKey = "Admin.Article.Delete",
                    Name = "文章管理-删除",
                    Sys_Menu = article_Sys_Menu
                });
                #endregion

                #region 角色管理
                // 角色RoleInfo菜单 增加授权操作
                Sys_Menu roleInfo_Sys_Menu = this._sys_MenuService.Find(m => m.ControllerName == "RoleInfo");
                this._functionInfoService.Create(new FunctionInfo
                {
                    AuthKey = "Admin.RoleInfo.AssignPower",
                    Name = "角色管理-授权",
                    Sys_Menu = roleInfo_Sys_Menu
                });
                #endregion

                #region 主题模板 ThemeTemplate
                Sys_Menu themeTemplate_Sys_Menu = this._sys_MenuService.Find(m => m.ControllerName == "ThemeTemplate");
                this._functionInfoService.Create(new FunctionInfo
                {
                    AuthKey = "Admin.ThemeTemplate.Index",
                    Name = "主题模板-列表",
                    Sys_Menu = themeTemplate_Sys_Menu
                });
                this._functionInfoService.Create(new FunctionInfo
                {
                    AuthKey = "Admin.ThemeTemplate.UploadTemplate",
                    Name = "主题模板-上传本地主题模板页面",
                    Sys_Menu = themeTemplate_Sys_Menu
                });
                this._functionInfoService.Create(new FunctionInfo
                {
                    AuthKey = "Admin.ThemeTemplate.UploadTemplateFile",
                    Name = "主题模板-上传本地主题模板",
                    Sys_Menu = themeTemplate_Sys_Menu
                });
                this._functionInfoService.Create(new FunctionInfo
                {
                    AuthKey = "Admin.ThemeTemplate.InstallZip",
                    Name = "主题模板-安装",
                    Sys_Menu = themeTemplate_Sys_Menu
                });
                this._functionInfoService.Create(new FunctionInfo
                {
                    AuthKey = "Admin.ThemeTemplate.Uninstall",
                    Name = "主题模板-卸载",
                    Sys_Menu = themeTemplate_Sys_Menu
                });
                this._functionInfoService.Create(new FunctionInfo
                {
                    AuthKey = "Admin.ThemeTemplate.DeleteInstallZip",
                    Name = "主题模板-删除安装包",
                    Sys_Menu = themeTemplate_Sys_Menu
                });
                this._functionInfoService.Create(new FunctionInfo
                {
                    AuthKey = "Admin.ThemeTemplate.OpenClose",
                    Name = "主题模板-启用禁用",
                    Sys_Menu = themeTemplate_Sys_Menu
                });
                this._functionInfoService.Create(new FunctionInfo
                {
                    AuthKey = "Admin.ThemeTemplate.SetDefault",
                    Name = "主题模板-设置为默认模板",
                    Sys_Menu = themeTemplate_Sys_Menu
                });
                #endregion

                #region 单独操作（不放在系统菜单下）
                this._functionInfoService.Create(new FunctionInfo
                {
                    AuthKey = "Admin.ThemeTemplate.SelectTemplate",
                    Name = "主题模板-选择模板",
                    Sys_Menu = null
                });
                #endregion

                #region 全局-站点设置
                Sys_Menu setting_Sys_Menu = this._sys_MenuService.Find(m => m.ControllerName == "Setting");
                this._functionInfoService.Create(new FunctionInfo
                {
                    AuthKey = "Admin.Setting.Index",
                    Name = "站点设置-常规",
                    Sys_Menu = setting_Sys_Menu
                });
                this._functionInfoService.Create(new FunctionInfo
                {
                    AuthKey = "Admin.Setting.WebApi",
                    Name = "站点设置-WebApi",
                    Sys_Menu = setting_Sys_Menu
                });
                this._functionInfoService.Create(new FunctionInfo
                {
                    AuthKey = "Admin.Setting.FindPwd",
                    Name = "站点设置-找回密码",
                    Sys_Menu = setting_Sys_Menu
                });
                this._functionInfoService.Create(new FunctionInfo
                {
                    AuthKey = "Admin.Setting.SysEmail",
                    Name = "站点设置-系统邮箱",
                    Sys_Menu = setting_Sys_Menu
                });
                this._functionInfoService.Create(new FunctionInfo
                {
                    AuthKey = "Admin.Setting.Advanced",
                    Name = "站点设置-高级",
                    Sys_Menu = setting_Sys_Menu
                });
                #endregion

                #region 文库
                Sys_Menu bookInfo_Sys_Menu = this._sys_MenuService.Find(m => m.ControllerName == "BookInfo");
                this._functionInfoService.Create(new FunctionInfo
                {
                    AuthKey = "Admin.BookInfo.Index",
                    Name = "文库-列表",
                    Sys_Menu = bookInfo_Sys_Menu
                });
                this._functionInfoService.Create(new FunctionInfo
                {
                    AuthKey = "Admin.BookInfo.Edit",
                    Name = "文库-编辑",
                    Sys_Menu = bookInfo_Sys_Menu
                });
                this._functionInfoService.Create(new FunctionInfo
                {
                    AuthKey = "Admin.BookInfo.ImportBook",
                    Name = "文库-导入文库",
                    Sys_Menu = bookInfo_Sys_Menu
                });
                this._functionInfoService.Create(new FunctionInfo
                {
                    AuthKey = "Admin.BookInfo.Delete",
                    Name = "文库-删除",
                    Sys_Menu = bookInfo_Sys_Menu
                });
                this._functionInfoService.Create(new FunctionInfo
                {
                    AuthKey = "Admin.BookInfo.Create",
                    Name = "文库-创建",
                    Sys_Menu = bookInfo_Sys_Menu
                });
                this._functionInfoService.Create(new FunctionInfo
                {
                    AuthKey = "Admin.BookInfo.AddVideo",
                    Name = "文库章节-添加",
                    Sys_Menu = bookInfo_Sys_Menu
                });
                this._functionInfoService.Create(new FunctionInfo
                {
                    AuthKey = "Admin.BookInfo.DeleteVideo",
                    Name = "文库章节-删除",
                    Sys_Menu = bookInfo_Sys_Menu
                });
                this._functionInfoService.Create(new FunctionInfo
                {
                    AuthKey = "Admin.BookInfo.EditVideo",
                    Name = "文库章节-编辑",
                    Sys_Menu = bookInfo_Sys_Menu
                });
                #endregion

                #region 访问日志
                Sys_Menu logInfo_Sys_Menu = this._sys_MenuService.Find(m => m.ControllerName == "LogInfo");
                this._functionInfoService.Create(new FunctionInfo
                {
                    AuthKey = "Admin.LogInfo.Index",
                    Name = "访问日志-列表",
                    Sys_Menu = logInfo_Sys_Menu
                });
                this._functionInfoService.Create(new FunctionInfo
                {
                    AuthKey = "Admin.LogInfo.Delete",
                    Name = "访问日志-删除",
                    Sys_Menu = logInfo_Sys_Menu
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
        private void InitRoleInfo()
        {
            try
            {
                ShowMessage("开始初始化角色表");

                var allMenu = this._sys_MenuService.All().ToList();
                var allFunction = this._functionInfoService.All().ToList();

                // 系统组
                #region 系统组
                RoleInfo admin_roleInfo = new RoleInfo
                {
                    Name = "超级管理员",
                    Role_Menus = new List<Role_Menu>(),
                    Role_Functions = new List<Role_Function>()
                };
                foreach (var menu in allMenu)
                {
                    admin_roleInfo.Role_Menus.Add(new Role_Menu
                    {
                        Sys_MenuId = menu.ID,
                        CreateTime = DateTime.Now,
                    });
                }
                foreach (var func in allFunction)
                {
                    admin_roleInfo.Role_Functions.Add(new Role_Function
                    {
                        FunctionInfoId = func.ID,
                        CreateTime = DateTime.Now
                    });
                }
                // 1
                this._roleInfoService.Create(admin_roleInfo);
                // 2
                RoleInfo roleInfo_2 = new RoleInfo
                {
                    Name = "游客",
                    Role_Menus = new List<Role_Menu>(),
                    Role_Functions = new List<Role_Function>()
                };
                this._roleInfoService.Create(roleInfo_2);
                // 3
                RoleInfo roleInfo_3 = new RoleInfo
                {
                    Name = "副站长",
                    Role_Menus = new List<Role_Menu>(),
                    Role_Functions = new List<Role_Function>()
                };
                this._roleInfoService.Create(roleInfo_3);
                // 4
                RoleInfo roleInfo_4 = new RoleInfo
                {
                    Name = "运营",
                    Role_Menus = new List<Role_Menu>(),
                    Role_Functions = new List<Role_Function>()
                };
                this._roleInfoService.Create(roleInfo_4);
                // 5
                RoleInfo roleInfo_5 = new RoleInfo
                {
                    Name = "站长",
                    Role_Menus = new List<Role_Menu>(),
                    Role_Functions = new List<Role_Function>()
                };
                this._roleInfoService.Create(roleInfo_5);
                #endregion

                // 自定义用户组
                #region 自定义用户组
                // 6
                RoleInfo roleInfo_6 = new RoleInfo
                {
                    Name = "蓝钻",
                    Role_Menus = new List<Role_Menu>(),
                    Role_Functions = new List<Role_Function>()
                };
                this._roleInfoService.Create(roleInfo_6);
                // 7
                RoleInfo roleInfo_7 = new RoleInfo
                {
                    Name = "红钻",
                    Role_Menus = new List<Role_Menu>(),
                    Role_Functions = new List<Role_Function>()
                };
                this._roleInfoService.Create(roleInfo_7);
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

                var allRole = this._roleInfoService.All().ToList();

                UserInfo userInfo = null;
                // 超级管理员 1
                userInfo = new UserInfo()
                {
                    UserName = "admin",
                    Avatar = ":WebUISite:/assets/images/default-avatar.jpg",
                    Password = EncryptHelper.MD5Encrypt32("admin"),
                    Email = "yiyungent@126.com",
                    Description = "我是超级管理员",
                    Role_Users = new List<Role_User>(),
                    CreateTime = DateTime.Now
                };
                userInfo.Role_Users.Add(new Role_User
                {
                    RoleInfoId = (from m in allRole where m.ID == 1 select m).FirstOrDefault()?.ID ?? 0,
                    CreateTime = DateTime.Now
                });
                this._userInfoService.Create(userInfo);

                ShowMessage("成功");
            }
            catch (Exception ex)
            {
                ShowMessage("失败");
                ShowMessage(ex.Message);
            }
        }
        #endregion

        #region 初始化文章
        private void InitArticle()
        {
            try
            {
                ShowMessage("开始初始化文章表");

                // 默认文章
                this._articleService.Create(new Article
                {
                    AuthorId = 1,
                    ArticleStatus = Article.AStatus.Publish,
                    OpenStatus = Article.OStatus.All,
                    CommentCount = 0,
                    CommentStatus = Article.CStatus.Open,
                    CreateTime = DateTime.Now,
                    LastUpdateTime = DateTime.Now,
                    Title = "Hello World！",
                    Content = "Hello World! 这是一篇自动生成的文章",
                    CustomUrl = "HelloWorld.html",
                    Description = "这是一篇自动生成的文章"
                });

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

                IList<UserInfo> allUserInfo = this._userInfoService.All().ToList();

                // 每个用户都有一个默认收藏夹（不可删除）
                foreach (var user in allUserInfo)
                {
                    this._favoriteService.Create(new Favorite
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
    }
}