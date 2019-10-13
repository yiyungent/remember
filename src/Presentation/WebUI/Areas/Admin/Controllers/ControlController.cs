using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Areas.Admin.Controllers
{
    /// <summary>
    /// 公共常用控件
    /// </summary>
    public class ControlController : Controller
    {
        public PartialViewResult MessagePartial()
        {
            return PartialView("_MessagePartial");
        }
    }
}