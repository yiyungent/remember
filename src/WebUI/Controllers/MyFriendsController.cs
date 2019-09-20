using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class MyFriendsController : Controller
    {
        // GET: MyFriends
        public ActionResult Index()
        {
            return View("~/Views/Home/Index.cshtml");
        }

        public ActionResult MyFollow()
        {
            return View("~/Views/Home/Index.cshtml");
        }

        public ActionResult MyFans()
        {
            return View("~/Views/Home/Index.cshtml");
        }
    }
}