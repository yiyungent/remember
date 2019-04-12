using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Castle.ActiveRecord;
using Newtonsoft.Json;
using Remember.Web.Models;
using System.Threading;
using System.Text;
using Remember.Core;
using Remember.Service;
using Remember.Domain;
using System.Configuration;
using Remember.Common;
using NHibernate.Criterion;

namespace Remember.Web.Controllers
{
    public delegate void Del_OnInstall(ref InstallProgressList list);
    public class InstallController : Controller
    {
        private event Del_OnInstall _onInstall;

        private event Action _onInstallComplete;

        private static InstallProgressList _installProgressList = new InstallProgressList();

        private static readonly string _connStrKey = "connection.connection_string";

        private static InstallConfig _installConfig;

        /// <summary>
        /// <code>true</code>: 安装已经锁定(已经安装过)
        /// </summary>
        private bool _installLock;

        public InstallController()
        {
            _onInstall += CreateSchema;
            _onInstall += InitTableData;
            _onInstall += InitTableSysMenu;
            _onInstall += InitTableSysFunction;
            _onInstall += InitTableSysRole;
            _onInstall += InitTableSysUser;
            _onInstallComplete += RedirectToInstallCompletePage;
        }

        #region Index表单视图
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        #endregion

        #region 开始安装-提交表单
        [HttpPost]
        public string Index(InstallConfig installConfig)
        {
            if (!ModelState.IsValid)
            {
                IDictionary<string, string> errorInfos = ModelStateFindErrors(ModelState);

                var jsonObj = new { code = -1, errorInfos };
                string jsonStr = JsonConvert.SerializeObject(jsonObj);
                return jsonStr;
            }
            else
            {
                // 将 installConfig 写入配置文件(管理员账号密码不写入)

                // 以下三种方法都失败，ConnectionStrings 提示只读
                //ConfigurationManager.ConnectionStrings[_connStrKey].ConnectionString = string.Format("Database={0};Data Source={1};User Id={2};Password={3};Charset=utf8;Allow Zero DateTime=True", installConfig.dbname, installConfig.dbhost, installConfig.dbuser, installConfig.dbpw);
                //ConfigurationManager.ConnectionStrings[_connStrKey] = new ConnectionStringSettings(_connStrKey, string.Format("Database={0};Data Source={1};User Id={2};Password={3};Charset=utf8;Allow Zero DateTime=True", installConfig.dbname, installConfig.dbhost, installConfig.dbuser, installConfig.dbpw));

                //ConfigurationManager.ConnectionStrings.Add(new ConnectionStringSettings(_connStrKey, string.Format("Database={0};Data Source={1};User Id={2};Password={3};Charset=utf8;Allow Zero DateTime=True", installConfig.dbname, installConfig.dbhost, installConfig.dbuser, installConfig.dbpw)));
                // 跳转到 安装进度(使用 installConfig，执行安装)

                _installConfig = installConfig;
                return "{\"code\":1}";
            }
        }
        #endregion

        #region 将模型验证的错误转换成 PropertyName -> ErrorMessagesStr
        private IDictionary<string, string> ModelStateFindErrors(ModelStateDictionary modelStateDic)
        {
            var keyAndErrors = from m in modelStateDic
                               where m.Value.Errors.Any()
                               select new { m.Key, m.Value.Errors };
            // 注意： 每一个 key 都对应着一个错误集合
            var errorsColl = from m in keyAndErrors select m.Errors;
            string[] keyColl = (from m in keyAndErrors select m.Key).ToArray();

            IDictionary<string, string> errorInfos = new Dictionary<string, string>();
            int index = 0;
            foreach (var item in errorsColl)
            {
                // item: 每一个 Key 对应的 错误集合
                // 只要错误集合中的错误消息
                var errorMsgs = from m in item
                                select m.ErrorMessage;
                errorInfos.Add(keyColl[index], string.Join(" 且 ", errorMsgs));
                index++;
            }
            return errorInfos;
        }
        #endregion

        #region 安装进度视图
        public ActionResult InstallProgress()
        {
            return View();
        }
        #endregion

        #region 安装完成视图
        public ActionResult InstallComplete()
        {
            return View();
        }
        #endregion

        #region 执行安装
        public void ExecInstall()
        {
            _onInstall(ref _installProgressList);
            _onInstallComplete();
        }
        #endregion

        #region 安装完成后跳转到安装完成页
        private void RedirectToInstallCompletePage()
        {
            Response.Write(string.Format("<script>parent.location.href=\"{0}\"</script>", Url.Action("InstallComplete")));
        }
        #endregion

        #region 输出当前进度消息
        private void ShowProgressMsg(InstallProgress progres)
        {
            string js = "<script type=\"text/javascript\">window.scrollTo(0, document.body.scrollHeight);<" + "/script>";
            string message = string.Format("<div>{0}...{1}</div>", progres.info, progres.isSuccess ? "成功" : "失败");
            Response.Write(message);
            Response.Write(js);
            Response.Flush();
        }
        private void ShowProgressMsg(string message)
        {
            string js = "<script type=\"text/javascript\">window.scrollTo(0, document.body.scrollHeight);<" + "/script>";
            Response.Write(message);
            Response.Write(js);
            Response.Flush();
        }
        #endregion

        // start 自定义安装步骤

        #region 创建数据库表结构
        private void CreateSchema(ref InstallProgressList list)
        {
            InstallProgress pro = new InstallProgress { info = "创建数据库 表结构" };
            list.AddItem(pro);
            try
            {
                ActiveRecordStarter.CreateSchema();
                pro.isSuccess = true;
            }
            catch (Exception ex)
            {
                pro.exception = ex;
                ShowProgressMsg(ex.Message);
            }
            Thread.Sleep(3000);
            ShowProgressMsg(pro);
        }
        #endregion

        #region 开始初始化表数据
        private void InitTableData(ref InstallProgressList list)
        {
            InstallProgress pro = new InstallProgress { info = "开始 初始化表数据" };
            list.AddItem(pro);
            pro.isSuccess = true;
            ShowProgressMsg(pro.info);
        }
        #endregion

        #region 初始化角色表数据
        private void InitTableSysRole(ref InstallProgressList list)
        {
            InstallProgress pro = new InstallProgress { info = "SysRole 表初始化数据" };
            list.AddItem(pro);
            try
            {
                IList<SysMenu> allMenu = Container.Instance.Resolve<SysMenuService>().GetAll();
                IList<SysFunction> allFunc = Container.Instance.Resolve<SysFunctionService>().GetAll();

                Container.Instance.Resolve<SysRoleService>().Create(new SysRole()
                {
                    Name = "系统管理员",
                    Status = 0,
                    SysMenuList = allMenu,
                    SysFunctionList = allFunc
                });

                pro.isSuccess = true;
            }
            catch (Exception ex)
            {
                pro.exception = ex;
                ShowProgressMsg(ex.Message);
            }
            Thread.Sleep(3000);
            ShowProgressMsg(pro);
        }
        #endregion

        #region 初始化用户表数据
        private void InitTableSysUser(ref InstallProgressList list)
        {
            InstallProgress pro = new InstallProgress { info = "SysUser 表初始化数据" };
            list.AddItem(pro);
            try
            {
                SysRole adminRole = Container.Instance.Resolve<SysRoleService>().GetEntity(1);
                List<SysRole> roleList = new List<SysRole>();
                roleList.Add(adminRole);
                Container.Instance.Resolve<SysUserService>().Create(new SysUser
                {
                    Name = "系统管理员",
                    LoginAccount = _installConfig.username,
                    Password = StringHelper.EncodeMD5(_installConfig.password),
                    SysRoleList = roleList,
                    Status = 0
                });

                pro.isSuccess = true;
            }
            catch (Exception ex)
            {
                pro.exception = ex;
                ShowProgressMsg(ex.Message);
            }
            Thread.Sleep(3000);
            ShowProgressMsg(pro);
        }
        #endregion

        #region 初始化菜单表数据
        private void InitTableSysMenu(ref InstallProgressList list)
        {
            InstallProgress pro = new InstallProgress { info = "SysMenu 表初始化数据" };
            list.AddItem(pro);
            try
            {
                #region 一级菜单
                Container.Instance.Resolve<SysMenuService>().Create(new SysMenu()
                {
                    Name = "系统管理",
                    SortCode = 10,
                });
                Container.Instance.Resolve<SysMenuService>().Create(new SysMenu()
                {
                    Name = "业务管理",
                    SortCode = 20,
                });
                #endregion

                SysMenu parentMenu = null;
                #region 系统管理的二级菜单
                parentMenu = Container.Instance.Resolve<SysMenuService>().GetEntity(1);
                Container.Instance.Resolve<SysMenuService>().Create(new SysMenu()
                {
                    Name = "菜单管理",
                    ClassName = "Remember.Web.Controllers.SysMenuController",
                    ControllerName = "SysMenu",
                    ActionName = "Index",
                    ParentMenu = parentMenu,
                    SortCode = 10,
                });
                Container.Instance.Resolve<SysMenuService>().Create(new SysMenu()
                {
                    Name = "用户管理",
                    ClassName = "Remember.Web.Controllers.SysUserController",
                    ControllerName = "SysUser",
                    ActionName = "Index",
                    ParentMenu = parentMenu,
                    SortCode = 20,
                });
                Container.Instance.Resolve<SysMenuService>().Create(new SysMenu()
                {
                    Name = "角色管理",
                    ClassName = "Remember.Web.Controllers.SysRoleController",
                    ControllerName = "SysRole",
                    ActionName = "Index",
                    ParentMenu = parentMenu,
                    SortCode = 30,
                });
                #endregion

                #region 业务的二级菜单
                parentMenu = Container.Instance.Resolve<SysMenuService>().GetEntity(2);
                Container.Instance.Resolve<SysMenuService>().Create(new SysMenu()
                {
                    Name = "卡片盒管理",
                    ClassName = "Remember.Web.Controllers.CardBoxController",
                    ControllerName = "CardBox",
                    ActionName = "Index",
                    ParentMenu = parentMenu,
                    SortCode = 10,
                });
                Container.Instance.Resolve<SysMenuService>().Create(new SysMenu()
                {
                    Name = "卡片管理",
                    ClassName = "Remember.Web.Controllers.CardInfoController",
                    ControllerName = "CardInfo",
                    ActionName = "Index",
                    ParentMenu = parentMenu,
                    SortCode = 20,
                });
                Container.Instance.Resolve<SysMenuService>().Create(new SysMenu()
                {
                    Name = "统计分析",
                    ClassName = "Remember.Web.Controllers.StatisticsController",
                    ControllerName = "Statistics",
                    ActionName = "Index",
                    ParentMenu = parentMenu,
                    SortCode = 30,
                });
                #endregion

                pro.isSuccess = true;
            }
            catch (Exception ex)
            {
                pro.exception = ex;
                ShowProgressMsg(ex.Message);
            }
            Thread.Sleep(3000);
            ShowProgressMsg(pro);
        }
        #endregion

        #region 初始化系统菜单
        private void InitTableSysFunction(ref InstallProgressList list)
        {
            InstallProgress pro = new InstallProgress { info = "SysFunction 表初始化数据" };
            list.AddItem(pro);
            try
            {
                IList<ICriterion> conditionList = new List<ICriterion>();
                // ID: 3-7   有 "新增", "修改", "删除", "查看" 的菜单
                conditionList.Add(Expression.Ge("ID", 3));
                conditionList.Add(Expression.Le("ID", 7));
                // 查找菜单
                IList<SysMenu> findMenuList = Container.Instance.Resolve<SysMenuService>().Query(conditionList);
                string[] funcNames = { "新增", "修改", "删除", "查看" };
                foreach (SysMenu menu in findMenuList)
                {
                    foreach (string funcName in funcNames)
                    {
                        // 为此菜单创建/关联拥有的操作
                        Container.Instance.Resolve<SysFunctionService>().Create(new SysFunction
                        {
                            Name = funcName,
                            SysMenu = menu
                        });
                    }
                }
                // ID: 8 只有 "查看" 的菜单
                Container.Instance.Resolve<SysFunctionService>().Create(new SysFunction
                {
                    Name = "查看",
                    SysMenu = Container.Instance.Resolve<SysMenuService>().GetEntity(8)
                });

                pro.isSuccess = true;
            }
            catch (Exception ex)
            {
                pro.exception = ex;
                ShowProgressMsg(ex.Message);
            }
            Thread.Sleep(1000);
            ShowProgressMsg(pro);
        }
        #endregion


        // end 自定义安装步骤
    }
}
