using Core;
using Domain;
using Domain.Entities;
using Framework.Infrastructure.Concrete;
using Framework.Models;
using Framework.Mvc;
using Framework.RequestResult;
using Services;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Areas.Account.Models;
using WebUI.Areas.Admin.Models;
using WebUI.Areas.Admin.Models.Common;

namespace WebUI.Areas.Account.Controllers
{
    public class ProfileController : Controller
    {
        #region Fields
        private IUserInfoService _userInfoService;
        private readonly IArticleService _articleService;
        #endregion

        #region Ctor
        public ProfileController(IUserInfoService userInfoService, IArticleService articleService)
        {
            ViewBag.PageHeader = "个人中心";
            ViewBag.PageHeaderDescription = "";
            ViewBag.BreadcrumbList = new List<BreadcrumbItem>
            {
                new BreadcrumbItem("个人中心"),
            };

            this._userInfoService = userInfoService;
            this._articleService = articleService;
        }
        #endregion

        #region 个人中心首页
        public ActionResult Index(string userName = null, int pageIndex = 1)
        {
            if (userName == null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            // 此主页对应的UserInfo
            UserInfo viewModel = AccountManager.GetUserInfoByUserName(userName);
            int authorId = viewModel.ID;
            if (viewModel == null)
            {
                // 不存在此用户
                return new View_NotExistAccountResult();
            }

            int pageSize = 6;
            pageIndex = pageIndex >= 1 ? pageIndex : 1;
            Query(pageIndex, pageSize, out IList<Article> list, out int totalCount, authorId);

            ListViewModel<Article> articles = new ListViewModel<Article>(list, pageIndex: pageIndex, pageSize: pageSize, totalCount: totalCount);
            ViewBag.ArticleVM = articles;

            return View(viewModel);
        }

        private void Query(int pageIndex, int pageSize, out IList<Article> list, out int totalCount, int authorId)
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
                    list = this._articleService.Filter<int>(pageIndex, pageSize, out totalCount, m => m.Title.Contains(query) && m.AuthorId == authorId && !m.IsDeleted, m => m.ID, false).ToList();
                    break;
                case "id":
                    queryType.Text = "ID";
                    if (int.TryParse(query, out int queryId))
                    {
                        list = this._articleService.Filter<int>(pageIndex, pageSize, out totalCount, m => m.ID == queryId && m.AuthorId == authorId && !m.IsDeleted, m => m.ID, false).ToList();
                    }
                    else if (string.IsNullOrEmpty(query))
                    {
                        list = this._articleService.Filter<int>(pageIndex, pageSize, out totalCount, m => m.AuthorId == authorId && !m.IsDeleted, m => m.ID, false).ToList();
                    }
                    else
                    {
                        list = new List<Article>();
                        totalCount = 0;
                    }
                    break;
                default:
                    queryType.Text = "标题";
                    list = this._articleService.Filter<int>(pageIndex, pageSize, out totalCount, m => m.Title.Contains(query) && m.AuthorId == authorId && !m.IsDeleted, m => m.ID, false).ToList();
                    break;
            }
            ViewBag.Query = query;
            ViewBag.QueryType = queryType;
        }
        #endregion

        #region 保存个人设置
        /// <summary>
        /// 保存(更新)当前登录用户-个人设置
        /// </summary>
        public JsonResult Settings(EditUserInfoViewModel inputModel)
        {
            try
            {
                CurrentAccountModel currentLoginAccount = AccountManager.GetCurrentAccount();
                UserInfo currentLoginUserInfo = AccountManager.GetCurrentUserInfo();
                if (currentLoginAccount.IsGuest)
                {
                    return Json(new { code = -2, message = "保存失败, 当前未登录" });
                }

                #region 数据有效性效验
                bool isExistUserName = this._userInfoService.Contains(m => m.UserName == inputModel.InputUserName && m.ID != currentLoginUserInfo.ID);
                if (isExistUserName)
                {
                    return Json(new { code = -4, message = "保存失败, 此用户名已被使用" });
                }
                #endregion

                // 为取到最新数据，从数据库中拿
                UserInfo dbModel = this._userInfoService.Find(m =>
                    m.UserName == currentLoginUserInfo.UserName
                    && !m.IsDeleted
                );

                dbModel.Email = inputModel.InputEmail;
                dbModel.UserName = inputModel.InputUserName;
                dbModel.Description = inputModel.InputDescription;

                this._userInfoService.Update(dbModel);

                return Json(new { code = 1, message = "保存成功" });
            }
            catch (Exception ex)
            {
                return Json(new { code = -1, message = "保存失败" });
            }
        }
        #endregion


    }
}