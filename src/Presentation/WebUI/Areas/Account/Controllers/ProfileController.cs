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
        #endregion

        #region Ctor
        public ProfileController(IUserInfoService userInfoService)
        {
            ViewBag.PageHeader = "个人中心";
            ViewBag.PageHeaderDescription = "";
            ViewBag.BreadcrumbList = new List<BreadcrumbItem>
            {
                new BreadcrumbItem("个人中心"),
            };

            this._userInfoService = userInfoService;
        }
        #endregion

        #region 个人中心首页
        public ActionResult Index(string userName = null)
        {
            if (userName == null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            // 此主页对应的UserInfo
            UserInfo viewModel = AccountManager.GetUserInfoByUserName(userName);
            if (viewModel == null)
            {
                // 不存在此用户
                return new View_NotExistAccountResult();
            }

            return View(viewModel);
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