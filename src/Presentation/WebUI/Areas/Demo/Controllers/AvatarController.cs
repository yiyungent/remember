using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Areas.Demo.Controllers
{
#if DEBUG
    public class AvatarController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        #region 头像上传
        public ActionResult Upload()
        {
            return View();
        }
        #endregion
    } 
#endif
}