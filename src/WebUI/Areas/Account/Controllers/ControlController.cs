using Core;
using Domain;
using Framework.Infrastructure.Concrete;
using Framework.Models;
using Framework.Mvc;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Areas.Account.Controllers
{
    public class ControlController : Controller
    {

        #region loadProfileBox
        public PartialViewResult ProfileBoxPartial(string userName)
        {
            UserInfo model = null;
            if (userName == "guest")
            {
                model = UserInfo_Guest.Instance;
            }
            else
            {
                model = AccountManager.GetUserInfoByUserName(userName);
                if (model == null)
                {
                    model = new UserInfo
                    {
                        Name = "用户不存在",
                        Avatar = "/images/notexist-avatar.jpg",
                        RoleInfoList = new List<RoleInfo>()
                    };
                }
            }

            return PartialView("_ProfileBoxPartial", model);
        }
        #endregion


    }
}