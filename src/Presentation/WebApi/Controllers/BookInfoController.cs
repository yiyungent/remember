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
using WebApi.Models.BookInfoVM;
using Services.Interface;
using Domain.Entities;
using Framework.Extensions;
using System.Web;

namespace WebApi.Controllers
{
    //[EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/BookInfo")]
    public class BookInfoController : BaseApiController
    {
        #region Fields
        private readonly IBookInfoService _bookInfoService;
        private readonly IFollower_FollowedService _follower_FollowedService;
        private readonly IUser_BookInfoService _user_BookInfoService;
        private readonly IUser_BookSectionService _user_BookSectionService;
        private readonly IBookInfo_CommentService _bookInfo_CommentService;
        private readonly ICommentService _commentService;
        private readonly IComment_LikeService _comment_LikeService;
        private readonly IComment_DislikeService _comment_DislikeService;
        private readonly IBookSectionService _bookSectionService;
        #endregion

        #region Ctor
        public BookInfoController(IBookInfoService bookInfoService,
            IFollower_FollowedService follower_FollowedService,
            IUser_BookInfoService user_BookInfoService,
            IUser_BookSectionService user_BookSectionService,
            IBookInfo_CommentService bookInfo_CommentService,
            ICommentService commentService,
            IComment_LikeService comment_LikeService,
            IComment_DislikeService comment_DislikeService,
            IBookSectionService bookSectionService)
        {
            this._bookInfoService = bookInfoService;
            this._follower_FollowedService = follower_FollowedService;
            this._user_BookInfoService = user_BookInfoService;
            this._user_BookSectionService = user_BookSectionService;
            this._bookInfo_CommentService = bookInfo_CommentService;
            this._commentService = commentService;
            this._comment_LikeService = comment_LikeService;
            this._comment_DislikeService = comment_DislikeService;
            this._bookSectionService = bookSectionService;
        }
        #endregion

        #region Methods

        #region Get: 获取指定ID的课程信息
        public ResponseData Get(int id)
        {
            ResponseData responseData = null;
            BookInfoViewModel viewModel = null;

            try
            {
                // 未登录用户返回基本课程数据
                #region 未登录用户返回 基本课程数据
                // TODO: 未处理课程不存在情况
                BookInfo courseBox = this._bookInfoService.Find(id);
                int creatorFansNum = this._follower_FollowedService.Count(m => m.FollowedId == courseBox.CreatorId && !m.IsDeleted);
                int learnViewNum = this._user_BookInfoService.Count(m => m.BookInfoId == courseBox.ID);

                viewModel = new BookInfoViewModel
                {
                    ID = courseBox.ID,
                    Name = courseBox.Name,
                    Desc = courseBox.Description,
                    CreateTime = courseBox.CreateTime.ToTimeStamp13(),
                    IsOpen = courseBox.IsOpen,
                    LastUpdateTime = courseBox.LastUpdateTime.ToTimeStamp13(),
                    PicUrl = courseBox.PicUrl.ToHttpAbsoluteUrl(),
                    Creator = new BookInfoViewModel.CreatorViewModel
                    {
                        ID = courseBox.CreatorId,
                        UserName = courseBox.Creator?.UserName ?? "",
                        Avatar = courseBox.Creator?.Avatar.ToHttpAbsoluteUrl(),
                        FansNum = creatorFansNum
                    },
                    Stat = new BookInfoViewModel.StatViewModel
                    {
                        CommentNum = courseBox.CommentNum,
                        LikeNum = courseBox.LikeNum,
                        DislikeNum = courseBox.DislikeNum,
                        FavNum = courseBox.Favorite_BookInfos?.Count ?? 0,
                        ShareNum = courseBox.ShareNum,
                        ViewNum = learnViewNum
                    },
                    // NOTE: 未登录用户 为 未对此课程加入学习，加入学习时间为 0，前端可通过判断这个知道是否加入学习此课程
                    JoinTime = 0,
                    VideoInfos = new List<BookInfoViewModel.VideoInfoViewModel>()
                };
                IList<BookSection> videoInfos = courseBox.BookSections.OrderBy(m => m.SortCode).Where(m => !m.IsDeleted).ToList();
                int pageNum = 1;
                foreach (var item in videoInfos)
                {
                    viewModel.VideoInfos.Add(new BookInfoViewModel.VideoInfoViewModel
                    {
                        ID = item.ID,
                        Title = item.Title,
                        Page = pageNum,
                        PlayUrl = item.Content.ToHttpAbsoluteUrl()
                    });
                    pageNum++;
                }
                #endregion

                #region 登录用户 附加 播放历史
                int currentUserId = AccountManager.GetCurrentAccount().UserId;
                if (currentUserId != 0)
                {
                    // 登录用户 附加 播放历史
                    User_BookInfo learner_CourseBox = this._user_BookInfoService.Find(m => m.ReaderId == currentUserId && m.BookInfoId == id && !m.IsDeleted);
                    if (learner_CourseBox != null)
                    {
                        User_BookSection learner_LastPlay_VideoInfo = this._user_BookSectionService.Find(m => m.ReaderId == currentUserId && m.BookSectionId == learner_CourseBox.LastViewSectionId && !m.IsDeleted);
                        if (learner_LastPlay_VideoInfo != null)
                        {
                            viewModel.LastPlayVideoInfo = new BookInfoViewModel.VideoInfoViewModel
                            {
                                ID = learner_LastPlay_VideoInfo.ID,
                                Page = learner_LastPlay_VideoInfo.BookSection.SortCode,
                                PlayUrl = learner_LastPlay_VideoInfo.BookSection.Content.ToHttpAbsoluteUrl(),
                                Title = learner_LastPlay_VideoInfo.BookSection.Title,

                                ProgressAt = learner_LastPlay_VideoInfo.ProgressAt,
                                LastPlayAt = learner_LastPlay_VideoInfo.LastViewAt
                            };
                        }
                        viewModel.JoinTime = learner_CourseBox.CreateTime.ToTimeStamp13();
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
        public ResponseData Post(BookInfoViewModel model)
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
                this._bookInfoService.Create(new BookInfo
                {
                    Name = model.Name,
                    Description = model.Desc,
                    Creator = new UserInfo { ID = ((UserIdentity)User).ID },
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
        public ResponseData Put(int id, [FromBody]BookInfoViewModel model)
        {
            ResponseData responseData = null;
            try
            {
                //CourseBoxService courseBoxService = Container.Instance.Resolve<CourseBoxService>();
                if (this._bookInfoService.Contains(m => m.ID == id && !m.IsDeleted))
                {
                    BookInfo courseBox = this._bookInfoService.Find(id);
                    courseBox.Name = model.Name;
                    courseBox.Description = model.Desc;
                    courseBox.LastUpdateTime = DateTime.Now;
                    courseBox.IsOpen = model.IsOpen;
                    if (string.IsNullOrEmpty(model.PicUrl))
                    {
                        courseBox.PicUrl = model.PicUrl;
                    }
                    this._bookInfoService.Update(courseBox);

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
        public ResponseData LearnCourseBox(ReadBookInfoInputModel inputModel)
        {
            ResponseData responseData = null;
            int courseBoxId = inputModel.BookInfoId;
            try
            {
                //bool isExist = Container.Instance.Resolve<CourseBoxService>().Exist(courseBoxId);
                bool isExist = this._bookInfoService.Contains(m => m.ID == courseBoxId && !m.IsDeleted);
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
                    bool isLearned = this._user_BookInfoService.Contains(m => m.ReaderId == currentUserId && m.BookInfoId == courseBoxId && !m.IsDeleted);
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
                            IList<User_BookInfo> learner_CourseBoxes = this._user_BookInfoService.Filter(m => m.BookInfoId == courseBoxId && m.ReaderId == currentUserId && !m.IsDeleted).ToList();
                            // 移除 此学习者与此课程的相关所有记录
                            foreach (var item in learner_CourseBoxes)
                            {
                                //Container.Instance.Resolve<Learner_CourseBoxService>().Delete(item.ID);
                                item.IsDeleted = true;
                                item.DeletedAt = DateTime.Now;
                                this._user_BookInfoService.Update(item);
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
                            this._user_BookInfoService.Create(new User_BookInfo
                            {
                                CreateTime = joinTime,
                                BookInfo = this._bookInfoService.Find(courseBoxId),
                                Reader = new UserInfo { ID = currentUserId },
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
            IList<BookInfoViewModel> viewModel = new List<BookInfoViewModel>();

            if (User.Identity != null)
            {
                #region 废弃
                //IList<Learner_CourseBox> iLearnCourseBoxTableList = Container.Instance.Resolve<Learner_CourseBoxService>().Query(new List<ICriterion>
                //{
                //    Expression.Eq("Learner.ID", ((UserIdentity)User.Identity).ID)
                //}).OrderByDescending(m => m.JoinTime).ToList(); /**/
                #endregion

                int currentUserId = ((UserIdentity)User.Identity).ID;
                IList<User_BookInfo> iLearnCourseBoxTableList = this._user_BookInfoService.Filter(m => m.ReaderId == currentUserId && !m.IsDeleted).OrderByDescending(m => m.CreateTime).ToList();

                IList<BookInfo> iLearnCourseBoxList = iLearnCourseBoxTableList.Select(m => m.BookInfo).ToList();

                foreach (var item in iLearnCourseBoxList)
                {
                    viewModel.Add(new BookInfoViewModel
                    {
                        ID = item.ID,
                        Name = item.Name,
                        Desc = item.Description,
                        CreateTime = item.CreateTime.ToTimeStamp13(),
                        IsOpen = item.IsOpen,
                        LastUpdateTime = item.LastUpdateTime.ToTimeStamp13(),
                        PicUrl = item.PicUrl,
                        Creator = new BookInfoViewModel.CreatorViewModel
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
            IList<BookInfoViewModel> viewModel = new List<BookInfoViewModel>();

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
                IList<BookInfo> iCreateCourseBoxList = this._bookInfoService.Filter(m => m.CreatorId == currentUserId).OrderByDescending(m => m.CreateTime).ToList();

                foreach (var item in iCreateCourseBoxList)
                {
                    viewModel.Add(new BookInfoViewModel
                    {
                        ID = item.ID,
                        Name = item.Name,
                        Desc = item.Description,
                        CreateTime = item.CreateTime.ToTimeStamp13(),
                        IsOpen = item.IsOpen,
                        LastUpdateTime = item.LastUpdateTime.ToTimeStamp13(),
                        PicUrl = item.PicUrl,
                        Creator = new BookInfoViewModel.CreatorViewModel
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
                IList<Comment> comments = this._bookInfo_CommentService.Filter(m => m.BookInfoId == id && !m.IsDeleted).Select(m => m.Comment).ToList();

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
                IList<Comment> comments = this._bookInfo_CommentService.Filter(
                    m => m.BookInfoId == courseBoxId
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
                        LikeNum = item.LikeNum
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
                if (this._bookInfoService.Contains(m => m.ID == inputModel.CourseBoxId))
                {
                    // 评论课程  
                    // 1. CourseBox.CommentNum + 1  当前课程 评论数 + 1 
                    BookInfo courseBox = this._bookInfoService.Find(inputModel.CourseBoxId);
                    courseBox.CommentNum = courseBox.CommentNum + 1;

                    this._bookInfoService.Update(courseBox);
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

                    this._bookInfo_CommentService.Create(new BookInfo_Comment
                    {
                        CommentId = comment.ID,
                        BookInfoId = inputModel.CourseBoxId,
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
        public ResponseData PlayHistoryPush(ViewHistoryPushInputModel inputModel)
        {
            ResponseData responseData = null;
            try
            {
                int currentUserId = ((UserIdentity)User.Identity).ID;

                // 更新此用户与此门课程的最新播放视频
                // 1.先查出此视频所在课程
                //VideoInfo videoInfo = Container.Instance.Resolve<VideoInfoService>().GetEntity(inputModel.ID);
                BookSection videoInfo = this._bookSectionService.Find(inputModel.ID);
                // 2.更新此用户与此门课程的关系，最新播放视频为此视频
                //Learner_CourseBox learner_CourseBox = Container.Instance.Resolve<Learner_CourseBoxService>().Query(new List<ICriterion>
                //{
                //    Expression.And(
                //        Expression.Eq("CourseBox.ID", videoInfo.CourseBox.ID),
                //        Expression.Eq("Learner.ID", currentUserId)
                //    )
                //}).FirstOrDefault();
                User_BookInfo learner_CourseBox = this._user_BookInfoService.Find(m =>
                    m.BookInfoId == videoInfo.BookInfo.ID
                    && m.ReaderId == currentUserId
                    && !m.IsDeleted
                );
                if (learner_CourseBox == null)
                {
                    // 没有此课程的学习/播放记录
                    // 只要一播放就自动算作将此课程加入学习
                    learner_CourseBox = new User_BookInfo();
                    learner_CourseBox.LastViewSection = videoInfo;

                    learner_CourseBox.ReaderId = currentUserId;
                    learner_CourseBox.BookInfo = videoInfo.BookInfo;
                    learner_CourseBox.CreateTime = DateTime.Now;
                    //Container.Instance.Resolve<Learner_CourseBoxService>().Create(learner_CourseBox);
                    this._user_BookInfoService.Create(learner_CourseBox);
                }
                else
                {
                    // 已有此课程的学习/播放记录
                    learner_CourseBox.LastViewSection = videoInfo;
                    //Container.Instance.Resolve<Learner_CourseBoxService>().Edit(learner_CourseBox);
                    this._user_BookInfoService.Update(learner_CourseBox);
                }

                // 更新此用户与此视频最新播放位置等
                //Learner_VideoInfo learner_VideoInfo = Container.Instance.Resolve<Learner_VideoInfoService>().Query(new List<ICriterion>
                //{
                //    Expression.And(
                //        Expression.Eq("VideoInfo.ID", videoInfo.ID),
                //        Expression.Eq("Learner.ID", currentUserId)
                //    )
                //}).FirstOrDefault();
                User_BookSection learner_VideoInfo = this._user_BookSectionService.Find(m =>
                    m.BookSectionId == videoInfo.ID
                    && m.ReaderId == currentUserId
                    && !m.IsDeleted
                );
                if (learner_VideoInfo == null)
                {
                    // 没有此视频的播放记录
                    learner_VideoInfo = new User_BookSection();
                    learner_VideoInfo.LastAccessIp = HttpContext.Current.Request.UserHostName;
                    learner_VideoInfo.LastViewAt = (long)inputModel.LastViewAt;
                    learner_VideoInfo.ProgressAt = (long)inputModel.LastViewAt;
                    learner_VideoInfo.LastViewTime = DateTime.Now;

                    learner_VideoInfo.ReaderId = currentUserId;
                    learner_VideoInfo.BookSection = videoInfo;
                    //Container.Instance.Resolve<Learner_VideoInfoService>().Create(learner_VideoInfo);
                    this._user_BookSectionService.Create(learner_VideoInfo);
                }
                else
                {
                    // 已有此视频的播放记录
                    learner_VideoInfo.LastAccessIp = HttpContext.Current.Request.UserHostName;
                    learner_VideoInfo.LastViewAt = (long)inputModel.LastViewAt;
                    learner_VideoInfo.LastViewTime = DateTime.Now;

                    if (learner_VideoInfo.LastViewAt > learner_VideoInfo.ProgressAt)
                    {
                        // 最新播放位置 大于 播放进度，则更新播放进度
                        learner_VideoInfo.ProgressAt = learner_VideoInfo.LastViewAt;
                    }

                    //Container.Instance.Resolve<Learner_VideoInfoService>().Edit(learner_VideoInfo);
                    this._user_BookSectionService.Update(learner_VideoInfo);
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
            IBookInfoService bookInfoService = Core.ContainerManager.Resolve<IBookInfoService>();
            int currentUserId = AccountManager.GetCurrentUserInfo().ID;
            IList<BookInfo> iCreateCourseBoxList = bookInfoService.Filter(m => m.CreatorId == currentUserId && !m.IsDeleted).OrderByDescending(m => m.CreateTime).ToList();
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
            IUser_BookInfoService user_BookInfoService = Core.ContainerManager.Resolve<IUser_BookInfoService>();
            int currentUserId = AccountManager.GetCurrentUserInfo().ID;
            IList<User_BookInfo> learner_CourseBoxes = user_BookInfoService.Filter(m => m.ReaderId == currentUserId && !m.IsDeleted).OrderByDescending(m => m.CreateTime).ToList();
            if (learner_CourseBoxes.Select(m => m.BookInfo).Select(m => m.ID).Contains(courseBoxId))
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
