using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Areas.Demo.Controllers
{
#if DEBUG
    public class BookController : Controller
    {
        // GET: Demo/Book
        public ActionResult Index()
        {
            return View();
        }
    } 
#endif
}