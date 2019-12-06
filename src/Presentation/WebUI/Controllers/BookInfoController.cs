using Core;
using Domain;
using Domain.Entities;
using Framework.Infrastructure.Concrete;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class BookInfoController : Controller
    {
        #region Fields
        private readonly IBookInfoService _bookInfoService;
        private readonly ICommentService _commentService;
        private readonly IBookInfo_CommentService _bookInfo_CommentService;
        #endregion

        #region Ctor
        public BookInfoController(IBookInfoService courseBoxService, ICommentService commentService, IBookInfo_CommentService courseBox_CommentService)
        {
            this._bookInfoService = courseBoxService;
            this._commentService = commentService;
            this._bookInfo_CommentService = courseBox_CommentService;
        }
        #endregion

        public ActionResult Index(int id)
        {
            //CourseBox viewModel = Container.Instance.Resolve<CourseBoxService>().GetEntity(id);
            BookInfo viewModel = this._bookInfoService.Find(m => m.ID == id && !m.IsDeleted);

            //IList<Comment> comments = Container.Instance.Resolve<CourseBox_CommentService>().Query(new List<ICriterion>
            //{
            //    Expression.Eq("CourseBox.ID", id)
            //}).Select(m => m.Comment).OrderByDescending(m => m.CreateTime).ToList();
            IList<Comment> comments = this._bookInfo_CommentService.Filter(m =>
                m.BookInfoId == id
                && !m.IsDeleted
            ).Select(m => m.Comment).OrderByDescending(m => m.CreateTime).ToList();
            ViewBag.Comments = comments;

            //IList<CourseBox> recomCourseBoxes = Container.Instance.Resolve<CourseBoxService>().GetAll().OrderByDescending(m => m.LikeNum).Take(10).ToList();
            IList<BookInfo> recomCourseBoxes = this._bookInfoService.All().OrderByDescending(m => m.LikeNum).Take(10).ToList();
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

                //Container.Instance.Resolve<CourseBox_CommentService>().Create(new CourseBox_Comment
                //{
                //    Comment = new Comment
                //    {
                //        Author = author,
                //        CreateTime = DateTime.Now,
                //        LastUpdateTime = DateTime.Now,
                //        Content = content
                //    },
                //    CourseBox = new CourseBox { ID = courseBoxId }
                //});
                Comment comment = new Comment
                {
                    Author = author,
                    CreateTime = DateTime.Now,
                    LastUpdateTime = DateTime.Now,
                    Content = content
                };
                this._commentService.Create(comment);
                // TODO: 不知道这样是否可行: 提交多表，待测试
                this._bookInfo_CommentService.Create(new BookInfo_Comment
                {
                    CommentId = comment.ID,
                    BookInfoId = courseBoxId
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