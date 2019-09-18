using Common;
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
    //[EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/CourseBox")]
    public class CourseBoxController : ApiController
    {
        #region Get: 获取指定ID的课程信息
        public ResponseData Get(int id)
        {
            ResponseData responseData = null;
            CourseBoxViewModel viewModel = null;

            try
            {
                // 未登录用户返回基本课程数据
                CourseBox courseBox = Container.Instance.Resolve<CourseBoxService>().GetEntity(id);
                string creatorAvatar = Container.Instance.Resolve<SettingService>().Query(new List<ICriterion>
                {
                    Expression.Eq("SetKey", "WebApiSite")
                }).FirstOrDefault().SetValue + courseBox.Creator.Avatar;
                viewModel = new CourseBoxViewModel
                {
                    ID = courseBox.ID,
                    Name = courseBox.Name,
                    Desc = courseBox.Description,
                    CreateTime = courseBox.CreateTime.ToTimeStamp13(),
                    StartTime = courseBox.StartTime.ToTimeStamp13(),
                    EndTime = courseBox.EndTime.ToTimeStamp13(),
                    IsOpen = courseBox.IsOpen,
                    LastUpdateTime = courseBox.LastUpdateTime.ToTimeStamp13(),
                    LearnDay = courseBox.LearnDay,
                    PicUrl = courseBox.PicUrl,
                    Creator = new Models.UserInfoVM.UserInfoViewModel
                    {
                        ID = courseBox.Creator.ID,
                        UserName = courseBox.Creator.UserName,
                        Name = courseBox.Creator.Name,
                        Avatar = creatorAvatar
                    },
                    Stat = new CourseBoxViewModel.StatViewModel
                    {
                        CommentNum = courseBox.CommentNum,
                        LikeNum = courseBox.LikeNum,
                        DislikeNum = courseBox.DislikeNum,
                        FavNum = courseBox.FavoriteList?.Count ?? 0,
                        ShareNum = courseBox.ShareNum,
                        ViewNum = 21
                    },
                    CourseInfos = new List<CourseBoxViewModel.CourseInfoViewModel>()
                };
                IList<CourseInfo> courseInfos = courseBox.CourseInfoList.OrderBy(m => m.Page).ToList();
                foreach (var item in courseInfos)
                {
                    viewModel.CourseInfos.Add(new CourseBoxViewModel.CourseInfoViewModel
                    {
                        ID = item.ID,
                        Title = item.Title,
                        Page = item.Page,
                        Content = item.Content
                    });
                }

                // 登录用户增加返回学习记录数据
                if (User.Identity != null && User.Identity is UserIdentity)
                {
                    Learner_CourseBox learner_CourseBox = Container.Instance.Resolve<Learner_CourseBoxService>().Query(new List<ICriterion>
                    {
                        Expression.And(
                         Expression.Eq("Learner.ID", ((UserIdentity)User.Identity).ID),
                         Expression.Eq("CourseBox.ID", id)
                         )
                    }).FirstOrDefault();
                    if (learner_CourseBox != null)
                    {
                        Learner_CourseInfo learner_LastAccessCourseInfo = Container.Instance.Resolve<Learner_CourseInfoService>().GetEntity(learner_CourseBox.LasAccesstCourseInfo.ID);
                        viewModel.JoinTime = learner_CourseBox.JoinTime.ToTimeStamp13();
                        viewModel.LastAccessCourseInfo = new CourseBoxViewModel.CourseInfoViewModel
                        {
                            ID = learner_CourseBox.LasAccesstCourseInfo.ID,
                            Page = learner_CourseBox.LasAccesstCourseInfo.Page,
                            Title = learner_CourseBox.LasAccesstCourseInfo.Title,
                            LastPlayAt = learner_LastAccessCourseInfo.LastPlayAt,
                            ProgressAt = learner_LastAccessCourseInfo.ProgressAt
                        };
                        viewModel.SpendTime = learner_CourseBox.SpendTime;

                        for (int i = 0; i < viewModel.CourseInfos.Count; i++)
                        {
                            int courseInfoId = viewModel.CourseInfos[i].ID;
                            Learner_CourseInfo learner_CourseInfo = Container.Instance.Resolve<Learner_CourseInfoService>().Query(new List<ICriterion>
                            {
                                Expression.And(
                                    Expression.Eq("Learner.ID", ((UserIdentity)User.Identity).ID ),
                                    Expression.Eq("CourseInfo.ID", courseInfoId)
                                )
                            }).FirstOrDefault();
                            viewModel.CourseInfos[i].LastPlayAt = learner_CourseInfo.LastPlayAt;
                            viewModel.CourseInfos[i].ProgressAt = learner_CourseInfo.ProgressAt;
                        }
                    }
                }

                responseData = new ResponseData
                {
                    Code = 1,
                    Message = "成功",
                    Data = viewModel
                };
            }
            catch (Exception ex)
            {
                responseData = new ResponseData
                {
                    Code = -1,
                    Message = "获取课程信息失败",
                    Data = viewModel
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
                    Description = model.Desc,
                    Creator = AccountManager.GetCurrentUserInfo(),
                    StartTime = model.StartTime.ToDateTime13(),
                    EndTime = model.EndTime.ToDateTime13(),
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
                    courseBox.Description = model.Desc;
                    courseBox.StartTime = model.StartTime.ToDateTime13();
                    courseBox.EndTime = model.EndTime.ToDateTime13();
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

        #region 课程内所有课件详细历史记录
        [NeedAuth]
        public ResponseData HistoryDetails(int courseBoxId)
        {
            ResponseData responseData = null;
            try
            {

            }
            catch (Exception ex)
            {
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
                Learner_CourseBoxService courseBoxTableService = Container.Instance.Resolve<Learner_CourseBoxService>();
                IList<Learner_CourseBox> iLearnCourseBoxTableList = courseBoxTableService.Query(new List<ICriterion>
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
                        Desc = item.Description,
                        CreateTime = item.CreateTime.ToTimeStamp13(),
                        StartTime = item.StartTime.ToTimeStamp13(),
                        EndTime = item.EndTime.ToTimeStamp13(),
                        IsOpen = item.IsOpen,
                        LastUpdateTime = item.LastUpdateTime.ToTimeStamp13(),
                        LearnDay = item.LearnDay,
                        PicUrl = item.PicUrl,
                        Creator = new Models.UserInfoVM.UserInfoViewModel
                        {
                            ID = item.Creator.ID,
                            UserName = item.Creator.UserName,
                            Name = item.Creator.Name,
                            Avatar = item.Creator.Avatar
                        }
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
                        Desc = item.Description,
                        CreateTime = item.CreateTime.ToTimeStamp13(),
                        StartTime = item.StartTime.ToTimeStamp13(),
                        EndTime = item.EndTime.ToTimeStamp13(),
                        IsOpen = item.IsOpen,
                        LastUpdateTime = item.LastUpdateTime.ToTimeStamp13(),
                        LearnDay = item.LearnDay,
                        PicUrl = item.PicUrl,
                        Creator = new Models.UserInfoVM.UserInfoViewModel
                        {
                            ID = item.Creator.ID,
                            UserName = item.Creator.UserName,
                            Name = item.Creator.Name,
                            Avatar = item.Creator.Avatar
                        }
                    });
                }
            }

            return viewModel;
        }
        #endregion

        #region 此课程盒的评论列表
        [HttpGet]
        [Route("CommentList")]
        public ResponseData CommentList(int id, int pageNum = 1, int pageSize = 20, int orderType = 1)
        {
            ResponseData responseData = null;
            try
            {
                CommentListLoadViewModel viewModel = new CommentListLoadViewModel()
                {
                    Page = new CommentListLoadViewModel.PageViewModel
                    {
                        PageNum = pageNum,
                        PageSize = pageSize
                    },
                    Comments = new List<CommentViewModel>()
                };
                CourseBox_CommentService courseBox_CommentService = Container.Instance.Resolve<CourseBox_CommentService>();

                // 当前课程的所有评论
                IList<Comment> comments = courseBox_CommentService.Query(new List<ICriterion>
                {
                    Expression.Eq("CourseBox.ID", id)
                }).Select(m => m.Comment).ToList();

                // 当前课程的一级评论
                IList<Comment> firstLevelComments = comments.Where(m => m.Parent == null || m.Parent.ID == 0).ToList();

                // 一级评论分页
                if (orderType == 1)
                {
                    // 按热度(赞数-踩数) 从大到小 排序
                    firstLevelComments = firstLevelComments.OrderByDescending(m => (m.LikeNum - m.DislikeNum)).Skip(pageNum * pageSize).Take(pageSize).ToList();
                }
                else if (orderType == 2)
                {
                    // 按时间-最新 排序
                    firstLevelComments = firstLevelComments.OrderByDescending(m => m.CreateTime.ToTimeStamp10()).Skip(pageNum * pageSize).Take(pageSize).ToList();
                }

                // Comments
                viewModel.Comments = CommentDBModelToViewModel(firstLevelComments);

                responseData = new ResponseData
                {
                    Code = 1,
                    Message = "success",
                    Data = viewModel
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
                //CourseBoxCommentService courseBoxCommentService = Container.Instance.Resolve<CourseBoxCommentService>();
                UserInfo author = new UserInfo
                {
                    ID = ((UserIdentity)User.Identity).ID
                };
                //courseBoxCommentService.Create(new CourseBoxComment
                //{
                //    Content = content,
                //    CreateTime = DateTime.Now,
                //    LastUpdateTime = DateTime.Now,
                //    CourseBox = new CourseBox
                //    {
                //        ID = id
                //    },
                //    Author = author
                //});

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

        #region 是我创建的课程?
        [HttpGet]
        [Route("IsICreate")]
        public bool IsICreate(int id)
        {
            return IsICreateCourseBox(id);
        }
        #endregion

        #region 是我学习的课程?
        [HttpGet]
        [Route("IsILearn")]
        public bool IsILearn(int id)
        {
            return IsILearnCourseBox(id);
        }
        #endregion

        #region Helpers

        #region 是我创建的课程?
        public static bool IsICreateCourseBox(int courseBoxId)
        {
            bool isICreate = false;
            CourseBoxService courseBoxService = Container.Instance.Resolve<CourseBoxService>();
            IList<CourseBox> iCreateCourseBoxList = courseBoxService.Query(new List<ICriterion>
            {
                Expression.Eq("Creator.ID", AccountManager.GetCurrentUserInfo().ID)
            }).OrderByDescending(m => m.CreateTime).ToList();
            if (iCreateCourseBoxList.Select(m => m.ID).Contains(courseBoxId))
            {
                isICreate = true;
            }

            return isICreate;
        }
        #endregion

        #region 是我学习的课程？
        public static bool IsILearnCourseBox(int courseBoxId)
        {
            bool isILearn = false;
            Learner_CourseBoxService courseBoxTableService = Container.Instance.Resolve<Learner_CourseBoxService>();
            IList<Learner_CourseBox> iLearnCourseBoxTableList = courseBoxTableService.Query(new List<ICriterion>
            {
                Expression.Eq("Reader.ID", AccountManager.GetCurrentUserInfo().ID)
            }).OrderByDescending(m => m.JoinTime).ToList();
            IList<CourseBox> iLearnCourseBoxList = iLearnCourseBoxTableList.Select(m => m.CourseBox).ToList();
            if (iLearnCourseBoxList.Select(m => m.ID).Contains(courseBoxId))
            {
                isILearn = true;
            }

            return isILearn;
        }
        #endregion

        #region 递归加载评论回复楼中楼 (域模型->视图模型)
        private IList<CommentViewModel> CommentDBModelToViewModel(IList<Comment> firstLevelComments)
        {
            IList<CommentViewModel> rtn = null;
            foreach (var item in firstLevelComments)
            {
                CommentViewModel commentViewModel = new CommentViewModel();
                commentViewModel.ID = item.ID;
                commentViewModel.Content = item.Content;
                commentViewModel.CreateTime = item.CreateTime.ToTimeStamp10();
                commentViewModel.Author = new Models.UserInfoVM.UserInfoViewModel
                {
                    ID = item.Author.ID,
                    UserName = item.Author.UserName,
                    Name = item.Author.Name,
                    Avatar = item.Author.Avatar
                };
                commentViewModel.ParentId = item.Parent?.ID ?? 0;
                //有哪些评论回复了此条评论
                if (item.Children != null && item.Children.Count >= 1)
                {
                    LoadCommentToViewModel(commentViewModel, item.Children);
                }

            }

            return rtn;
        }

        private void LoadCommentToViewModel(CommentViewModel viewModel, IList<Comment> children)
        {
            viewModel.Children = new List<CommentViewModel>();
            foreach (var item in children)
            {
                CommentViewModel vmItem = new CommentViewModel();
                vmItem.ID = item.ID;
                vmItem.Content = item.Content;
                vmItem.CreateTime = item.CreateTime.ToTimeStamp10();
                vmItem.Author = new Models.UserInfoVM.UserInfoViewModel
                {
                    ID = item.Author.ID,
                    UserName = item.Author.UserName,
                    Name = item.Author.Name,
                    Avatar = item.Author.Avatar
                };
                vmItem.ParentId = item.Parent?.ID ?? 0;
                if (item.Children != null && item.Children.Count >= 1)
                {
                    LoadCommentToViewModel(vmItem, item.Children);
                }

                viewModel.Children.Add(vmItem);
            }
        }
        #endregion

        #endregion
    }
}
