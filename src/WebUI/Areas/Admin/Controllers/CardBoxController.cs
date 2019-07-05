using Core;
using Domain;
using NHibernate.Criterion;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Areas.Admin.Models.CardBoxVM;
using WebUI.Areas.Admin.Models.Common;
using WebUI.Extensions;

namespace WebUI.Areas.Admin.Controllers
{
    public class CardBoxController : Controller
    {
        #region Ctor
        public CardBoxController()
        { }
        #endregion

        #region 列表
        public ViewResult Index(int pageIndex = 1, int pageSize = 6)
        {
            IList<ICriterion> queryConditions = new List<ICriterion>();
            Query(queryConditions);

            ListViewModel<CardBox> viewModel = new ListViewModel<CardBox>(queryConditions, pageIndex: pageIndex, pageSize: pageSize);
            TempData["RedirectUrl"] = Request.RawUrl;

            return View(viewModel);
        }

        private void Query(IList<ICriterion> queryConditions)
        {
            // 输入的查询关键词
            string query = Request["q"]?.Trim() ?? "";
            // 查询类型
            QueryType queryType = new QueryType();
            queryType.Val = Request["type"]?.Trim() ?? "username";
            switch (queryType.Val.ToLower())
            {
                case "username":
                    queryType.Text = "用户名";
                    queryConditions.Add(Expression.Like("UserName", query, MatchMode.Anywhere));
                    break;
                case "name":
                    queryType.Text = "展示名";
                    queryConditions.Add(Expression.Like("Name", query, MatchMode.Anywhere));
                    break;
                case "id":
                    queryType.Text = "ID";
                    queryConditions.Add(Expression.Eq("ID", int.Parse(query)));
                    break;
                default:
                    queryType.Text = "用户名";
                    queryConditions.Add(Expression.Like("UserName", query, MatchMode.Anywhere));
                    break;
            }
            ViewBag.Query = query;
            ViewBag.QueryType = queryType;
        }
        #endregion

        #region 删除
        public JsonResult Delete(int id)
        {
            try
            {
                Container.Instance.Resolve<CardBoxService>().Delete(id);

                return Json(new { code = 1, message = "删除成功" });
            }
            catch (Exception ex)
            {
                return Json(new { code = 1, message = "删除失败" });
            }
        }
        #endregion

        #region 查看
        public ViewResult Detail(int id)
        {
            CardBox viewModel = Container.Instance.Resolve<CardBoxService>().GetEntity(id);

            return View(viewModel);
        }
        #endregion

        #region 编辑
        [HttpGet]
        public ViewResult Edit(int id)
        {
            CardBox dbModel = Container.Instance.Resolve<CardBoxService>().GetEntity(id);
            CardBoxViewModel viewModel = (CardBoxViewModel)dbModel;

            return View(viewModel);
        }

        [HttpPost]
        public JsonResult Edit(CardBoxViewModel inputModel)
        {
            try
            {
                // 数据格式效验
                if (ModelState.IsValid)
                {

                    #region 数据有效效验



                    #endregion

                    // 输入模型->数据库模型
                    CardBox dbModel = (CardBox)inputModel;

                    Container.Instance.Resolve<CardBoxService>().Edit(dbModel);

                    return Json(new { code = 1, message = "保存成功" });
                }
                else
                {
                    string errorMessage = ModelState.GetErrorMessage();
                    return Json(new { code = -1, message = errorMessage });
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = -2, message = "保存失败" });
            }
        }
        #endregion

        #region 新增
        [HttpGet]
        public ViewResult Create()
        {
            CardBoxViewModel viewModel = new CardBoxViewModel();

            return View(viewModel);
        }

        [HttpPost]
        public JsonResult Create(CardBoxViewModel inputModel)
        {
            try
            {
                // 数据格式效验
                if (ModelState.IsValid)
                {
                    #region 数据有效效验

                    #endregion

                    CardBox dbModel = (CardBox)inputModel;

                    Container.Instance.Resolve<CardBoxService>().Create(dbModel);

                    return Json(new { code = 1, message = "添加成功" });
                }
                else
                {
                    string errorMessage = ModelState.GetErrorMessage();
                    return Json(new { code = -1, message = errorMessage });
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = -2, message = "添加失败" });
            }
        }
        #endregion
    }
}