using AutoMapperConfig;
using Domain.Entities;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// 文章服务接口
        /// </summary>
        private readonly IArticleService _articleService;

        public HomeController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        public ActionResult Index()
        {
            var viewModel = _articleService.FindHomePageArticles(4);

            return View(viewModel);
        }

        public ActionResult Details(int id)
        {
            var viewModel = _articleService.Find(id).ToModel();

            return View(viewModel);
        }

        public JsonResult Edit(string content)
        {
            var article = _articleService.Find(1);
            article.Content = content;

            _articleService.Update(article);

            return Json("成功", JsonRequestBehavior.AllowGet);
        }

        public string TestFk()
        {
            UserInfo user = _articleService.Find(1).Author;
            Article article = _articleService.Find(2);

            var query  = _articleService.All();
            var list = query.ToList();

            return user.Avatar;
        }
    }
}