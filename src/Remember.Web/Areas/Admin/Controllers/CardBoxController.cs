using Remember.Core;
using Remember.Domain;
using Remember.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Remember.Web.Areas.Admin.Controllers
{
    public class CardBoxController : Controller
    {
        #region 列表
        public ActionResult Index(int pageIndex = 1, int pageSize = 10)
        {
            IList<CardBox> list = Container.Instance.Resolve<CardBoxService>().GetAll();
            ViewBag.TotalCount = list.Count;
            // 当前页号超过总页数，则显示最后一页
            int lastPageIndex = (int)Math.Ceiling((double)list.Count / pageSize);
            pageIndex = pageIndex <= lastPageIndex ? pageIndex : lastPageIndex;
            ViewBag.PageIndex = pageIndex;
            ViewBag.PageSize = pageSize;
            // 使用 Skip 还顺便解决了 若 pageIndex <= 0 的错误情况
            var data = (from m in list
                        orderby m.ID descending
                        select m).Skip((pageIndex - 1) * pageSize).Take(pageSize);

            return View(data.ToList());
        }
        #endregion

        #region 新增
        [HttpGet]
        public ViewResult Create()
        {
            CardBox model = new CardBox();

            return View(model);
        }

        [HttpPost]
        public JsonResult Create(CardBox model)
        {
            #region 检查登录状态
            //SysUser sysUser = Session["User"] as SysUser;
            //if (sysUser == null)
            //{
            //    return Json(new { code = -2, message = "当前未登录" });
            //} 
            #endregion
            try
            {
                //model.Creator = sysUser;
                model.Creator = Container.Instance.Resolve<SysUserService>().GetEntity(1);

                Container.Instance.Resolve<CardBoxService>().Create(model);
                string message = string.Format("{0} 创建成功", model.Name);
                TempData["message"] = message;

                return Json(new { code = 1, message });
            }
            catch (Exception ex)
            {
                return Json(new { code = -1, message = ex.Message });
            }
        }
        #endregion

        #region 查看明细
        public ViewResult Details(int id)
        {
            CardBox model = Container.Instance.Resolve<CardBoxService>().GetEntity(id);

            return View(model);
        }
        #endregion

        #region 修改
        [HttpGet]
        public ViewResult Edit(int id)
        {
            CardBox model = Container.Instance.Resolve<CardBoxService>().GetEntity(id);

            return View(model);
        }

        [HttpPost]
        public JsonResult Edit(CardBox model)
        {
            try
            {
                CardBox dbEntry = Container.Instance.Resolve<CardBoxService>().GetEntity(model.ID);
                // 修改
                dbEntry.Name = model.Name;
                dbEntry.Description = model.Description;
                string message = string.Format("{0} 保存修改成功", dbEntry.Name);
                TempData["message"] = message;

                Container.Instance.Resolve<CardBoxService>().Edit(dbEntry);

                return Json(new { code = 1, message });
            }
            catch (Exception ex)
            {
                return Json(new { code = -1, message = ex.Message });
            }
        }
        #endregion

        #region 删除
        [HttpPost]
        public JsonResult Delete(int id)
        {
            try
            {
                CardBox dbEntry = Container.Instance.Resolve<CardBoxService>().GetEntity(id);
                Container.Instance.Resolve<CardBoxService>().Delete(id);
                string message = string.Format("{0} 删除成功", dbEntry.Name);
                TempData["message"] = message;

                return Json(new { code = 1, message });
            }
            catch (Exception ex)
            {
                return Json(new { code = -1, message = ex.Message });
            }
        }
        #endregion
    }
}
