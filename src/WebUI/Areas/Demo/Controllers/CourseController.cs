using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Areas.Demo.Controllers
{
    public class CourseController : Controller
    {
        // GET: Demo/Course
        public ActionResult Index()
        {
            return View();
        }

        public ViewResult Section(int id)
        {
            VideoInfo viewModel = new VideoInfo();

            return View(viewModel);
        }
    }
}