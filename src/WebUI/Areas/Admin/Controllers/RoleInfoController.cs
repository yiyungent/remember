using Core;
using Domain;
using Framework.Factories;
using Framework.HtmlHelpers;
using Framework.Infrastructure.Abstract;
using Framework.Infrastructure.Concrete;
using Framework.Models;
using Framework.Mvc;
using NHibernate.Criterion;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Areas.Admin.Models;
using WebUI.Areas.Admin.Models.Common;

namespace WebUI.Areas.Admin.Controllers
{
    public class RoleInfoController : Controller
    {
        private IAuthManager _authManager;

        #region Ctor
        public RoleInfoController()
        {
            this._authManager = HttpOneRequestFactory.Get<IAuthManager>();

            ViewBag.PageHeader = "角色管理";
            ViewBag.PageHeaderDescription = "角色管理";
            ViewBag.BreadcrumbList = new List<BreadcrumbItem>
            {
                new BreadcrumbItem("业务管理"),
            };
        }
        #endregion

        #region 首页-列表
        public ViewResult Index(int pageIndex = 1, int pageSize = 6)
        {
            IList<ICriterion> queryConditions = new List<ICriterion>();

            ListViewModel<RoleInfo> model = new ListViewModel<RoleInfo>(queryConditions, pageIndex: pageIndex, pageSize: pageSize);
            TempData["RedirectUrl"] = Request.RawUrl;

            return View(model);
        }
        #endregion

        #region 删除
        public JsonResult Delete(int id)
        {
            try
            {
                Container.Instance.Resolve<RoleInfoService>().Delete(id);

                return Json(new { code = 1, message = "删除成功" });
            }
            catch (Exception ex)
            {
                return Json(new { code = -1, message = "删除失败" });
            }
        }
        #endregion

        #region 查看
        public ViewResult Detail(int id)
        {
            RoleInfo model = Container.Instance.Resolve<RoleInfoService>().GetEntity(id);

            return View(model);
        }
        #endregion

        #region 编辑
        [HttpGet]
        public ViewResult Edit(int id)
        {
            RoleInfo roleInfo = Container.Instance.Resolve<RoleInfoService>().GetEntity(id);
            RoleInfoForEditViewModel model = (RoleInfoForEditViewModel)roleInfo;

            return View(model);
        }

        [HttpPost]
        public JsonResult Edit(RoleInfoForEditViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // hack 游客
                    if (model.ID == 2)
                    {
                        return Json(new { code = -3, message = "游客名禁止修改" });
                    }

                    RoleInfo dbEntry = Container.Instance.Resolve<RoleInfoService>().GetEntity(model.ID);
                    dbEntry.Name = model.Name;

                    Container.Instance.Resolve<RoleInfoService>().Edit(dbEntry);

                    return Json(new { code = 1, message = "保存成功" });
                }
                else
                {
                    return Json(new { code = -1, message = "不合理的输入" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = -2, message = "保存失败" });
            }
        }
        #endregion

        #region 授权
        [HttpGet]
        public ViewResult AssignPower(int id)
        {
            RoleInfo model = Container.Instance.Resolve<RoleInfoService>().GetEntity(id);

            return View(model);
        }

        /// <summary>
        /// 授权
        /// </summary>
        /// <param name="id">角色ID</param>
        /// <param name="menuIds">授予的系统菜单ID: 1,2,4,5,6,</param>
        /// <param name="funcIds">授予的权限操作ID: 3,6,8,3,</param>
        [HttpPost]
        public JsonResult AssignPower(int id, string menuIds, string funcIds)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string[] menuIdStrArr = menuIds.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    // 只要拥有系统菜单下的任一操作权限 --> 就会拥有此对应系统菜单项 --> 就会拥有进入管理中心，即拥有此抽象的特殊操作权限(Admin.Home.Index  (后台)管理中心(框架))
                    if (menuIdStrArr != null && menuIdStrArr.Length > 0)
                    {
                        funcIds += "1";
                    }
                    string[] funcIdStrArr = funcIds.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    IList<int> menuIdList = new List<int>();
                    IList<int> funcIdList = new List<int>();
                    foreach (string idStr in menuIdStrArr)
                    {
                        menuIdList.Add(Convert.ToInt32(idStr));
                    }
                    foreach (string idStr in funcIdStrArr)
                    {
                        funcIdList.Add(Convert.ToInt32(idStr));
                    }
                    bool isSuccess = this._authManager.AssignPower(id, menuIdList, funcIdList);

                    if (isSuccess)
                    {
                        // 更新 Session 登录用户
                        AccountManager.UpdateSessionAccount();
                        return Json(new { code = 1, message = "保存成功, 菜单需刷新后有效" });
                    }
                    else
                    {
                        return Json(new { code = -1, message = "保存失败" });
                    }
                }
                else
                {
                    return Json(new { code = -2, message = "不合理的输入" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = -3, message = "保存失败" });
            }
        }


        #endregion


        #region 获取此角色的菜单权限树
        /// <summary>
        /// 获取此角色的菜单权限树
        /// </summary>
        /// <param name="id">角色ID</param>
        public JsonResult GetRole_MenuAndFunc_Tree(int id)
        {
            IList<ZNodeModel> rtnJson = new List<ZNodeModel>();

            RoleInfo roleInfo = Container.Instance.Resolve<RoleInfoService>().GetEntity(id);

            IList<Sys_Menu> allMenuList = this._authManager.AllMenuList();
            IList<FunctionInfo> allFuncList = this._authManager.AllFuncList();
            // 排除抽象的特殊操作（只要拥有系统菜单下的任一权限，即会拥有进入管理中心，即拥有此操作权限）
            allFuncList = allFuncList.Where(m => m.Name != "(后台)管理中心(框架)").ToList();
            IList<Sys_Menu> roleMenuList = this._authManager.GetMenuListByRole(roleInfo);
            IList<FunctionInfo> roleFuncList = this._authManager.GetFuncListByRole(roleInfo);

            foreach (Sys_Menu menu in allMenuList)
            {
                rtnJson.Add(new ZNodeModel
                {
                    id = menu.ID,
                    fId = null,
                    isParent = true,
                    name = menu.Name,
                    pId = menu.ParentMenu == null ? 0 : menu.ParentMenu.ID,
                    open = false,
                    @checked = roleMenuList.Contains(menu, new Sys_Menu_Compare())
                });
            }
            foreach (FunctionInfo func in allFuncList)
            {
                rtnJson.Add(new ZNodeModel
                {
                    // 标记为操作，由于Menu.ID, 和 FunctionInfo.ID 存在重复，所以不能写 FunctionInfo.ID
                    id = -1,
                    fId = func.ID,
                    isParent = false,
                    name = func.Name,
                    pId = func.Sys_Menu == null ? 0 : func.Sys_Menu.ID,
                    open = true,
                    @checked = roleFuncList.Contains(func, new FunctionInfo_Compare())
                });
            }

            return Json(rtnJson, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}