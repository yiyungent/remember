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
            InitFollower_Followed();
            InitArticle();
            InitCardBox();
            InitCardInfo();
            InitCourseBox();
            InitLearner_CourseBox();
            InitVideoInfo();
            InitLearner_VideoInfo();
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

        #region 初始化设置
        private void InitSetting()
        {
            try
            {
                ShowMessage("初始化设置");

                Dictionary<string, string> dic = new Dictionary<string, string>()
                {
                    { "DefaultTemplateName", "Blue" },

                    { "WebApiSite", "http://localhost:7784/" },
                    { "WebApiTitle","remember" },
                    { "WebApiDescription", "remember是xx推出的专业在线教育平台，聚合大量优质教育机构和名师，下设职业培训、公务员考试、托福雅思、考证考级、英语口语、中小学教育等众多在线学习精品课程，打造老师在线上课教学、学生及时互动学习的课堂。"},
                    { "WebApiKeywords", "" },

                    { "WebUISite", "http://localhost:21788/" },
                    { "WebUITitle", "remember - 在线学习" },
                    { "WebUIDescription", "remember是xx推出的专业在线教育平台，聚合大量优质教育机构和名师，下设职业培训、公务员考试、托福雅思、考证考级、英语口语、中小学教育等众多在线学习精品课程，打造老师在线上课教学、学生及时互动学习的课堂。"},
                    { "WebUIKeywords", "" },
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
                    Status = 1
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
                    Name = "仪表盘",
                    SortCode = 10,
                });
                Container.Instance.Resolve<Sys_MenuService>().Create(new Sys_Menu()
                {
                    Name = "系统管理",
                    SortCode = 20,
                });
                Container.Instance.Resolve<Sys_MenuService>().Create(new Sys_Menu()
                {
                    Name = "业务管理",
                    SortCode = 30,
                });
                #endregion

                #region 二级菜单
                // 一级菜单项---二级菜单的父菜单项
                Sys_Menu parentMenu = null;

                #region 仪表盘的二级菜单
                parentMenu = Container.Instance.Resolve<Sys_MenuService>().Query(new List<ICriterion>
                {
                    Expression.Eq("Name", "仪表盘")
                }).FirstOrDefault();

                Container.Instance.Resolve<Sys_MenuService>().Create(new Sys_Menu()
                {
                    Name = "仪表盘-1",
                    ControllerName = "Dashboard",
                    ActionName = "One",
                    AreaName = "Admin",
                    ParentMenu = parentMenu,
                    SortCode = 10,
                });
                #endregion

                #region 系统管理的二级菜单
                parentMenu = Container.Instance.Resolve<Sys_MenuService>().Query(new List<ICriterion>
                {
                    Expression.Eq("Name", "系统管理")
                }).FirstOrDefault();

                Container.Instance.Resolve<Sys_MenuService>().Create(new Sys_Menu()
                {
                    Name = "站点信息",
                    ControllerName = "BaseInfo",
                    ActionName = "Index",
                    AreaName = "Admin",
                    ParentMenu = parentMenu,
                    SortCode = 10,
                });
                Container.Instance.Resolve<Sys_MenuService>().Create(new Sys_Menu()
                {
                    Name = "用户管理",
                    ControllerName = "UserInfo",
                    ActionName = "Index",
                    AreaName = "Admin",
                    ParentMenu = parentMenu,
                    SortCode = 20,
                });
                Container.Instance.Resolve<Sys_MenuService>().Create(new Sys_Menu()
                {
                    Name = "角色管理",
                    ControllerName = "RoleInfo",
                    ActionName = "Index",
                    AreaName = "Admin",
                    ParentMenu = parentMenu,
                    SortCode = 30,
                });
                Container.Instance.Resolve<Sys_MenuService>().Create(new Sys_Menu()
                {
                    Name = "菜单管理",
                    ControllerName = "SysMenu",
                    ActionName = "Index",
                    AreaName = "Admin",
                    ParentMenu = parentMenu,
                    SortCode = 40,
                });
                //Container.Instance.Resolve<Sys_MenuService>().Create(new Sys_Menu()
                //{
                //    Name = "操作管理",
                //    ControllerName = "FunctionInfo",
                //    ActionName = "Index",
                //    AreaName = "Admin",
                //    ParentMenu = parentMenu,
                //    SortCode = 50,
                //});
                Container.Instance.Resolve<Sys_MenuService>().Create(new Sys_Menu()
                {
                    Name = "主题模板",
                    ControllerName = "ThemeTemplate",
                    ActionName = "Index",
                    AreaName = "Admin",
                    ParentMenu = parentMenu,
                    SortCode = 60,
                });
                Container.Instance.Resolve<Sys_MenuService>().Create(new Sys_Menu()
                {
                    Name = "插件管理",
                    ControllerName = "Plugin",
                    ActionName = "Index",
                    AreaName = "Admin",
                    ParentMenu = parentMenu,
                    SortCode = 70,
                });
                #endregion

                #region 业务管理的二级菜单
                parentMenu = Container.Instance.Resolve<Sys_MenuService>().Query(new List<ICriterion>
                {
                    Expression.Eq("Name", "业务管理")
                }).FirstOrDefault();


                Container.Instance.Resolve<Sys_MenuService>().Create(new Sys_Menu()
                {
                    Name = "文章管理",
                    ControllerName = "Article",
                    ActionName = "Index",
                    AreaName = "Admin",
                    ParentMenu = parentMenu,
                    SortCode = 80,
                });
                #endregion

                #endregion

                #region 三级菜单
                parentMenu = Container.Instance.Resolve<Sys_MenuService>().Query(new List<ICriterion>
                {
                    Expression.Eq("Name", "仪表盘-1")
                }).FirstOrDefault();

                Container.Instance.Resolve<Sys_MenuService>().Create(new Sys_Menu()
                {
                    Name = "仪表盘-1 - 测试三级菜单",
                    AreaName = "Admin",
                    ControllerName = "RoleInfo",
                    ActionName = "Index",
                    ParentMenu = parentMenu,
                    SortCode = 10,
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

                Container.Instance.Resolve<RoleInfoService>().Create(new RoleInfo
                {
                    Name = "超级管理员",
                    Status = 0,
                    Sys_MenuList = allMenu,
                    FunctionInfoList = allFunction
                });

                Container.Instance.Resolve<RoleInfoService>().Create(new RoleInfo
                {
                    Name = "游客",
                    Status = 0,
                    Sys_MenuList = null,
                    FunctionInfoList = null
                });

                Container.Instance.Resolve<RoleInfoService>().Create(new RoleInfo
                {
                    Name = "正式会员",
                    Status = 0,
                    Sys_MenuList = null,
                    FunctionInfoList = null
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
                    Name = "超级管理员admin",
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

                // 正式会员 1000
                for (int i = 0; i < 10; i++)
                {
                    userInfoService.Create(new UserInfo
                    {
                        Name = $"会员-{ i + 1 }",
                        UserName = "acc" + (i + 1),
                        Description = $"我是会员-{i + 1}",
                        Avatar = ":WebUISite:/assets/images/default-avatar.jpg",
                        Password = EncryptHelper.MD5Encrypt32("acc" + (i + 1)),
                        Email = "acc" + (i + 1) + "@qq.com",
                        Status = 0,
                        RoleInfoList = (from m in allRole where m.ID == 3 select m).ToList(),
                        RegTime = DateTime.Now
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

        #region 初始化关注者被关注者表
        private void InitFollower_Followed()
        {
            try
            {
                ShowMessage("初始化关注者被关注者表");

                IList<UserInfo> allUserInfo = Container.Instance.Resolve<UserInfoService>().GetAll();
                for (int i = 0; i < allUserInfo.Count; i++)
                {
                    UserInfo follower = allUserInfo[new Random().Next(0, allUserInfo.Count - 1)];
                    IList<UserInfo> exceptAllUserInfo = allUserInfo.Where(m => m.ID != follower.ID).ToList();
                    Container.Instance.Resolve<Follower_FollowedService>().Create(new Follower_Followed
                    {
                        Follower = follower,
                        Followed = exceptAllUserInfo[new Random().Next(0, exceptAllUserInfo.Count - 1)],
                        CreateTime = DateTime.Now.AddDays(new Random().Next(0, 100)).AddMinutes(new Random().Next(0, 1000))
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

        #region 初始化文章
        private void InitArticle()
        {
            try
            {
                ShowMessage("开始初始化文章");

                ArticleService articleService = Container.Instance.Resolve<ArticleService>();
                for (int i = 0; i < 10; i++)
                {
                    articleService.Create(new Article
                    {
                        CreateTime = DateTime.Now,
                        LastUpdateTime = DateTime.Now,
                        Author = new UserInfo { ID = 1 },
                        Title = "测试文章" + (i + 1),
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

        #region 初始化课程
        private void InitCourseBox()
        {
            try
            {
                ShowMessage("开始初始化课程");

                CourseBoxService courseBoxService = Container.Instance.Resolve<CourseBoxService>();

                for (int i = 0; i < 500; i++)
                {
                    UserInfo userInfo = Container.Instance.Resolve<UserInfoService>().GetEntity(1);
                    CourseBox courseBox = new CourseBox();
                    courseBox.Name = "课程-" + (i + 1);
                    courseBox.Description = $"这是测试课程-{(i + 1)}的描述";
                    courseBox.Creator = userInfo;
                    courseBox.PicUrl = "https://static.runoob.com/images/mix/img_fjords_wide.jpg";
                    courseBox.CreateTime = DateTime.Now.AddDays(i);
                    courseBox.LastUpdateTime = DateTime.Now.AddDays(i);

                    courseBoxService.Create(courseBox);
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

                IList<CourseBox> allCourseBox = Container.Instance.Resolve<CourseBoxService>().GetAll();

                IList<UserInfo> allUserInfo = Container.Instance.Resolve<UserInfoService>().GetAll();

                for (int i = 0; i < allCourseBox.Count; i++)
                {
                    // 此课程的学习人数
                    int learnNum = allUserInfo.Count;
                    for (int j = 0; j < learnNum; j++)
                    {
                        CourseBox courseBox = Container.Instance.Resolve<CourseBoxService>().GetEntity(i + 1);

                        Learner_CourseBox learner_CourseBox = new Learner_CourseBox
                        {
                            CourseBox = courseBox,
                            JoinTime = DateTime.Now.AddDays(j + 1),
                            Learner = allUserInfo[new Random().Next(0, allUserInfo.Count - 1)],
                            SpendTime = 100,
                        };

                        Container.Instance.Resolve<Learner_CourseBoxService>().Create(learner_CourseBox);
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

        #region 初始化视频课件
        private void InitVideoInfo()
        {
            try
            {
                ShowMessage("开始初始化课程内容");

                IList<CourseBox> allCourseBox = Container.Instance.Resolve<CourseBoxService>().GetAll();
                for (int i = 0; i < allCourseBox.Count; i++)
                {
                    CourseBox courseBox = allCourseBox[i];
                    // 每门课程 10课件
                    for (int j = 0; j < 10; j++)
                    {
                        VideoInfo courseInfo = new VideoInfo();
                        courseInfo.Title = "视频标题" + (j + 1);
                        courseInfo.PlayUrl = $"/static/upload/videos/" + (j + 1) + ".mp4";
                        courseInfo.Page = (j + 1);
                        courseInfo.CourseBox = courseBox;


                        Container.Instance.Resolve<VideoInfoService>().Create(courseInfo);
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

        #region 初始化Learner_VideoInfo表
        private void InitLearner_VideoInfo()
        {
            try
            {
                ShowMessage("初始化Learner_VideoInfo表");

                IList<CourseBox> allCourseBox = Container.Instance.Resolve<CourseBoxService>().GetAll();
                IList<Learner_CourseBox> allLearner_CourseBox = Container.Instance.Resolve<Learner_CourseBoxService>().GetAll();

                for (int i = 0; i < allLearner_CourseBox.Count; i++)
                {
                    // 当前 学习者-课程
                    Learner_CourseBox learner_CourseBox = allLearner_CourseBox[i];

                    // 为当前学习者所学的这门课程中的所有视频 加上学习播放记录
                    for (int j = 0; j < learner_CourseBox.CourseBox.VideoInfos.Count; j++)
                    {
                        VideoInfo videoInfo = learner_CourseBox.CourseBox.VideoInfos[j];

                        Container.Instance.Resolve<Learner_VideoInfoService>().Create(new Learner_VideoInfo
                        {
                            Learner = learner_CourseBox.Learner,
                            VideoInfo = videoInfo,
                            LastAccessIp = "127.0.0." + new Random().Next(0, 255),
                            LastPlayAt = 60000,
                            LastPlayTime = DateTime.Now.AddDays(new Random().Next(0, 1000)).AddHours(new Random().Next(0, 60)),
                            ProgressAt = 670000
                        });
                    }

                    // 找出当前这学习者对于这门课程中的所有视频 - 哪个视频是最近观看的 LastPlayTime 最大 - 为其更新为 对于学习者-这门课程而言最新课程
                    IList<VideoInfo> currentCourseBox_All_VideoInfo = learner_CourseBox.CourseBox.VideoInfos;
                    long lastNewPlayTime = 0;
                    int lastNewPlayTime_MapTo_VideoInfoId = 0;
                    for (int j = 0; j < currentCourseBox_All_VideoInfo.Count; j++)
                    {
                        VideoInfo videoInfo = currentCourseBox_All_VideoInfo[j];

                        Learner_VideoInfo learner_VideoInfo = Container.Instance.Resolve<Learner_VideoInfoService>().Query(new List<ICriterion>
                        {
                            Expression.And(
                                    Expression.Eq("Learner.ID", learner_CourseBox.Learner.ID),
                                    Expression.Eq("VideoInfo.ID", videoInfo.ID)
                                )
                        }).FirstOrDefault();

                        if (learner_VideoInfo.LastPlayTime.ToTimeStamp13() > lastNewPlayTime)
                        {
                            lastNewPlayTime = learner_VideoInfo.LastPlayTime.ToTimeStamp13();
                            lastNewPlayTime_MapTo_VideoInfoId = learner_VideoInfo.VideoInfo.ID;
                        }
                    }

                    learner_CourseBox.LastPlayVideoInfo = new VideoInfo { ID = lastNewPlayTime_MapTo_VideoInfoId };
                    Container.Instance.Resolve<Learner_CourseBoxService>().Edit(learner_CourseBox);
                }


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

                CommentService commentService = Container.Instance.Resolve<CommentService>();
                UserInfoService userInfoService = Container.Instance.Resolve<UserInfoService>();

                // 课程评论
                #region 课程评论
                // 一级评论
                for (int i = 0; i < 500; i++)
                {
                    commentService.Create(new Comment
                    {
                        Author = userInfoService.GetEntity(i + 2),
                        Content = "课程-评论" + (i + 1),
                        LikeNum = new Random().Next(0, i * 100),
                        DislikeNum = new Random().Next(0, i * 100),
                        CreateTime = DateTime.Now.AddDays(new Random().Next(0, i)),
                        LastUpdateTime = DateTime.Now.AddDays(new Random().Next(0, i))
                    });
                }
                // 二级评论
                for (int i = 0; i < 500; i++)
                {
                    commentService.Create(new Comment
                    {
                        Author = userInfoService.GetEntity(i + 2),
                        Content = "课程-评论" + (i + 1),
                        LikeNum = new Random().Next(0, i * 100),
                        DislikeNum = new Random().Next(0, i * 100),
                        CreateTime = DateTime.Now.AddDays(new Random().Next(0, i)),
                        LastUpdateTime = DateTime.Now.AddDays(new Random().Next(0, i)),
                        Parent = commentService.GetEntity(new Random().Next(1, 500))
                    });
                }
                // 三级评论
                for (int i = 0; i < 500; i++)
                {
                    commentService.Create(new Comment
                    {
                        Author = userInfoService.GetEntity(i + 2),
                        Content = "课程-评论" + (i + 1),
                        LikeNum = new Random().Next(0, i * 100),
                        DislikeNum = new Random().Next(0, i * 100),
                        CreateTime = DateTime.Now.AddDays(new Random().Next(0, i)),
                        LastUpdateTime = DateTime.Now.AddDays(new Random().Next(0, i)),
                        Parent = commentService.GetEntity(new Random().Next(501, 1000))
                    });
                }
                #endregion

                // 课件评论
                #region 课件评论
                // 一级评论
                for (int i = 0; i < 500; i++)
                {
                    commentService.Create(new Comment
                    {
                        Author = userInfoService.GetEntity(i + 2),
                        Content = "课件-评论" + (i + 1),
                        LikeNum = new Random().Next(0, i * 100),
                        DislikeNum = new Random().Next(0, i * 100),
                        CreateTime = DateTime.Now.AddDays(new Random().Next(0, i)),
                        LastUpdateTime = DateTime.Now.AddDays(new Random().Next(0, i))
                    });
                }
                // 二级评论
                for (int i = 0; i < 500; i++)
                {
                    commentService.Create(new Comment
                    {
                        Author = userInfoService.GetEntity(i + 2),
                        Content = "课件-评论" + (i + 1),
                        LikeNum = new Random().Next(0, i * 100),
                        DislikeNum = new Random().Next(0, i * 100),
                        CreateTime = DateTime.Now.AddDays(new Random().Next(0, i)),
                        LastUpdateTime = DateTime.Now.AddDays(new Random().Next(0, i)),
                        Parent = commentService.GetEntity(new Random().Next(1501, 2000))
                    });
                }
                // 三级评论
                for (int i = 0; i < 500; i++)
                {
                    commentService.Create(new Comment
                    {
                        Author = userInfoService.GetEntity(i + 2),
                        Content = "课件-评论" + (i + 1),
                        LikeNum = new Random().Next(0, i * 100),
                        DislikeNum = new Random().Next(0, i * 100),
                        CreateTime = DateTime.Now.AddDays(new Random().Next(0, i)),
                        LastUpdateTime = DateTime.Now.AddDays(new Random().Next(0, i)),
                        Parent = commentService.GetEntity(new Random().Next(2500, 3000))
                    });
                }
                #endregion


                ShowMessage("成功");
            }
            catch (Exception)
            {
                ShowMessage("失败");
            }
        }
        #endregion

        #region 初始化课程评论
        private void InitCourseBox_Comment()
        {
            try
            {
                ShowMessage("初始化课程评论");

                CourseBox_CommentService courseBox_CommentService = Container.Instance.Resolve<CourseBox_CommentService>();
                CommentService commentService = Container.Instance.Resolve<CommentService>();

                IList<CourseBox> allCourseBox = Container.Instance.Resolve<CourseBoxService>().GetAll();
                IList<Comment> allCourseBox_Comment = commentService.Query(new List<ICriterion>
                {
                     Expression.Like("Content", "课程", MatchMode.Anywhere)
                }).ToList();

                for (int i = 0; i < allCourseBox.Count; i++)
                {
                    CourseBox courseBox = allCourseBox[i];
                    for (int j = 0; j < allCourseBox_Comment.Count; j++)
                    {
                        courseBox_CommentService.Create(new CourseBox_Comment
                        {
                            CourseBox = courseBox,
                            Comment = allCourseBox_Comment[new Random().Next(0, allCourseBox_Comment.Count)]
                        });
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



    }
}