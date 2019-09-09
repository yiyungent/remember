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
using WebUI.Areas.Admin.Models.CourseBoxVM;
using WebUI.Extensions;
using WebUI.Models.CourseBoxVM;

namespace WebUI.Controllers
{
    public class CourseBoxController : Controller
    {
        #region Fields
        private CourseInfoService _courseInfoService;

        private CourseBoxService _courseBoxService;

        private CourseBoxTableService _courseBoxTableService;
        #endregion

        #region Ctor
        public CourseBoxController()
        {
            this._courseInfoService = Container.Instance.Resolve<CourseInfoService>();
            this._courseBoxService = Container.Instance.Resolve<CourseBoxService>();
            this._courseBoxTableService = Container.Instance.Resolve<CourseBoxTableService>();
        }
        #endregion

        #region 课程盒首页
        [HttpGet]
        /// <summary>
        /// 课程盒首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            UserInfo currentUser = AccountManager.GetCurrentUserInfo();
            IList<CourseBoxTable> iLearnCourseBoxTableList = _courseBoxTableService.Query(new List<ICriterion>
            {
                Expression.Eq("Reader.ID", currentUser.ID)
            }).OrderByDescending(m => m.JoinTime).ToList();
            ViewBag.ILearnCourseBoxTableList = iLearnCourseBoxTableList;

            IList<CourseBox> iCreateCourseBoxList = _courseBoxService.Query(new List<ICriterion>
            {
                Expression.Eq("Creator.ID", currentUser.ID)
            }).OrderByDescending(m => m.CreateTime).ToList();
            ViewBag.ICreateCourseBoxList = iCreateCourseBoxList;

            return View();
        }
        #endregion

        #region 课程盒描述页
        [HttpGet]
        /// <summary>
        /// 卡片盒描述页
        /// </summary>
        /// <param name="id">卡片盒Id</param>
        /// <returns></returns>
        public ViewResult Details(int id)
        {
            CourseBox viewModel = null;
            if (_courseBoxService.Exist(id))
            {
                viewModel = _courseBoxService.GetEntity(id);
            }
            UserInfo currentUser = AccountManager.GetCurrentUserInfo();
            ViewBag.User = currentUser;

            CourseBoxTable courseBoxTable = null;
            if (viewModel != null)
            {
                IList<CourseBoxTable> courseBoxTables = _courseBoxTableService.Query(new List<ICriterion>
                {
                    Expression.Eq("Reader.ID", currentUser.ID),
                    Expression.Eq("CourseBox.ID", viewModel.ID)
                }).ToList();
                if (courseBoxTables != null && courseBoxTables.Count >= 1)
                {
                    courseBoxTable = courseBoxTables[0];
                }
            }
            ViewBag.CourseBoxTable = courseBoxTable;

            return View(viewModel);
        }
        #endregion

        #region 课程盒的目录页
        public ViewResult Catalog(int id)
        {
            return View();
        }
        #endregion

        #region 评论编辑页
        public ActionResult Comment()
        {
            return View();
        }
        #endregion

        #region 搜索课程盒
        [HttpGet]
        public ActionResult Search()
        {
            string q = Request.QueryString["q"]?.Trim();
            ViewBag.Q = q;

            IList<ICriterion> queryConditions = new List<ICriterion>();
            ViewBag.SearchCourseBoxList = new List<CourseBox>();

            if (!string.IsNullOrEmpty(q?.Trim()))
            {
                queryConditions.Add(
                    Expression.Or(Expression.Like("Name", q.Trim(), MatchMode.Anywhere), Expression.Like("Description", q.Trim(), MatchMode.Anywhere))
                );

                ViewBag.SearchCourseBoxList = _courseBoxService.Query(queryConditions);
            }

            return View();
        }

        [HttpPost]
        public JsonResult Search(string q = null)
        {
            IList<string> searchSuggestionList = new List<string>();
            IList<CourseBox> courseBoxes = new List<CourseBox>();

            IList<ICriterion> queryConditions = new List<ICriterion>();
            if (!string.IsNullOrEmpty(q?.Trim()))
            {
                // 搜索建议- Name
                queryConditions.Add(Expression.Like("Name", q.Trim(), MatchMode.Anywhere));
                IList<CourseBox> courseBoxesByName = _courseBoxService.Query(queryConditions).ToList();
                foreach (var item in courseBoxesByName)
                {
                    if (!courseBoxes.Contains(item, new CourseBoxCompare()))
                    {
                        courseBoxes.Add(item);
                        searchSuggestionList.Add(item.Name);
                    }
                }
                // 搜索建议 - Description
                queryConditions.Clear();
                queryConditions.Add(Expression.Like("Description", q.Trim(), MatchMode.Anywhere));
                IList<CourseBox> courseBoxesByDesc = _courseBoxService.Query(queryConditions).ToList();
                foreach (var item in courseBoxesByDesc)
                {
                    if (!courseBoxes.Contains(item, new CourseBoxCompare()))
                    {
                        courseBoxes.Add(item);
                        searchSuggestionList.Add(item.Description);
                    }
                }
            }

            return Json(searchSuggestionList, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 创建课程盒
        [HttpGet]
        public ViewResult Create()
        {
            CourseBoxViewModel viewModel = new CourseBoxViewModel();

            return View(viewModel);
        }

        [HttpPost]
        public JsonResult Create(CourseBoxViewModel inputModel)
        {
            try
            {
                // 数据格式效验
                if (ModelState.IsValid)
                {
                    #region 数据有效效验

                    #endregion

                    UserInfo creator = AccountManager.GetCurrentUserInfo();
                    CourseBox dbModel = (CourseBox)inputModel;
                    dbModel.Creator = creator;
                    dbModel.CreateTime = DateTime.UtcNow;
                    dbModel.LastUpdateTime = DateTime.UtcNow;

                    #region 失败
                    //CourseInfo parentCourseInfo = new CourseInfo
                    //{
                    //    Title = "分类1",
                    //    CourseBox = dbModel
                    //};
                    //_CourseInfoService.Create(parentCourseInfo);
                    //CourseInfo CourseInfo = new CourseInfo
                    //{
                    //    Title = "示例1",
                    //    Content = "示例内容，可尝试编辑本页面",
                    //    Parent = parentCourseInfo,
                    //    CourseBox = dbModel
                    //};
                    //dbModel.CourseInfoList.Add(parentCourseInfo);
                    //dbModel.CourseInfoList.Add(CourseInfo); 
                    #endregion

                    _courseBoxService.Create(dbModel);

                    CourseBox createdCourseBox = _courseBoxService.Query(new List<ICriterion>
                    {
                        Expression.Eq("Creator.ID", creator.ID)
                    }).OrderByDescending(m => m.ID).Take(1).ToList()[0];

                    #region 添加示例内容
                    //CourseInfo parentCourseInfo = new CourseInfo
                    //{
                    //    Title = "分类1",
                    //    CourseBox = createdCourseBox
                    //};
                    //_courseInfoService.Create(parentCourseInfo);
                    CourseInfo CourseInfo = new CourseInfo
                    {
                        Title = "示例1",
                        Content = "示例内容，可尝试编辑本页面",
                        Parent = null,
                        CourseBox = createdCourseBox
                    };
                    _courseInfoService.Create(CourseInfo);
                    #endregion



                    return Json(new { code = 1, message = "添加成功", createdCourseBoxId = createdCourseBox.ID });
                }
                else
                {
                    string errorMessage = ModelState.GetErrorMessage();
                    return Json(new { code = -1, message = errorMessage });
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = -2, message = "添加失败" });
            }
        }
        #endregion

        #region 举报
        public ViewResult Report()
        {
            return View();
        }
        #endregion

        #region 增加课程内容-视频
        public ActionResult AddVideo(int id)
        {
            return View();
        }
        #endregion

        #region 增加课程内容-富文本贴
        public ViewResult AddRichText(int id)
        {
            return View();
        }
        #endregion

        #region 增加课程内容-试卷题
        public ViewResult AddQuestion(int id)
        {
            return View();
        }
        #endregion

        #region 增加课程内容-附件
        public ViewResult AddAttachment(int id)
        {
            return View();
        }
        #endregion

        public class CourseBoxCompare : IEqualityComparer<CourseBox>
        {
            public bool Equals(CourseBox x, CourseBox y)
            {
                if (x == null || y == null)
                {
                    return false;
                }
                if (x.ID == y.ID)
                {
                    return true;
                }

                return false;
            }

            public int GetHashCode(CourseBox obj)
            {
                throw new NotImplementedException();
            }
        }
    }
}