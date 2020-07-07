using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Areas.Admin.Models.SEOVM;
using WebUI.Extensions;

namespace WebUI.Areas.Admin.Controllers
{
    public class SEOController : Controller
    {
        private readonly ISettingService _settingService;

        public SEOController(ISettingService settingService)
        {
            this._settingService = settingService;
        }

        #region 首页设置
        [HttpGet]
        public ViewResult Index()
        {
            SEOViewModel viewModel = new SEOViewModel();

            string title = _settingService.GetSet("SEO.Home.Index.Title");
            string desc = _settingService.GetSet("SEO.Home.Index.Desc");
            string keywords = _settingService.GetSet("SEO.Home.Index.Keywords");

            viewModel.HomeTitle = title;
            viewModel.HomeDesc = desc;
            viewModel.HomeKeywords = keywords;

            return View(viewModel);
        }

        [HttpPost]
        public JsonResult Index(SEOViewModel inputModel)
        {
            try
            {
                // 数据格式效验
                if (ModelState.IsValid)
                {

                    #region 数据有效效验

                    #endregion

                    try
                    {
                        _settingService.Set("SEO.Home.Index.Title", inputModel.HomeTitle);
                        _settingService.Set("SEO.Home.Index.Desc", inputModel.HomeDesc);
                        _settingService.Set("SEO.Home.Index.Keywords", inputModel.HomeKeywords);
                    }
                    catch (Exception ex)
                    {
                        return Json(new { code = -1, message = "保存失败" });
                    }

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

        #region 文章页设置
        [HttpGet]
        public ViewResult Article()
        {
            SEOViewModel viewModel = new SEOViewModel();
            viewModel.ArticleTitle = _settingService.GetSet("SEO.Article.Index.Title");
            viewModel.ArticleDesc = _settingService.GetSet("SEO.Article.Index.Desc");
            viewModel.ArticleKeywords = _settingService.GetSet("SEO.Article.Index.Keywords");

            return View(viewModel);
        }

        [HttpPost]
        public JsonResult Article(SEOViewModel inputModel)
        {
            try
            {
                // 数据格式效验
                if (ModelState.IsValid)
                {

                    #region 数据有效效验

                    #endregion

                    try
                    {
                        _settingService.Set("SEO.Article.Index.Title", inputModel.ArticleTitle);
                        _settingService.Set("SEO.Article.Index.Desc", inputModel.ArticleDesc);
                        _settingService.Set("SEO.Article.Index.Keywords", inputModel.ArticleKeywords);
                    }
                    catch (Exception ex)
                    {
                        return Json(new { code = -1, message = "保存失败" });
                    }

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


    }
}