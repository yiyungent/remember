using Core;
using Domain;
using Domain.Entities;
using Framework.Extensions;
using Framework.Infrastructure.Concrete;
using Services;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
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
                    list = this._articleService.Filter<int>(pageIndex, pageSize, out totalCount, m => m.Title.Contains(query) && !m.IsDeleted, m => m.ID, false).ToList();
                    break;
                case "id":
                    queryType.Text = "ID";
                    int articleId = int.Parse(query);
                    list = this._articleService.Filter<int>(pageIndex, pageSize, out totalCount, m => m.ID == articleId && !m.IsDeleted, m => m.ID, false).ToList();
                    break;
                default:
                    queryType.Text = "标题";
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
            Article viewModel = this._articleService.Find(m => m.ID == id && !m.IsDeleted);

            return View(viewModel);
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

                    Article dbModel = this._articleService.Find(m => m.ID == inputModel.ID && !m.IsDeleted);

                    // 输入模型->数据库模型
                    dbModel.Title = inputModel.Title;
                    dbModel.Content = inputModel.Content;
                    dbModel.CustomUrl = inputModel.CustomUrl;
                    dbModel.LastUpdateTime = DateTime.Now;

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
                var dbModel = this._articleService.Find(m => m.ID == id && !m.IsDeleted);
                dbModel.IsDeleted = true;
                dbModel.DeletedAt = DateTime.Now;
                this._articleService.Update(dbModel);


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

        #region 添加
        [HttpGet]
        public ViewResult Create()
        {
            Article viewModel = new Article();
            DateTime now = DateTime.Now;
            viewModel.CustomUrl = $"article-{now.Year}-{now.Month}-{now.Day}-" + Guid.NewGuid().ToString().Substring(0, 8) + ".html";

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
                    dbModel.AuthorId = AccountManager.GetCurrentAccount().UserId;
                    dbModel.CreateTime = DateTime.Now;
                    dbModel.LastUpdateTime = DateTime.Now;
                    dbModel.CustomUrl = inputModel.CustomUrl;

                    this._articleService.Create(dbModel);

                    // TODO: 添加搜索索引
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
            Article viewModel = (Article)System.Web.HttpContext.Current.Items["CmsPage"];

            return View(viewModel);
        }
        #endregion

        #region 上传图片
        [HttpPost]
        public JsonResult UploadImg()
        {
            try
            {
                int currentUserId = AccountManager.GetCurrentAccount().UserId;

                // 保存到当前用户的文件夹
                string basePath = $"~/Upload/images/{currentUserId}/{DateTime.Now.ToString("yyyy-MM-dd")}/";

                // 如果路径含有~，即需要服务器映射为绝对路径，则进行映射
                basePath = (basePath.IndexOf("~") > -1) ? System.Web.HttpContext.Current.Server.MapPath(basePath) : basePath;
                HttpPostedFile file = System.Web.HttpContext.Current.Request.Files[0];
                // 如果目录不存在，则创建目录
                if (!Directory.Exists(basePath))
                {
                    Directory.CreateDirectory(basePath);
                }

                string fileName = System.Web.HttpContext.Current.Request["name"];
                if (string.IsNullOrEmpty(fileName))
                {
                    fileName = file.FileName;
                }
                // 文件保存
                string saveFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string fullPath = basePath + saveFileName;
                file.SaveAs(fullPath);

                string imgUrl = ($"/Upload/images/{currentUserId}/{DateTime.Now.ToString("yyyy-MM-dd")}/" + saveFileName).ToHttpAbsoluteUrl();
                return Json(new WangEditorUploadImgResult
                {
                    errno = 0,
                    data = new string[] { imgUrl }
                });
            }
            catch (Exception ex)
            {
                return Json(new WangEditorUploadImgResult
                {
                    errno = -1,
                    data = new string[] { }
                });
            }
        }
        #endregion

        public sealed class WangEditorUploadImgResult
        {

            /// <summary>
            /// errno 即错误代码，0 表示没有错误。
            /// 如果有错误，errno != 0，可通过下文中的监听函数 fail 拿到该错误码进行自定义处理
            /// </summary>
            public int errno { get; set; }

            /// <summary>
            /// data 是一个数组，返回若干图片的线上地址
            /// </summary>
            public string[] data { get; set; }
        }
    }
}