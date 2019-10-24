using Core.Common;
using Domain;
using Framework.Infrastructure.Concrete;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
//using System.Web.Http.Cors;
using WebApi.Attributes;
using WebApi.Infrastructure;
using WebApi.Models;
using WebApi.Models.Common;
using WebApi.Models.CourseBoxVM;
using Services.Interface;
using Domain.Entities;
using Framework.Extensions;
using System.Web;

namespace WebApi.Controllers
{
    //[EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/CourseBox")]
    public class CourseBoxController : BaseController
    {
        #region Fields
        private readonly ICourseBoxService _courseBoxService;
        private readonly IFollower_FollowedService _follower_FollowedService;
        private readonly ILearner_CourseBoxService _learner_CourseBoxService;
        private readonly ILearner_VideoInfoService _learner_VideoInfoService;
        private readonly ICourseBox_CommentService _courseBox_CommentService;
        private readonly ICommentService _commentService;
        private readonly IComment_LikeService _comment_LikeService;
        private readonly IComment_DislikeService _comment_DislikeService;
        private readonly IVideoInfoService _videoInfoService;
        #endregion

        #region Ctor
        public CourseBoxController(ICourseBoxService courseBoxService,
            IFollower_FollowedService follower_FollowedService,
            ILearner_CourseBoxService learner_CourseBoxService,
            ILearner_VideoInfoService learner_VideoInfoService,
            ICourseBox_CommentService courseBox_CommentService,
            ICommentService commentService,
            IComment_LikeService comment_LikeService,
            IComment_DislikeService comment_DislikeService,
            IVideoInfoService videoInfoService)
        {
            this._courseBoxService = courseBoxService;
            this._follower_FollowedService = follower_FollowedService;
            this._learner_CourseBoxService = learner_CourseBoxService;
            this._learner_VideoInfoService = learner_VideoInfoService;
            this._courseBox_CommentService = courseBox_CommentService;
            this._commentService = commentService;
            this._comment_LikeService = comment_LikeService;
            this._comment_DislikeService = comment_DislikeService;
            this._videoInfoService = videoInfoService;
        }
        #endregion

        #region Methods

        #region Get: 获取指定ID的课程信息
        public ResponseData Get(int id)
        {
            ResponseData responseData = null;
            CourseBoxViewModel viewModel = null;

            try
            {
                // 未登录用户返回基本课程数据
                #region 未登录用户返回 基本课程数据
                // TODO: 未处理课程不存在情况
                CourseBox courseBox = this._courseBoxService.Find(id);
                int creatorFansNum = this._follower_FollowedService.Count(m => m.FollowedId == courseBox.CreatorId && !m.IsDeleted);
                int learnViewNum = this._learner_CourseBoxService.Count(m => m.CourseBoxId == courseBox.ID);

                viewModel = new CourseBoxViewModel
                {
                    ID = courseBox.ID,
                    Name = courseBox.Name,
                    Desc = courseBox.Description,
                    CreateTime = courseBox.CreateTime.ToTimeStamp13(),
                    StartTime = courseBox.StartTime.ToTimeStamp13(),
                    EndTime = courseBox.EndTime.ToTimeStamp13(),
                    IsOpen = courseBox.IsOpen ?? false,
                    LastUpdateTime = courseBox.LastUpdateTime.ToTimeStamp13(),
                    PicUrl = courseBox.PicUrl.ToHttpAbsoluteUrl(),
                    Creator = new CourseBoxViewModel.CreatorViewModel
                    {
                        ID = courseBox.CreatorId ?? 0,
                        UserName = courseBox.Creator?.UserName ?? "",
                        Avatar = courseBox.Creator?.Avatar.ToHttpAbsoluteUrl(),
                        FansNum = creatorFansNum
                    },
                    Stat = new CourseBoxViewModel.StatViewModel
                    {
                        CommentNum = courseBox.CommentNum ?? 0,
                        LikeNum = courseBox.LikeNum ?? 0,
                        DislikeNum = courseBox.DislikeNum ?? 0,
                        FavNum = courseBox.Favorite_CourseBoxes?.Count ?? 0,
                        ShareNum = courseBox.ShareNum ?? 0,
                        ViewNum = learnViewNum
                    },
                    // NOTE: 未登录用户 为 未对此课程加入学习，加入学习时间为 0，前端可通过判断这个知道是否加入学习此课程
                    JoinTime = 0,
                    VideoInfos = new List<CourseBoxViewModel.VideoInfoViewModel>()
                };
                IList<VideoInfo> videoInfos = courseBox.VideoInfos.OrderBy(m => m.Page).Where(m => !m.IsDeleted).ToList();
                int pageNum = 1;
                foreach (var item in videoInfos)
                {
                    viewModel.VideoInfos.Add(new CourseBoxViewModel.VideoInfoViewModel
                    {
                        ID = item.ID,
                        Title = item.Title,
                        Page = pageNum,
                        PlayUrl = item.PlayUrl.ToHttpAbsoluteUrl()
                    });
                    pageNum++;
                }
                #endregion

                #region 登录用户 附加 播放历史
                int currentUserId = AccountManager.GetCurrentAccount().UserId;
                if (currentUserId != 0)
                {
                    // 登录用户 附加 播放历史
                    Learner_CourseBox learner_CourseBox = this._learner_CourseBoxService.Find(m => m.LearnerId == currentUserId && m.CourseBoxId == id && !m.IsDeleted);
                    if (learner_CourseBox != null)
                    {
                        Learner_VideoInfo learner_LastPlay_VideoInfo = this._learner_VideoInfoService.Find(m => m.LearnerId == currentUserId && m.VideoInfoId == learner_CourseBox.LastPlayVideoInfoId && !m.IsDeleted);
                        if (learner_LastPlay_VideoInfo != null)
                        {
                            viewModel.LastPlayVideoInfo = new CourseBoxViewModel.VideoInfoViewModel
                            {
                                ID = learner_LastPlay_VideoInfo.ID,
                                Page = learner_LastPlay_VideoInfo.VideoInfo.Page,
                                PlayUrl = learner_LastPlay_VideoInfo.VideoInfo.PlayUrl.ToHttpAbsoluteUrl(),
                                Title = learner_LastPlay_VideoInfo.VideoInfo.Title,

                                ProgressAt = learner_LastPlay_VideoInfo.ProgressAt,
                                LastPlayAt = learner_LastPlay_VideoInfo.LastPlayAt
                            };
                        }
                        viewModel.JoinTime = learner_CourseBox.JoinTime.ToTimeStamp13();
                    }
                }
                #endregion

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
                //Container.Instance.Resolve<CourseBoxService>().Create(new CourseBox
                //{
                //    Name = model.Name,
                //    Description = model.Desc,
                //    Creator = new UserInfo { ID = ((UserIdentity)User).ID },
                //    StartTime = model.StartTime.ToDateTime13(),
                //    EndTime = model.EndTime.ToDateTime13(),
                //    LearnDay = model.LearnDay,
                //    CreateTime = DateTime.Now,
                //    LastUpdateTime = DateTime.Now,
                //    IsOpen = model.IsOpen,
                //    PicUrl = model.PicUrl
                //});
                this._courseBoxService.Create(new CourseBox
                {
                    Name = model.Name,
                    Description = model.Desc,
                    Creator = new UserInfo { ID = ((UserIdentity)User).ID },
                    StartTime = model.StartTime.ToDateTime13(),
                    EndTime = model.EndTime.ToDateTime13(),
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
                //CourseBoxService courseBoxService = Container.Instance.Resolve<CourseBoxService>();
                if (this._courseBoxService.Contains(m => m.ID == id && !m.IsDeleted))
                {
                    CourseBox courseBox = this._courseBoxService.Find(id);
                    courseBox.Name = model.Name;
                    courseBox.Description = model.Desc;
                    courseBox.StartTime = model.StartTime.ToDateTime13();
                    courseBox.EndTime = model.EndTime.ToDateTime13();
                    courseBox.LastUpdateTime = DateTime.Now;
                    courseBox.IsOpen = model.IsOpen;
                    if (string.IsNullOrEmpty(model.PicUrl))
                    {
                        courseBox.PicUrl = model.PicUrl;
                    }
                    this._courseBoxService.Update(courseBox);

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
                //bool isExist = Container.Instance.Resolve<CourseBoxService>().Exist(courseBoxId);
                bool isExist = this._courseBoxService.Contains(m => m.ID == courseBoxId && !m.IsDeleted);
                if (isExist)
                {
                    //bool isLearned = Container.Instance.Resolve<Learner_CourseBoxService>().Count(
                    //    Expression.And
                    //    (
                    //        Expression.Eq("Status", Domain.Base.StatusEnum.Normal),
                    //        Expression.And(
                    //            Expression.Eq("Learner.ID", ((UserIdentity)User.Identity).ID),
                    //            Expression.Eq("CourseBox.ID", courseBoxId)
                    //        )
                    //    )) >= 1;
                    int currentUserId = ((UserIdentity)User.Identity).ID;
                    bool isLearned = this._learner_CourseBoxService.Contains(m => m.LearnerId == currentUserId && m.CourseBoxId == courseBoxId && !m.IsDeleted);
                    if (isLearned)
                    {
                        try
                        {
                            // 取消学习
                            //IList<Learner_CourseBox> learner_CourseBoxes = Container.Instance.Resolve<Learner_CourseBoxService>().Query(new List<ICriterion>
                            //{
                            //    Expression.And(
                            //        Expression.Eq("CourseBox.ID", courseBoxId),
                            //        Expression.Eq("Learner.ID", ((UserIdentity)User.Identity).ID)
                            //    )
                            //});
                            IList<Learner_CourseBox> learner_CourseBoxes = this._learner_CourseBoxService.Filter(m => m.CourseBoxId == courseBoxId && m.LearnerId == currentUserId && !m.IsDeleted).ToList();
                            // 移除 此学习者与此课程的相关所有记录
                            foreach (var item in learner_CourseBoxes)
                            {
                                //Container.Instance.Resolve<Learner_CourseBoxService>().Delete(item.ID);
                                item.IsDeleted = true;
                                item.DeletedAt = DateTime.Now;
                                this._learner_CourseBoxService.Update(item);
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
                            DateTime joinTime = DateTime.Now;
                            //Container.Instance.Resolve<Learner_CourseBoxService>().Create(new Learner_CourseBox
                            //{
                            //    JoinTime = jointTime,
                            //    CourseBox = Container.Instance.Resolve<CourseBoxService>().GetEntity(courseBoxId),
                            //    Learner = new UserInfo { ID = ((UserIdentity)User.Identity).ID },
                            //});
                            this._learner_CourseBoxService.Create(new Learner_CourseBox
                            {
                                JoinTime = joinTime,
                                CourseBox = this._courseBoxService.Find(courseBoxId),
                                Learner = new UserInfo { ID = currentUserId },
                            });

                            responseData = new ResponseData
                            {
                                Code = 1,
                                Message = "加入学习此课程成功",
                                Data = new
                                {
                                    joinTime = joinTime.ToTimeStamp13()
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
                #region 废弃
                //IList<Learner_CourseBox> iLearnCourseBoxTableList = Container.Instance.Resolve<Learner_CourseBoxService>().Query(new List<ICriterion>
                //{
                //    Expression.Eq("Learner.ID", ((UserIdentity)User.Identity).ID)
                //}).OrderByDescending(m => m.JoinTime).ToList(); /**/
                #endregion

                int currentUserId = ((UserIdentity)User.Identity).ID;
                IList<Learner_CourseBox> iLearnCourseBoxTableList = this._learner_CourseBoxService.Filter(m => m.LearnerId == currentUserId && !m.IsDeleted).OrderByDescending(m => m.JoinTime).ToList();

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
                        IsOpen = item.IsOpen ?? false,
                        LastUpdateTime = item.LastUpdateTime.ToTimeStamp13(),
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
                #region 废弃
                //CourseBoxService courseBoxService = Container.Instance.Resolve<CourseBoxService>();
                //IList<CourseBox> iCreateCourseBoxList = courseBoxService.Query(new List<ICriterion>
                //{
                //    Expression.Eq("Creator.ID", ((UserIdentity)User.Identity).ID)
                //}).OrderByDescending(m => m.CreateTime).ToList(); 
                #endregion
                int currentUserId = ((UserIdentity)User.Identity).ID;
                IList<CourseBox> iCreateCourseBoxList = this._courseBoxService.Filter(m => m.CreatorId == currentUserId).OrderByDescending(m => m.CreateTime).ToList();

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
                        IsOpen = item.IsOpen ?? false,
                        LastUpdateTime = item.LastUpdateTime.ToTimeStamp13(),
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

                // 当前课程的所有评论
                IList<Comment> comments = this._courseBox_CommentService.Filter(m => m.CourseBoxId == id && !m.IsDeleted).Select(m => m.Comment).ToList();

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

        #region 简单按时间倒序排序的评论列表
        [HttpGet]
        [Route("SimpleComments")]
        public ResponseData SimpleComments(int courseBoxId)
        {
            ResponseData responseData = null;
            try
            {
                SimpleCommentsViewModel viewModel = new SimpleCommentsViewModel();
                viewModel.CourseBoxId = courseBoxId;

                // TODO: 未做课程是否存在等有效性效验

                //IList<Comment> comments = Container.Instance.Resolve<CourseBox_CommentService>().Query(new List<ICriterion>
                //{
                //    Expression.Eq("CourseBox.ID", courseBoxId)
                //}).Select(m => m.Comment).OrderByDescending(m => m.CreateTime).ToList();
                IList<Comment> comments = this._courseBox_CommentService.Filter(
                    m => m.CourseBoxId == courseBoxId
                    && !m.IsDeleted
                ).Select(m => m.Comment).OrderByDescending(m => m.CreateTime).ToList();

                foreach (var item in comments)
                {
                    viewModel.Comments.Add(new SimpleCommentsViewModel.CommentModel
                    {
                        ID = item.ID,
                        Author = new SimpleCommentsViewModel.AuthorModel
                        {
                            ID = item.Author.ID,
                            UserName = item.Author.UserName,
                            Avatar = item.Author.Avatar.ToHttpAbsoluteUrl()
                        },
                        Content = item.Content,
                        CreateTime = item.CreateTime.ToTimeStamp13(),
                        LikeNum = item.LikeNum ?? 0
                    });
                }


                responseData = new ResponseData
                {
                    Code = 1,
                    Message = "加载评论成功",
                    Data = viewModel
                };
            }
            catch (Exception ex)
            {
                responseData = new ResponseData
                {
                    Code = -1,
                    Message = "加载评论失败"
                };
            }

            return responseData;
        }
        #endregion

        #region 评论课程
        [HttpPost]
        [NeedAuth]
        [Route("Comment")]
        public ResponseData Comment(CommentInputModel inputModel)
        {
            ResponseData responseData = null;
            try
            {
                if (this._courseBoxService.Contains(m => m.ID == inputModel.CourseBoxId))
                {
                    // 评论课程  
                    // 1. CourseBox.CommentNum + 1  当前课程 评论数 + 1 
                    CourseBox courseBox = this._courseBoxService.Find(inputModel.CourseBoxId);
                    courseBox.CommentNum = courseBox.CommentNum + 1;

                    this._courseBoxService.Update(courseBox);
                    int currentUserId = ((UserIdentity)User.Identity).ID;

                    // 2. CourseBox_Comment 插入一条记录

                    Comment comment = new Comment
                    {
                        AuthorId = currentUserId,
                        Content = inputModel.Content,
                        CreateTime = DateTime.Now,
                        LastUpdateTime = DateTime.Now
                    };
                    this._commentService.Create(comment);

                    this._courseBox_CommentService.Create(new CourseBox_Comment
                    {
                        CommentId = comment.ID,
                        CourseBoxId = inputModel.CourseBoxId,
                    });

                    responseData = new ResponseData
                    {
                        Code = 1,
                        Message = "评论成功"
                    };
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
                    Message = "评论失败:" + ex.Message
                };
            }

            return responseData;
        }
        #endregion

        #region 评论点赞或踩
        [Route("CommentLike")]
        [HttpPost]
        [NeedAuth]
        public ResponseData CommentLike(CommentLikeInputModel inputModel)
        {
            ResponseData responseData = null;
            string msg = "";
            try
            {
                int currentUserId = ((UserIdentity)User.Identity).ID;

                //var comment = Container.Instance.Resolve<CommentService>().GetEntity(inputModel.CommentId);
                var comment = this._commentService.Find(inputModel.CommentId);
                switch (inputModel.DoType)
                {
                    case 1:
                        msg = "点赞";
                        comment.LikeNum = comment.LikeNum + 1;
                        //Container.Instance.Resolve<CommentService>().Edit(comment);
                        this._commentService.Update(comment);

                        //Container.Instance.Resolve<Comment_LikeService>().Create(new Comment_Like
                        //{
                        //    Comment = new Comment { ID = inputModel.CommentId },
                        //    UserInfo = new UserInfo { ID = currentUserId },
                        //    CreateTime = DateTime.Now
                        //});
                        this._comment_LikeService.Create(new Comment_Like
                        {
                            CommentId = inputModel.CommentId,
                            UserInfoId = currentUserId,
                            CreateTime = DateTime.Now
                        });

                        break;
                    case 2:
                        msg = "踩";
                        comment.DislikeNum = comment.DislikeNum + 1;
                        //Container.Instance.Resolve<CommentService>().Edit(comment);
                        this._commentService.Update(comment);

                        //Container.Instance.Resolve<Comment_DislikeService>().Create(new Comment_Dislike
                        //{
                        //    Comment = new Comment { ID = inputModel.CommentId },
                        //    UserInfo = new UserInfo { ID = currentUserId },
                        //    CreateTime = DateTime.Now
                        //});
                        this._comment_DislikeService.Create(new Comment_Dislike
                        {
                            CommentId = inputModel.CommentId,
                            UserInfoId = currentUserId,
                            CreateTime = DateTime.Now
                        });

                        break;
                }

                responseData = new ResponseData
                {
                    Code = 1,
                    Message = msg + "成功"
                };
            }
            catch (Exception ex)
            {
                responseData = new ResponseData
                {
                    Code = -1,
                    Message = msg + "失败: " + ex.Message + ex.InnerException?.Message
                };
            }

            return responseData;
        }
        #endregion

        #region 播放历史推送
        [Route("PlayHistoryPush")]
        [HttpPost]
        [HttpOptions]
        [NeedAuth]
        public ResponseData PlayHistoryPush(PlayHistoryPushInputModel inputModel)
        {
            ResponseData responseData = null;
            try
            {
                int currentUserId = ((UserIdentity)User.Identity).ID;

                // 更新此用户与此门课程的最新播放视频
                // 1.先查出此视频所在课程
                //VideoInfo videoInfo = Container.Instance.Resolve<VideoInfoService>().GetEntity(inputModel.ID);
                VideoInfo videoInfo = this._videoInfoService.Find(inputModel.ID);
                // 2.更新此用户与此门课程的关系，最新播放视频为此视频
                //Learner_CourseBox learner_CourseBox = Container.Instance.Resolve<Learner_CourseBoxService>().Query(new List<ICriterion>
                //{
                //    Expression.And(
                //        Expression.Eq("CourseBox.ID", videoInfo.CourseBox.ID),
                //        Expression.Eq("Learner.ID", currentUserId)
                //    )
                //}).FirstOrDefault();
                Learner_CourseBox learner_CourseBox = this._learner_CourseBoxService.Find(m =>
                    m.CourseBoxId == videoInfo.CourseBox.ID
                    && m.LearnerId == currentUserId
                    && !m.IsDeleted
                );
                if (learner_CourseBox == null)
                {
                    // 没有此课程的学习/播放记录
                    // 只要一播放就自动算作将此课程加入学习
                    learner_CourseBox = new Learner_CourseBox();
                    learner_CourseBox.LastPlayVideoInfo = videoInfo;

                    learner_CourseBox.LearnerId = currentUserId;
                    learner_CourseBox.CourseBox = videoInfo.CourseBox;
                    learner_CourseBox.JoinTime = DateTime.Now;
                    //Container.Instance.Resolve<Learner_CourseBoxService>().Create(learner_CourseBox);
                    this._learner_CourseBoxService.Create(learner_CourseBox);
                }
                else
                {
                    // 已有此课程的学习/播放记录
                    learner_CourseBox.LastPlayVideoInfo = videoInfo;
                    //Container.Instance.Resolve<Learner_CourseBoxService>().Edit(learner_CourseBox);
                    this._learner_CourseBoxService.Update(learner_CourseBox);
                }

                // 更新此用户与此视频最新播放位置等
                //Learner_VideoInfo learner_VideoInfo = Container.Instance.Resolve<Learner_VideoInfoService>().Query(new List<ICriterion>
                //{
                //    Expression.And(
                //        Expression.Eq("VideoInfo.ID", videoInfo.ID),
                //        Expression.Eq("Learner.ID", currentUserId)
                //    )
                //}).FirstOrDefault();
                Learner_VideoInfo learner_VideoInfo = this._learner_VideoInfoService.Find(m =>
                    m.VideoInfoId == videoInfo.ID
                    && m.LearnerId == currentUserId
                    && !m.IsDeleted
                );
                if (learner_VideoInfo == null)
                {
                    // 没有此视频的播放记录
                    learner_VideoInfo = new Learner_VideoInfo();
                    learner_VideoInfo.LastAccessIp = HttpContext.Current.Request.UserHostName;
                    learner_VideoInfo.LastPlayAt = (long)inputModel.LastPlayAt;
                    learner_VideoInfo.LastPlayTime = DateTime.Now;

                    learner_VideoInfo.LearnerId = currentUserId;
                    learner_VideoInfo.VideoInfo = videoInfo;
                    //Container.Instance.Resolve<Learner_VideoInfoService>().Create(learner_VideoInfo);
                    this._learner_VideoInfoService.Create(learner_VideoInfo);
                }
                else
                {
                    // 已有此视频的播放记录
                    learner_VideoInfo.LastAccessIp = HttpContext.Current.Request.UserHostName;
                    learner_VideoInfo.LastPlayAt = (long)inputModel.LastPlayAt;
                    learner_VideoInfo.LastPlayTime = DateTime.Now;
                    //Container.Instance.Resolve<Learner_VideoInfoService>().Edit(learner_VideoInfo);
                    this._learner_VideoInfoService.Update(learner_VideoInfo);
                }

                responseData = new ResponseData
                {
                    Code = 1,
                    Message = "成功",
                    Data = inputModel
                };
            }
            catch (Exception ex)
            {
                responseData = new ResponseData
                {
                    Code = -1,
                    Message = "失败: " + ex.Message + ex.InnerException?.Message,
                    Data = inputModel
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
            #region 废弃
            //CourseBoxService courseBoxService = Container.Instance.Resolve<CourseBoxService>();
            //IList<CourseBox> iCreateCourseBoxList = courseBoxService.Query(new List<ICriterion>
            //{
            //    Expression.Eq("Creator.ID", AccountManager.GetCurrentUserInfo().ID)
            //}).OrderByDescending(m => m.CreateTime).ToList(); 
            #endregion
            ICourseBoxService courseBoxService = Core.ContainerManager.Resolve<ICourseBoxService>();
            int currentUserId = AccountManager.GetCurrentUserInfo().ID;
            IList<CourseBox> iCreateCourseBoxList = courseBoxService.Filter(m => m.CreatorId == currentUserId && !m.IsDeleted).OrderByDescending(m => m.CreateTime).ToList();
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
            #region 废弃
            //Learner_CourseBoxService courseBoxTableService = Container.Instance.Resolve<Learner_CourseBoxService>();
            //IList<Learner_CourseBox> iLearnCourseBoxTableList = courseBoxTableService.Query(new List<ICriterion>
            //{
            //    Expression.Eq("Learner.ID", AccountManager.GetCurrentUserInfo().ID)
            //}).OrderByDescending(m => m.JoinTime).ToList(); 
            #endregion
            ILearner_CourseBoxService learner_CourseBoxService = Core.ContainerManager.Resolve<ILearner_CourseBoxService>();
            int currentUserId = AccountManager.GetCurrentUserInfo().ID;
            IList<Learner_CourseBox> learner_CourseBoxes = learner_CourseBoxService.Filter(m => m.LearnerId == currentUserId && !m.IsDeleted).OrderByDescending(m => m.JoinTime).ToList();
            if (learner_CourseBoxes.Select(m => m.CourseBox).Select(m => m.ID).Contains(courseBoxId))
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
                    LoadCommentToViewModel(commentViewModel, item.Children.ToList());
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
                    LoadCommentToViewModel(vmItem, item.Children.ToList());
                }

                viewModel.Children.Add(vmItem);
            }
        }
        #endregion

        #endregion 

        #endregion
    }
}
