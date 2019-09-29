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
using WebApi.DomainExt;

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
                int creatorFansNum = Container.Instance.Resolve<Follower_FollowedService>().Count(Expression.Eq("Followed.ID", courseBox.Creator.ID));
                int learnViewNum = Container.Instance.Resolve<Learner_CourseBoxService>().Count(Expression.Eq("CourseBox.ID", courseBox.ID));

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
                    PicUrl = courseBox.PicUrl.ToHttpAbsoluteUrl(),
                    Creator = new CourseBoxViewModel.CreatorViewModel
                    {
                        ID = courseBox.Creator.ID,
                        UserName = courseBox.Creator.UserName,
                        Avatar = courseBox.Creator.Avatar.ToHttpAbsoluteUrl(),
                        FansNum = creatorFansNum
                    },
                    Stat = new CourseBoxViewModel.StatViewModel
                    {
                        CommentNum = courseBox.CommentNum,
                        LikeNum = courseBox.LikeNum,
                        DislikeNum = courseBox.DislikeNum,
                        FavNum = courseBox.FavoriteList?.Count ?? 0,
                        ShareNum = courseBox.ShareNum,
                        ViewNum = learnViewNum
                    },
                    // NOTE: 未登录用户 为 未对此课程加入学习，加入学习时间为 0，前端可通过判断这个知道是否加入学习此课程
                    JoinTime = 0,
                    VideoInfos = new List<CourseBoxViewModel.VideoInfoViewModel>()
                };
                IList<VideoInfo> courseInfos = courseBox.VideoInfos.OrderBy(m => m.Page).ToList();
                foreach (var item in courseInfos)
                {
                    viewModel.VideoInfos.Add(new CourseBoxViewModel.VideoInfoViewModel
                    {
                        ID = item.ID,
                        Title = item.Title,
                        Page = item.Page,
                        PlayUrl = item.PlayUrl.ToHttpAbsoluteUrl()
                    });
                }

                // 登录用户增加返回学习记录数据
                UserInfo user = AccountManager.GetCurrentUserInfo();
                if (user != null)
                {
                    Learner_CourseBox learner_CourseBox = Container.Instance.Resolve<Learner_CourseBoxService>().Query(new List<ICriterion>
                    {
                        Expression.And(
                         Expression.Eq("Learner.ID", user.ID),
                         Expression.Eq("CourseBox.ID", id)
                         )
                    }).FirstOrDefault();
                    // 此用户有学习此课程
                    if (learner_CourseBox != null)
                    {
                        Learner_VideoInfo learner_LastAccessVideoInfo = null;
                        if (learner_CourseBox.LastPlayVideoInfo != null)
                        {
                            learner_LastAccessVideoInfo = Container.Instance.Resolve<Learner_VideoInfoService>().GetEntity(learner_CourseBox.LastPlayVideoInfo.ID);
                            viewModel.LastPlayVideoInfo = new CourseBoxViewModel.VideoInfoViewModel
                            {
                                ID = learner_CourseBox.LastPlayVideoInfo.ID,
                                Page = learner_CourseBox.LastPlayVideoInfo.Page,
                                Title = learner_CourseBox.LastPlayVideoInfo.Title,
                                LastPlayAt = learner_LastAccessVideoInfo.LastPlayAt,
                                ProgressAt = learner_LastAccessVideoInfo.ProgressAt
                            };
                        }

                        viewModel.JoinTime = learner_CourseBox.JoinTime.ToTimeStamp13();
                        viewModel.SpendTime = learner_CourseBox.SpendTime;

                        for (int i = 0; i < viewModel.VideoInfos.Count; i++)
                        {
                            int videoInfoId = viewModel.VideoInfos[i].ID;
                            Learner_VideoInfo learner_VideoInfo = Container.Instance.Resolve<Learner_VideoInfoService>().Query(new List<ICriterion>
                            {
                                Expression.And(
                                    Expression.Eq("Learner.ID", user.ID ),
                                    Expression.Eq("VideoInfo.ID", videoInfoId)
                                )
                            }).FirstOrDefault();
                            viewModel.VideoInfos[i].LastPlayAt = learner_VideoInfo?.LastPlayAt ?? 0;
                            viewModel.VideoInfos[i].ProgressAt = learner_VideoInfo?.ProgressAt ?? 0;
                        }
                    }
                }

                responseData = new ResponseData
                {
                    Code = 1,
                    Message = "获取课程信息成功",
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
            ResponseData responseData = null;
            try
            {
                Container.Instance.Resolve<CourseBoxService>().Create(new CourseBox
                {
                    Name = model.Name,
                    Description = model.Desc,
                    Creator = new UserInfo { ID = ((UserIdentity)User).ID },
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

        #region 此课程内-所有视频课件-详细历史记录
        [NeedAuth]
        public ResponseData History(int courseBoxId)
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


        #region 加入学习/取消学习此课程
        /// <summary>
        /// 如果我已经加入学习此课程，则取消对此课程的学习，否则加入学习
        /// </summary>
        /// <param name="courseBoxId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("LearnCourseBox")]
        [NeedAuth]
        public ResponseData LearnCourseBox(LearnCourseBoxInputModel inputModel)
        {
            ResponseData responseData = null;
            int courseBoxId = inputModel.CourseBoxId;
            try
            {
                bool isExist = Container.Instance.Resolve<CourseBoxService>().Exist(courseBoxId);
                if (isExist)
                {
                    bool isLearned = Container.Instance.Resolve<Learner_CourseBoxService>().Count(
                        Expression.And
                        (
                            Expression.Eq("Status", Domain.Base.StatusEnum.Normal),
                            Expression.And(
                                Expression.Eq("Learner.ID", ((UserIdentity)User.Identity).ID),
                                Expression.Eq("CourseBox.ID", courseBoxId)
                            )
                        )) >= 1;
                    if (isLearned)
                    {
                        try
                        {
                            // 取消学习
                            IList<Learner_CourseBox> learner_CourseBoxes = Container.Instance.Resolve<Learner_CourseBoxService>().Query(new List<ICriterion>
                        {
                            Expression.And(
                                Expression.Eq("CourseBox.ID", courseBoxId),
                                Expression.Eq("Learner.ID", ((UserIdentity)User.Identity).ID)
                            )
                        });
                            // 移除 此学习者与此课程的相关所有记录
                            foreach (var item in learner_CourseBoxes)
                            {
                                Container.Instance.Resolve<Learner_CourseBoxService>().Delete(item.ID);
                            }
                            // TODO: 此学习者与此课程，与此课程的视频课件的相关记录 Learner_VideoInfo

                            responseData = new ResponseData
                            {
                                Code = 1,
                                Message = "取消学习此课程成功"
                            };
                        }
                        catch (Exception ex)
                        {
                            responseData = new ResponseData
                            {
                                Code = -1,
                                Message = "取消学习此课程失败"
                            };
                        }
                    }
                    else
                    {
                        try
                        {
                            // 加入学习
                            DateTime jointTime = DateTime.Now;
                            Container.Instance.Resolve<Learner_CourseBoxService>().Create(new Learner_CourseBox
                            {
                                JoinTime = jointTime,
                                CourseBox = Container.Instance.Resolve<CourseBoxService>().GetEntity(courseBoxId),
                                Learner = new UserInfo { ID = ((UserIdentity)User.Identity).ID },
                            });

                            responseData = new ResponseData
                            {
                                Code = 1,
                                Message = "加入学习此课程成功",
                                Data = new
                                {
                                    joinTime = jointTime.ToTimeStamp13()
                                }
                            };
                        }
                        catch (Exception ex)
                        {
                            responseData = new ResponseData
                            {
                                Code = -1,
                                Message = "加入学习此课程失败"
                            };
                        }
                    }
                }
                else
                {
                    responseData = new ResponseData
                    {
                        Code = -3,
                        Message = "不存在此课程"
                    };
                }
            }
            catch (Exception ex)
            {
                responseData = new ResponseData
                {
                    Code = -2,
                    Message = "失败"
                };
            }

            return responseData;
        }
        #endregion


        #region 我学习的课程列表
        [HttpGet]
        [NeedAuth]
        [Route("ILearnCourseBoxList")]
        public ResponseData ILearnCourseBoxList()
        {
            ResponseData responseData = null;
            IList<CourseBoxViewModel> viewModel = new List<CourseBoxViewModel>();

            if (User.Identity != null)
            {
                IList<Learner_CourseBox> iLearnCourseBoxTableList = Container.Instance.Resolve<Learner_CourseBoxService>().Query(new List<ICriterion>
                {
                    Expression.Eq("Learner.ID", ((UserIdentity)User.Identity).ID)
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
                        Creator = new CourseBoxViewModel.CreatorViewModel
                        {
                            ID = item.Creator.ID,
                            UserName = item.Creator.UserName,
                            Avatar = item.Creator.Avatar
                        }
                    });
                }

                responseData = new ResponseData
                {
                    Code = 1,
                    Message = "成功获取 我学习的课程列表",
                    Data = viewModel
                };
            }

            return responseData;
        }
        #endregion

        #region 我创建的课程列表
        [HttpGet]
        [Route("ICreateCourseBoxList")]
        public ResponseData ICreateCourseBoxList()
        {
            ResponseData responseData = null;
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
                        Creator = new CourseBoxViewModel.CreatorViewModel
                        {
                            ID = item.Creator.ID,
                            UserName = item.Creator.UserName,
                            Avatar = item.Creator.Avatar
                        }
                    });
                }

                responseData = new ResponseData
                {
                    Code = 1,
                    Message = "成功获取 我创建的课程列表",
                    Data = viewModel
                };
            }

            return responseData;
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
                if (Container.Instance.Resolve<CourseBoxService>().Exist(id))
                {
                    // 评论课程  
                    // 1. CourseBox.CommentNum + 1  当前课程 评论数 + 1 
                    CourseBox courseBox = Container.Instance.Resolve<CourseBoxService>().GetEntity(id);
                    courseBox.CommentNum = courseBox.CommentNum + 1;

                    Container.Instance.Resolve<CourseBoxService>().Edit(courseBox);

                    // 2. CourseBox_Comment 插入一条记录
                    UserInfo userInfo = new UserInfo
                    {
                        ID = ((UserIdentity)User.Identity).ID
                    };
                    Container.Instance.Resolve<CourseBox_CommentService>().Create(new CourseBox_Comment
                    {
                        Comment = new Comment
                        {
                            Author = userInfo,
                            Content = content,
                            CreateTime = DateTime.Now,
                            LastUpdateTime = DateTime.Now
                        },
                        CourseBox = new CourseBox { ID = id }
                    });
                }
                else
                {
                    // 课程不存在
                    responseData = new ResponseData
                    {
                        Code = -2,
                        Message = "评论的课程不存在"
                    };
                }
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
                Expression.Eq("Learner.ID", AccountManager.GetCurrentUserInfo().ID)
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
                commentViewModel.Author = new CommentViewModel.AuthorViewModel
                {
                    ID = item.Author.ID,
                    UserName = item.Author.UserName,
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
                vmItem.Author = new CommentViewModel.AuthorViewModel
                {
                    ID = item.Author.ID,
                    UserName = item.Author.UserName,
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
