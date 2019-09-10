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
using WebApi.Attributes;
using WebApi.Infrastructure;
using WebApi.Models;
using WebApi.Models.Common;
using WebApi.Models.CourseBoxVM;

namespace WebApi.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/CourseBox")]
    public class CourseBoxController : ApiController
    {
        #region Get: 获取指定ID的课程信息
        public ResponseData Get(int id)
        {
            ResponseData responseData = null;
            CourseBoxService courseBoxService = Container.Instance.Resolve<CourseBoxService>();
            if (courseBoxService.Exist(id))
            {
                CourseBox dbModel = courseBoxService.GetEntity(id);
                CourseBoxViewModel viewModel = new CourseBoxViewModel()
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
                    CreatorUserName = dbModel.Creator.UserName
                };

                responseData = new ResponseData
                {
                    Code = 1,
                    Message = "成功",
                    Data = viewModel
                };
            }
            else
            {
                responseData = new ResponseData
                {
                    Code = -1,
                    Message = "不存在此课程",
                };
            }

            return responseData;
        }
        #endregion

        #region Post: 创建新课程
        [NeedAuth]
        public ResponseData Post(CourseBoxViewModel model)
        {
            CourseBoxService courseBoxService = Container.Instance.Resolve<CourseBoxService>();
            ResponseData responseData = null;
            try
            {
                courseBoxService.Create(new CourseBox
                {
                    Name = model.Name,
                    Description = model.Description,
                    Creator = ApiAccountManager.GetCurrentUserInfo(),
                    StartTime = DateTime.Parse(model.StartTime),
                    EndTime = DateTime.Parse(model.EndTime),
                    LearnDay = model.LearnDay,
                    CreateTime = DateTime.Now,
                    LastUpdateTime = DateTime.Now,
                    IsOpen = model.IsOpen,
                    PicUrl = model.PicUrl
                });

                responseData = new ResponseData
                {
                    Code = 1,
                    Message = "创建课程成功"
                };
            }
            catch (Exception ex)
            {
                responseData = new ResponseData
                {
                    Code = -1,
                    Message = "创建课程失败" +
                    ""
                };
            }

            return responseData;
        }
        #endregion

        #region Put: 更新课程基本信息
        [NeedAuth]
        public ResponseData Put(int id, [FromBody]CourseBoxViewModel model)
        {
            ResponseData responseData = null;
            try
            {
                CourseBoxService courseBoxService = Container.Instance.Resolve<CourseBoxService>();
                if (courseBoxService.Exist(id))
                {
                    CourseBox courseBox = courseBoxService.GetEntity(id);
                    courseBox.Name = model.Name;
                    courseBox.Description = model.Description;
                    courseBox.StartTime = DateTime.Parse(model.StartTime);
                    courseBox.EndTime = DateTime.Parse(model.EndTime);
                    courseBox.LearnDay = model.LearnDay;
                    courseBox.LastUpdateTime = DateTime.Now;
                    courseBox.IsOpen = model.IsOpen;
                    if (string.IsNullOrEmpty(model.PicUrl))
                    {
                        courseBox.PicUrl = model.PicUrl;
                    }
                    courseBoxService.Edit(courseBox);

                    responseData = new ResponseData
                    {
                        Code = 1,
                        Message = "课程基本信息更新成功"
                    };
                }
                else
                {
                    responseData = new ResponseData
                    {
                        Code = -1,
                        Message = "该课程不存在"
                    };
                }
            }
            catch (Exception)
            {
                responseData = new ResponseData
                {
                    Code = -1,
                    Message = "课程基本信息更新失败"
                };
            }

            return responseData;
        }
        #endregion

        #region 我学习的课程列表
        [HttpGet]
        [NeedAuth]
        [Route("ILearnCourseBoxList")]
        public IList<CourseBoxViewModel> ILearnCourseBoxList()
        {
            IList<CourseBoxViewModel> viewModel = new List<CourseBoxViewModel>();

            if (User.Identity != null)
            {
                CourseBoxTableService courseBoxTableService = Container.Instance.Resolve<CourseBoxTableService>();
                IList<CourseBoxTable> iLearnCourseBoxTableList = courseBoxTableService.Query(new List<ICriterion>
                {
                    Expression.Eq("Reader.ID", ((UserIdentity)User.Identity).ID)
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
                        CreatorUserName = item.Creator.Name
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

            if (User.Identity != null)
            {
                CourseBoxService courseBoxService = Container.Instance.Resolve<CourseBoxService>();
                IList<CourseBox> iCreateCourseBoxList = courseBoxService.Query(new List<ICriterion>
                {
                    Expression.Eq("Creator.ID", ((UserIdentity)User.Identity).ID)
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
                        CreatorUserName = item.Creator.Name
                    });
                }
            }

            return viewModel;
        }
        #endregion

        #region 此课程盒的评论列表
        [HttpGet]
        [Route("CommentList")]
        public ResponseData CommentList(int id)
        {
            ResponseData responseData = null;
            try
            {
                IList<CommentViewModel> viewList = new List<CommentViewModel>();
                CourseBoxCommentService courseBoxCommentService = Container.Instance.Resolve<CourseBoxCommentService>();

                IList<CourseBoxComment> dbList = courseBoxCommentService.Query(new List<ICriterion>
                {
                    Expression.Eq("CourseBox.ID", id)
                }).ToList();
                foreach (var item in dbList)
                {
                    viewList.Add(new CommentViewModel
                    {
                        ID = item.ID,
                        Content = item.Content,
                        CreateTime = item.CreateTime.ToString("yyyy-MM-dd HH:mm"),
                        Author = new Models.UserInfoVM.UserInfoViewModel
                        {
                            ID = item.Author.ID,
                            UserName = item.Author.UserName,
                            Name = item.Author.Name,
                            Avatar = item.Author.Avatar
                        }
                    });
                }

                responseData = new ResponseData
                {
                    Code = 1,
                    Message = "success",
                    Data = viewList
                };
            }
            catch (Exception ex)
            {
                responseData = new ResponseData
                {
                    Code = -1,
                    Message = "failure"
                };
            }

            return responseData;
        }
        #endregion

        #region 评论课程
        [HttpPost]
        [NeedAuth]
        [Route("Comment")]
        public ResponseData Comment(int id, [FromBody]string content)
        {
            ResponseData responseData = null;
            try
            {
                CourseBoxCommentService courseBoxCommentService = Container.Instance.Resolve<CourseBoxCommentService>();
                UserInfo author = new UserInfo
                {
                    ID = ((UserIdentity)User.Identity).ID
                };
                courseBoxCommentService.Create(new CourseBoxComment
                {
                    Content = content,
                    CreateTime = DateTime.Now,
                    LastUpdateTime = DateTime.Now,
                    CourseBox = new CourseBox
                    {
                        ID = id
                    },
                    Author = author
                });

                responseData = new ResponseData
                {
                    Code = 1,
                    Message = "评论成功"
                };
            }
            catch (Exception ex)
            {
                responseData = new ResponseData
                {
                    Code = -1,
                    Message = "评论失败"
                };
            }

            return responseData;
        }
        #endregion
    }
}
