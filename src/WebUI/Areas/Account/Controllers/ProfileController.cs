using Core;
using Domain;
using Framework.Infrastructure.Concrete;
using Framework.Models;
using Framework.Mvc;
using Framework.RequestResult;
using NHibernate.Criterion;
using Service;
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
        #region Ctor
        public ProfileController()
        {
            ViewBag.PageHeader = "个人中心";
            ViewBag.PageHeaderDescription = "";
            ViewBag.BreadcrumbList = new List<BreadcrumbItem>
            {
                new BreadcrumbItem("个人中心"),
            };
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
            UserInfo model = AccountManager.GetUserInfoByUserName(userName);
            if (model == null)
            {
                // 不存在此用户
                return new View_NotExistAccountResult();
            }

            return View(model);
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
                UserInfo currentLoginUserInfo = currentLoginAccount.UserInfo;
                if (currentLoginAccount.IsGuest)
                {
                    return Json(new { code = -2, message = "保存失败, 当前未登录" });
                }

                // 为取到最新数据，从数据库中拿
                UserInfo dbUserInfo = Container.Instance.Resolve<UserInfoService>().Query(new List<ICriterion>
                {
                    Expression.Eq("UserName", currentLoginUserInfo.UserName)
                }).FirstOrDefault();

                dbUserInfo.Name = inputModel.InputName;
                dbUserInfo.Email = inputModel.InputEmail;
                dbUserInfo.Description = inputModel.InputDescription;

                Container.Instance.Resolve<UserInfoService>().Edit(dbUserInfo);
                // 更新 Session 中登录用户信息
                AccountManager.UpdateSessionAccount();

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