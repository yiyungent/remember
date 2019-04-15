using Remember.Core;
using Remember.Domain;
using Remember.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Remember.Web.Attributes;
using NHibernate.Criterion;
using Remember.Web.Controllers;

namespace Remember.Web.Controllers
{
    public class CardInfoController : Controller
    {
        #region 列表
        public ViewResult Index(int pageIndex = 1, string keyword = "")
        {
            int pageSize = 2;
            IList<ICriterion> conditionList = new List<ICriterion>();
            // 注意:如果首次(未传keyword)加载，那么keyword为"", like "" 为 所有
            conditionList.Add(Expression.Like("Content", keyword.Trim(), MatchMode.Anywhere));
            // 所有满足条件的(若无关键词条件则为所有)
            IList<CardInfo> allList = Container.Instance.Resolve<CardInfoService>().Query(conditionList);

            #region 分页处理
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
            #endregion

            return View(currentPageList.ToList());
        }
        #endregion

        #region 新增
        /// <summary>
        /// 新增
        /// </summary>
        [HttpGet]
        public ViewResult Create()
        {
            // 1.准备实体
            CardBox mo = new CardBox();
            // 2.返回前预处理
            ViewBag.DDLCardBox = InitDDLForCardBox(0);

            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
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
        [ValidateInput(false)]
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

        #region 辅助方法
        /// <summary>
        /// 初始化下拉备选项-卡片盒
        /// </summary>
        private IList<SelectListItem> InitDDLForCardBox(int selectedValue)
        {
            IList<SelectListItem> ret = new List<SelectListItem>();
            ret.Add(new SelectListItem()
            {
                Text = "请选择卡片盒",
                Value = "0",
                Selected = (selectedValue == 0)
            });
            IList<CardBox> all = Container.Instance.Resolve<CardBoxService>().GetAll();
            foreach (var item in all)
            {
                ret.Add(new SelectListItem()
                {
                    Text = item.Name,
                    Value = item.ID.ToString(),
                    Selected = (selectedValue == item.ID)
                });
            }

            return ret;
        }
        #endregion
    }
}
