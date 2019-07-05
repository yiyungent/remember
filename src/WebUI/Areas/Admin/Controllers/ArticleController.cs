using Core;
using Domain;
using Framework.Infrastructure.Concrete;
using NHibernate.Criterion;
using Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Areas.Admin.Models.Common;
using WebUI.Extensions;
using WebUI.Infrastructure.Search;
using WebUI.Models.SearchVM;

namespace WebUI.Areas.Admin.Controllers
{
    public class ArticleController : Controller
    {
        #region Fields
        private ArticleService _articleService; 
        #endregion

        #region Ctor
        public ArticleController()
        {
            this._articleService = Container.Instance.Resolve<ArticleService>();
        } 
        #endregion

        #region 列表
        public ActionResult Index(int pageIndex = 1, int pageSize = 6)
        {
            IList<ICriterion> queryConditions = new List<ICriterion>();
            Query(queryConditions);

            ListViewModel<Article> viewModel = new ListViewModel<Article>(queryConditions, pageIndex: pageIndex, pageSize: pageSize);
            TempData["RedirectUrl"] = Request.RawUrl;

            return View(viewModel);
        }

        private void Query(IList<ICriterion> queryConditions)
        {
            // 输入的查询关键词
            string query = Request["q"]?.Trim() ?? "";
            // 查询类型
            QueryType queryType = new QueryType();
            queryType.Val = Request["type"]?.Trim() ?? "title";
            switch (queryType.Val.ToLower())
            {
                case "title":
                    queryType.Text = "标题";
                    queryConditions.Add(Expression.Like("Title", query, MatchMode.Anywhere));
                    break;
                case "id":
                    queryType.Text = "ID";
                    queryConditions.Add(Expression.Eq("ID", int.Parse(query)));
                    break;
                default:
                    queryType.Text = "标题";
                    queryConditions.Add(Expression.Like("Title", query, MatchMode.Anywhere));
                    break;
            }
            ViewBag.Query = query;
            ViewBag.QueryType = queryType;
        }
        #endregion

        #region 编辑
        [HttpGet]
        public ViewResult Edit(int id)
        {
            Article dbModel = Container.Instance.Resolve<ArticleService>().GetEntity(id);

            return View(dbModel);
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Edit(Article inputModel)
        {
            try
            {
                // 数据格式效验
                if (ModelState.IsValid)
                {

                    #region 数据有效效验



                    #endregion

                    ArticleService articleService = Container.Instance.Resolve<ArticleService>();
                    Article dbModel = articleService.GetEntity(inputModel.ID);

                    // 输入模型->数据库模型
                    dbModel.Title = inputModel.Title;
                    dbModel.Content = inputModel.Content;
                    dbModel.CustomUrl = inputModel.CustomUrl;
                    dbModel.LastUpdateTime = DateTime.Now;

                    articleService.Edit(dbModel);

                    // 添加到队列-新建此文章索引 -- 不需要先删除，因为 SearchIndexManager 会先删除此ID的索引，再新建
                    SearchIndexManager.GetInstance().AddQueue(inputModel.ID.ToString(), inputModel.Title, inputModel.Content, inputModel.PublishTime);

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

        #region 删除
        [HttpPost]
        public JsonResult Delete(int id)
        {
            try
            {
                Container.Instance.Resolve<ArticleService>().Delete(id);

                // 添加到队列-删除此文章索引
                SearchIndexManager.GetInstance().DeleteQueue(id.ToString());

                return Json(new { code = 1, message = "删除成功" });
            }
            catch (Exception ex)
            {
                return Json(new { code = 1, message = "删除失败" });
            }
        }
        #endregion

        #region 新增
        [HttpGet]
        public ViewResult Create()
        {
            Article viewModel = new Article();
            viewModel.CustomUrl = "article-" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day;

            return View(viewModel);
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Create(Article inputModel)
        {
            try
            {
                // 数据格式效验
                if (ModelState.IsValid)
                {
                    #region 数据有效效验

                    #endregion

                    Article dbModel = inputModel;
                    dbModel.Author = new UserInfo { ID = AccountManager.GetCurrentUserInfo().ID };
                    dbModel.PublishTime = DateTime.Now;
                    dbModel.LastUpdateTime = DateTime.Now;
                    dbModel.CustomUrl = inputModel.CustomUrl;

                    _articleService.Create(dbModel);
                    int lastId = _articleService.GetLastId();

                    // 添加到队列-新建此文章索引
                    SearchIndexManager.GetInstance().AddQueue(lastId.ToString(), inputModel.Title, inputModel.Content, inputModel.PublishTime);

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

        #region 用于自定义Url的文章内容展示
        public ActionResult Page()
        {
            Article dbModel = (Article)System.Web.HttpContext.Current.Items["CmsPage"];

            return View(dbModel);
        }
        #endregion
    }
}