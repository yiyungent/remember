using Core;
using Domain;
using Domain.Entities;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Areas.Admin.Models;
using WebUI.Areas.Admin.Models.Common;

namespace WebUI.Areas.Admin.Controllers
{
    public class SysMenuController : Controller
    {
        #region Fields
        private readonly ISys_MenuService _sys_MenuService;
        #endregion

        #region Ctor
        public SysMenuController(ISys_MenuService sys_MenuService)
        {
            //ViewBag.PageHeader = "菜单管理";
            //ViewBag.PageHeaderDescription = "菜单管理";
            //ViewBag.BreadcrumbList = new List<BreadcrumbItem>
            //{
            //    new BreadcrumbItem("系统管理"),
            //};
            this._sys_MenuService = sys_MenuService;
        }
        #endregion

        #region 列表
        public ActionResult Index()
        {
            return View();
        }
        #endregion

        #region 编辑
        [HttpGet]
        public ViewResult Edit(int id)
        {
            //Sys_Menu viewModel = Container.Instance.Resolve<Sys_MenuService>().GetEntity(id);
            Sys_Menu viewModel = this._sys_MenuService.Find(m => m.ID == id && !m.IsDeleted);
            int parentId = viewModel.ParentId ?? 0;
            ViewBag.DDLParent = InitDDLForParent(viewModel, parentId);

            return View(viewModel);
        }

        [HttpPost]
        public JsonResult Edit(Sys_Menu inputModel)
        {
            try
            {
                //Sys_Menu dbModel = Container.Instance.Resolve<Sys_MenuService>().GetEntity(inputModel.ID);
                Sys_Menu dbModel = this._sys_MenuService.Find(m => m.ID == inputModel.ID && !m.IsDeleted);
                // 上级菜单
                if (inputModel.ParentId == null || inputModel.ParentId == 0)
                {
                    //inputModel.ParentMenu = null;
                    inputModel.ParentId = null;
                }
                // 设置修改后的值
                dbModel.Name = inputModel.Name;
                dbModel.SortCode = inputModel.SortCode;
                //dbModel.ParentMenu = inputModel.ParentMenu;
                dbModel.ParentId = inputModel.ParentId;
                //Container.Instance.Resolve<Sys_MenuService>().Edit(dbModel);
                this._sys_MenuService.Update(dbModel);

                return Json(new { code = 1, message = "保存成功" });
            }
            catch (Exception ex)
            {
                return Json(new { code = -1, message = "保存失败" });
            }
        }
        #endregion

        #region Helpers
        private IList<SelectListItem> InitDDLForParent(Sys_Menu self, int parentId)
        {
            IList<SelectListItem> ret = new List<SelectListItem>();
            ret.Add(new SelectListItem()
            {
                Text = "请选择",
                Value = "0",
                Selected = (0 == parentId),
            });

            //IList<Sys_Menu> all = Container.Instance.Resolve<Sys_MenuService>().GetAll();
            IList<Sys_Menu> all = this._sys_MenuService.All().ToList();
            // 找出自己及后代的ID
            IList<int> idRange = new List<int>();
            GetIdRange(self, idRange, all);

            // not in 查询
            var find = from m in all
                       where idRange.Contains(m.ID) == false
                       select m;
            // 方案一：不考虑层级的实现
            //foreach (var item in find)
            //{
            //    ret.Add(new SelectListItem()
            //    {
            //        Text = item.Name,
            //        Value = item.ID.ToString(),
            //        Selected = (item.ID == parentId)
            //    });
            //}
            // 方案二：考虑层级的实现
            // 1.一级菜单
            var first = from m in find
                        where m.ParentId == null || m.ParentId == 0
                        orderby m.SortCode
                        select m;
            foreach (var item in first)
            {
                AddSelfAndChildrenForDDL("", item, ret, find.ToList(), parentId);
            }

            return ret;
        }

        private void AddSelfAndChildrenForDDL(string pref, Sys_Menu self, IList<SelectListItem> target, IList<Sys_Menu> all, int parentId)
        {
            // 1.添加自己
            target.Add(new SelectListItem()
            {
                Text = pref + self.Name,
                Value = self.ID.ToString(),
                Selected = (self.ID == parentId)
            });
            // 2.递归添加子女
            var child = from m in all
                            //where m.ParentMenu != null && m.ParentMenu.ID == self.ID
                        where m.ParentId != null && m.ParentId != 0 && m.Parent.ID == self.ID
                                && !m.IsDeleted
                        orderby m.SortCode
                        select m;
            foreach (var item in child)
            {
                AddSelfAndChildrenForDDL(pref + "--", item, target, all, parentId);
            }
        }

        /// <summary>
        /// 递归方法：添加自己及其子女
        /// </summary>
        /// <param name="self"></param>
        /// <param name="idRange"></param>
        /// <param name="all"></param>
        private void GetIdRange(Sys_Menu self, IList<int> idRange, IList<Sys_Menu> all)
        {
            // 添加自己
            idRange.Add(self.ID);

            // 关于子女循环
            // 第二种解决方法：
            if (self.Children == null) return;
            foreach (var item in self.Children)
            {
                // 递归调用---添加自己和自己的子女
                GetIdRange(item, idRange, all);
            }
        }
        #endregion
    }
}