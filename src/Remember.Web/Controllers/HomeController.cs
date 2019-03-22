using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Castle.ActiveRecord;
using Remember.Core;
using Remember.Service;
using Remember.Domain;

namespace Remember.Web.Controllers
{
    public class HomeController : Controller
    {
        #region 首页
        public ViewResult Index()
        {

            return View();
        } 
        #endregion
    }
}
