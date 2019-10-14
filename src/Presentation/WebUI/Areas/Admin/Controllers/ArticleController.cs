using Core;
using Domain;
using Domain.Entities;
using Framework.Infrastructure.Concrete;
using Services;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Areas.Admin.Models.Common;
using WebUI.Extensions;
using WebUI.Models.SearchVM;

namespace WebUI.Areas.Admin.Controllers
{
    public class ArticleController : Controller
    {
        #region Fields
        private readonly IArticleService _articleService;
        #endregion

        #region Ctor
        public ArticleController(IArticleService articleService)
        {
            this._articleService = articleService;
        }
        #endregion

        #region 列表
        public ActionResult Index(int pageIndex = 1, int pageSize = 6)
        {
            Query(pageIndex, pageSize, out IList<Article> list, out int totalCount);

            ListViewModel<Article> viewModel = new ListViewModel<Article>(list, pageIndex: pageIndex, pageSize: pageSize, totalCount: totalCount);
            TempData["RedirectUrl"] = Request.RawUrl;

            return View(viewModel);
        }

        private void Query(int pageIndex, int pageSize, out IList<Article> list, out int totalCount)
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
                    //queryConditions.Add(Expression.Like("Title", query, MatchMode.Anywhere));
                    list = this._articleService.Filter<int>(pageIndex, pageSize, out totalCount, m => m.Title.Contains(query) && !m.IsDeleted, m => m.ID, false).ToList();
                    break;
                case "id":
                    queryType.Text = "ID";
                    //queryConditions.Add(Expression.Eq("ID", int.Parse(query)));
                    list = this._articleService.Filter<int>(pageIndex, pageSize, out totalCount, m => m.ID == int.Parse(query) && !m.IsDeleted, m => m.ID, false).ToList();
                    break;
                default:
                    queryType.Text = "标题";
                    //queryConditions.Add(Expression.Like("Title", query, MatchMode.Anywhere));
                    list = this._articleService.Filter<int>(pageIndex, pageSize, out totalCount, m => m.Title.Contains(query) && !m.IsDeleted, m => m.ID, false).ToList();
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
            //Article dbModel = Container.Instance.Resolve<ArticleService>().GetEntity(id);
            Article dbModel = this._articleService.Find(m => m.ID == id && !m.IsDeleted);

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

                    //ArticleService articleService = Container.Instance.Resolve<ArticleService>();
                    Article dbModel = this._articleService.Find(m => m.ID == inputModel.ID && !m.IsDeleted);

                    // 输入模型->数据库模型
                    dbModel.Title = inputModel.Title;
                    dbModel.Content = inputModel.Content;
                    dbModel.CustomUrl = inputModel.CustomUrl;
                    dbModel.LastUpdateTime = DateTime.Now;

                    //articleService.Edit(dbModel);
                    this._articleService.Update(dbModel);

                    // 添加到队列-新建此文章索引 -- 不需要先删除，因为 SearchIndexManager 会先删除此ID的索引，再新建
                    //SearchIndexManager.GetInstance().AddQueue(dbModel.ID.ToString(), dbModel.Title, dbModel.Content, dbModel.CreateTime, dbModel.CustomUrl);

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
                //Container.Instance.Resolve<ArticleService>().Delete(id);
                var dbModel = this._articleService.Find(m => m.ID == id && !m.IsDeleted);
                dbModel.IsDeleted = true;
                dbModel.DeletedAt = DateTime.Now;

                // 添加到队列-删除此文章索引
                //SearchIndexManager.GetInstance().DeleteQueue(id.ToString());

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
                    dbModel.AuthorId = AccountManager.GetCurrentUserInfo().ID;
                    dbModel.CreateTime = DateTime.Now;
                    dbModel.LastUpdateTime = DateTime.Now;
                    dbModel.CustomUrl = inputModel.CustomUrl;

                    this._articleService.Create(dbModel);
                    //int lastId = _articleService.GetLastId();

                    // 添加到队列-新建此文章索引
                    //SearchIndexManager.GetInstance().AddQueue(lastId.ToString(), dbModel.Title, dbModel.Content, dbModel.CreateTime, dbModel.CustomUrl);

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