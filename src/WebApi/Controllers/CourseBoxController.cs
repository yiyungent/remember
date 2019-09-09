using Core;
using Domain;
using Framework.Infrastructure.Concrete;
using NHibernate.Criterion;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using WebApi.Infrastructure;
using WebApi.Models;

namespace WebApi.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/CourseBox")]
    public class CourseBoxController : ApiController
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

        public CourseBoxViewModel Get(int id)
        {
            CourseBoxViewModel viewModel = null;
            if (_courseBoxService.Exist(id))
            {
                CourseBox dbModel = _courseBoxService.GetEntity(id);
                viewModel = new CourseBoxViewModel()
                {
                    ID = dbModel.ID,
                    Name = dbModel.Name,
                    Description = dbModel.Description,
                    CreateTime = dbModel.CreateTime.ToString("yyyy-MM-dd"),
                    StartTime = dbModel.StartTime.ToString("yyyy-MM-dd"),
                    EndTime = dbModel.EndTime.ToString("yyyy-MM-dd"),
                    IsOpen = dbModel.IsOpen,
                    LastUpdateTime = dbModel.LastUpdateTime.ToString("yyyy-MM-dd HH:mm"),
                    LearnDay = dbModel.LearnDay,
                    PicUrl = dbModel.PicUrl,
                    CourseBoxCreatorName = dbModel.Creator.Name
                };
            }

            return viewModel;
        }

        public void Post(CourseBoxViewModel data)
        {

        }

        public void Put(int id, [FromBody]CourseBoxViewModel data)
        {

        }

        #region 我学习的课程列表
        [HttpGet]
        [Route("ILearnCourseBoxList")]
        public IList<CourseBoxViewModel> ILearnCourseBoxList()
        {
            IList<CourseBoxViewModel> viewModel = new List<CourseBoxViewModel>();

            UserInfo currentUser = ApiAccountManager.GetCurrentUserInfo();
            if (currentUser != null)
            {
                IList<CourseBoxTable> iLearnCourseBoxTableList = _courseBoxTableService.Query(new List<ICriterion>
                {
                    Expression.Eq("Reader.ID", currentUser.ID)
                }).OrderByDescending(m => m.JoinTime).ToList();

                //IList<CourseBox> iCreateCourseBoxList = _courseBoxService.Query(new List<ICriterion>
                //{
                //    Expression.Eq("Creator.ID", currentUser.ID)
                //}).OrderByDescending(m => m.CreateTime).ToList();

                IList<CourseBox> iLearnCourseBoxList = iLearnCourseBoxTableList.Select(m => m.CourseBox).ToList();

                foreach (var item in iLearnCourseBoxList)
                {
                    viewModel.Add(new CourseBoxViewModel
                    {
                        ID = item.ID,
                        Name = item.Name,
                        Description = item.Description,
                        CreateTime = item.CreateTime.ToString("yyyy-MM-dd"),
                        StartTime = item.StartTime.ToString("yyyy-MM-dd"),
                        EndTime = item.EndTime.ToString("yyyy-MM-dd"),
                        IsOpen = item.IsOpen,
                        LastUpdateTime = item.LastUpdateTime.ToString("yyyy-MM-dd HH:mm"),
                        LearnDay = item.LearnDay,
                        PicUrl = item.PicUrl,
                        CourseBoxCreatorName = item.Creator.Name
                    });
                }
            }

            return viewModel;
        }
        #endregion

        #region 我创建的课程列表
        [HttpGet]
        [Route("ICreateCourseBoxList")]
        public IList<CourseBoxViewModel> ICreateCourseBoxList()
        {
            IList<CourseBoxViewModel> viewModel = new List<CourseBoxViewModel>();

            UserInfo currentUser = ApiAccountManager.GetCurrentUserInfo();
            if (currentUser != null)
            {
                IList<CourseBox> iCreateCourseBoxList = _courseBoxService.Query(new List<ICriterion>
                {
                    Expression.Eq("Creator.ID", currentUser.ID)
                }).OrderByDescending(m => m.CreateTime).ToList();

                foreach (var item in iCreateCourseBoxList)
                {
                    viewModel.Add(new CourseBoxViewModel
                    {
                        ID = item.ID,
                        Name = item.Name,
                        Description = item.Description,
                        CreateTime = item.CreateTime.ToString("yyyy-MM-dd"),
                        StartTime = item.StartTime.ToString("yyyy-MM-dd"),
                        EndTime = item.EndTime.ToString("yyyy-MM-dd"),
                        IsOpen = item.IsOpen,
                        LastUpdateTime = item.LastUpdateTime.ToString("yyyy-MM-dd HH:mm"),
                        LearnDay = item.LearnDay,
                        PicUrl = item.PicUrl,
                        CourseBoxCreatorName = item.Creator.Name
                    });
                }
            }

            return viewModel;
        } 
        #endregion
    }
}
