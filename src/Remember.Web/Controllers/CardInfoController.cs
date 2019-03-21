using Remember.Core;
using Remember.Domain;
using Remember.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Remember.Web.Attributes;

namespace Remember.Web.Controllers
{
    public class CardInfoController : Controller
    {
        #region 列表
        public ViewResult Index(int cardBoxId, int pageIndex = 1, int pageSize = 10)
        {
            ViewBag.CardBox = Container.Instance.Resolve<CardBoxService>().GetEntity(cardBoxId);

            IList<CardInfo> allList = Container.Instance.Resolve<CardInfoService>().GetAll().Where(m => m.CardBox.ID == cardBoxId).ToList();

            ViewBag.TotalCount = allList.Count;
            // 最后一页的页码
            int lastPageIndex = (int)Math.Ceiling((double)allList.Count / pageSize);
            // 若 pageIndex 超出范围，则显示最后一页
            pageIndex = pageIndex > lastPageIndex ? lastPageIndex : pageIndex;

            ViewBag.PageIndex = pageIndex;
            ViewBag.PageSize = pageSize;

            // 按 ID 倒序排列
            var currentPageList = (from m in allList
                                   orderby m.ID descending
                                   select m).Skip((pageIndex - 1) * pageSize).Take(pageSize);

            return View(currentPageList.ToList());
        }
        #endregion

        #region 新增
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="id">?cardBoxId=id</param>
        [HttpGet]
        public ViewResult Create(int id)
        {

            CardBox cardBox = Container.Instance.Resolve<CardBoxService>().GetEntity(id);
            ViewBag.CardBox = cardBox;

            return View();
        }

        [HttpPost]
        public JsonResult Create(CardInfo model)
        {
            try
            {
                model.CardBox = Container.Instance.Resolve<CardBoxService>().GetEntity(model.CardBox.ID);

                Container.Instance.Resolve<CardInfoService>().Create(model);
                string message = string.Format("{0} 新增一张卡片", model.CardBox.Name);
                TempData["message"] = message;

                return Json(new { code = 1, message });
            }
            catch (Exception ex)
            {
                return Json(new { code = -1, message = ex.Message });
            }
        }
        #endregion

        #region 修改
        [HttpGet]
        public ViewResult Edit(int id)
        {
            CardInfo model = Container.Instance.Resolve<CardInfoService>().GetEntity(id);

            return View(model);
        }

        [HttpPost]
        public JsonResult Edit(CardInfo model)
        {
            try
            {
                CardInfo dbEntry = Container.Instance.Resolve<CardInfoService>().GetEntity(model.ID);
                model.CardBox = dbEntry.CardBox;

                Container.Instance.Resolve<CardInfoService>().Edit(model);

                string message = string.Format("卡片 {0} 修改成功", model.ID);
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
            CardInfo model = Container.Instance.Resolve<CardInfoService>().GetEntity(id);

            return View(model);
        }
        #endregion

        #region 删除
        [HttpPost]
        public JsonResult Delete(int id)
        {
            try
            {
                Container.Instance.Resolve<CardInfoService>().Delete(id);

                string message = string.Format("卡片 {0} 删除成功", id);
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
