using Core;
using Domain;
using Framework.Infrastructure.Concrete;
using NHibernate.Criterion;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class CourseBoxController : Controller
    {
        public ActionResult Index(int id)
        {
            CourseBox viewModel = Container.Instance.Resolve<CourseBoxService>().GetEntity(id);

            IList<Comment> comments = Container.Instance.Resolve<CourseBox_CommentService>().Query(new List<ICriterion>
            {
                Expression.Eq("CourseBox.ID", id)
            }).Select(m => m.Comment).OrderByDescending(m => m.CreateTime).ToList();
            ViewBag.Comments = comments;

            IList<CourseBox> recomCourseBoxes = Container.Instance.Resolve<CourseBoxService>().GetAll().OrderByDescending(m => m.LikeNum).Take(10).ToList();
            ViewBag.RecomCourseBoxes = recomCourseBoxes;
            ViewBag.CurrentUser = AccountManager.GetCurrentUserInfo(true);


            return View(viewModel);
        }

        [HttpPost]
        public JsonResult Comment(int courseBoxId, string content)
        {
            try
            {
                UserInfo author = AccountManager.GetCurrentUserInfo();

                Container.Instance.Resolve<CourseBox_CommentService>().Create(new CourseBox_Comment
                {
                    Comment = new Comment
                    {
                        Author = author,
                        CreateTime = DateTime.Now,
                        LastUpdateTime = DateTime.Now,
                        Content = content
                    },
                    CourseBox = new CourseBox { ID = courseBoxId }
                });

                return Json(new { code = 1, message = "评论成功" });
            }
            catch (Exception ex)
            {
                return Json(new { code = -1, message = "评论失败" });
            }
        }
    }
}