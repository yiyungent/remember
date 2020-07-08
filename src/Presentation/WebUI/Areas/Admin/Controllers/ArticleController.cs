using Core;
using Core.Common;
using Domain;
using Domain.Entities;
using Framework.Attributes;
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

namespace WebUI.Areas.Admin.Controllers
{
    //[XSSFilter]
    public class ArticleController : Controller
    {
        #region Fields
        private readonly IArticleService _articleService;
        private readonly ICatInfoService _catInfoService;
        private readonly IArticle_CatService _article_CatService;
        #endregion

        #region Ctor
        public ArticleController(IArticleService articleService, ICatInfoService catInfoService, IArticle_CatService article_CatService)
        {
            this._articleService = articleService;
            this._catInfoService = catInfoService;
            this._article_CatService = article_CatService;
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
            viewModel.CustomUrl = viewModel.CustomUrl.Replace($"u{viewModel.AuthorId}/", "").Replace(".html", "");
            CatInfo selectedCat = _article_CatService.Find(m => m.ArticleId == viewModel.ID).CatInfo;
            IList<CatInfo> catInfos = _catInfoService.All().ToList();
            ViewBag.SelectedCat = selectedCat;
            ViewBag.CatInfos = catInfos;

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
                    IList<CatInfo> catInfos = _catInfoService.All().ToList();
                    int selectCatId = 0;
                    if (int.TryParse(Request.Form["CatId"], out selectCatId))
                    {
                        if (catInfos.Select(m => m.ID).Contains(selectCatId))
                        {
                            selectCatId = int.Parse(Request.Form["CatId"]);
                        }
                        else
                        {
                            return Json(new { code = -1, message = "添加失败,分区不存在" });
                        }
                    }
                    else
                    {
                        return Json(new { code = -1, message = "添加失败,分区ID为整数" });
                    }
                    Article dbModel = this._articleService.Find(m => m.ID == inputModel.ID && !m.IsDeleted);
                    string fullCustomUrl = $"u{dbModel.AuthorId}/{inputModel.CustomUrl}.html";
                    bool isExist = this._articleService.Contains(m => m.CustomUrl == fullCustomUrl && m.ID != inputModel.ID);
                    if (isExist)
                    {
                        return Json(new { code = -1, message = "添加失败,自定义URL已存在，请更改" });
                    }
                    #endregion

                    // 输入模型->数据库模型
                    dbModel.Title = inputModel.Title;
                    dbModel.Content = inputModel.Content;
                    dbModel.CustomUrl = fullCustomUrl;
                    dbModel.LastUpdateTime = DateTime.Now;

                    this._articleService.Update(dbModel);
                    // 更新分区s
                    Article_Cat article_Cat = this._article_CatService.Find(m => m.ArticleId == dbModel.ID);
                    article_Cat.ArticleId = inputModel.ID;
                    article_Cat.CatInfoId = selectCatId;
                    article_Cat.CreateTime = DateTime.Now.ToTimeStamp10();
                    this._article_CatService.Update(article_Cat);

                    return Json(new { code = 1, message = "保存成功" });
                }
                else
                {
                    string errorMessage = "保存失败";
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
            int userId = AccountManager.GetCurrentAccount().UserId;
            viewModel.AuthorId = userId;
            viewModel.CustomUrl = Guid.NewGuid().ToString().Substring(0, 7);

            IList<CatInfo> catInfos = _catInfoService.All().ToList();
            ViewBag.CatInfos = catInfos;
            ViewBag.DefaultSelectedCat = catInfos.Where(m => m.Parent != null).FirstOrDefault();

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
                    IList<CatInfo> catInfos = _catInfoService.All().ToList();
                    int selectCatId = 0;
                    if (int.TryParse(Request.Form["CatId"], out selectCatId))
                    {
                        if (catInfos.Select(m => m.ID).Contains(selectCatId))
                        {
                            selectCatId = int.Parse(Request.Form["CatId"]);
                        }
                        else
                        {
                            return Json(new { code = -1, message = "添加失败,分区不存在" });
                        }
                    }
                    else
                    {
                        return Json(new { code = -1, message = "添加失败,分区ID为整数" });
                    }
                    int userId = AccountManager.GetCurrentAccount().UserId;
                    string fullCustomUrl = $"u{userId}/{inputModel.CustomUrl}.html";
                    bool isExist = this._articleService.Contains(m => m.CustomUrl == fullCustomUrl);
                    if (isExist)
                    {
                        return Json(new { code = -1, message = "添加失败,自定义URL已存在，请更改" });
                    }
                    #endregion

                    Article dbModel = inputModel;
                    dbModel.AuthorId = userId;
                    dbModel.CreateTime = DateTime.Now;
                    dbModel.LastUpdateTime = DateTime.Now;
                    dbModel.CustomUrl = fullCustomUrl;
                    this._articleService.Create(dbModel);
                    this._article_CatService.Create(new Article_Cat
                    {
                        ArticleId = dbModel.ID,
                        CatInfoId = selectCatId,
                        CreateTime = DateTime.Now.ToTimeStamp10()
                    });

                    return Json(new { code = 1, message = "添加成功" });
                }
                else
                {
                    return Json(new { code = -1, message = "添加失败" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = -2, message = "添加失败" });
            }
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

                string imgUrl = ($"/Upload/images/{currentUserId}/{DateTime.Now.ToString("yyyy-MM-dd")}/" + saveFileName);
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