using Domain.Entities;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class ArticleController : Controller
    {
        private readonly ISettingService _settingService;

        public ArticleController(ISettingService settingService)
        {
            this._settingService = settingService;
        }

        #region 用于自定义Url的文章内容展示
        public ActionResult Index()
        {
            Article viewModel = (Article)System.Web.HttpContext.Current.Items["CmsPage"];
            string webName = _settingService.GetSet("Web.Name");
            // SEO
            string articleTitle = _settingService.GetSet("SEO.Article.Index.Title");
            string articleKeywords = _settingService.GetSet("SEO.Article.Index.Keywords");
            string articleDesc = _settingService.GetSet("SEO.Article.Index.Desc");

            ViewBag.WebName = webName;
            // TODO: 根据Content(num)给的数字，截取num个字符
            ViewBag.Title = articleTitle
                .Replace("{{Web.Name}}", webName)
                .Replace("{{Article.Title}}", viewModel.Title)
                .Replace("{{Article.Content(30)}}", viewModel.Content.Substring(0, viewModel.Content.Length > 30 ? 30 : viewModel.Content.Length));
            ViewBag.Keywords = articleKeywords
                .Replace("{{Web.Name}}", webName)
                .Replace("{{Article.Title}}", viewModel.Title)
                .Replace("{{Article.Content(30)}}", viewModel.Content.Substring(0, viewModel.Content.Length > 30 ? 30 : viewModel.Content.Length));
            ViewBag.Desc = articleDesc
                .Replace("{{Web.Name}}", webName)
                .Replace("{{Article.Title}}", viewModel.Title)
                .Replace("{{Article.Content(30)}}", viewModel.Content.Substring(0, viewModel.Content.Length > 30 ? 30 : viewModel.Content.Length));

            return View(viewModel);
        }
        #endregion
    }
}