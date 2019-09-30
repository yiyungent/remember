using AutoMapperConfig;
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
    }
}