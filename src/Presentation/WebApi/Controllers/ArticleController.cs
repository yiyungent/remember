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
using WebApi.Models.ArticleVM;
using Services.Interface;
using Domain.Entities;
using Framework.Extensions;
using System.Web;

namespace WebApi.Controllers
{
    //[EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/BookInfo")]
    public class ArticleController : BaseApiController
    {
        #region Fields
        private readonly IArticleService _articleService;
        private readonly IFollower_FollowedService _follower_FollowedService;
        //private readonly IUser_BookInfoService _user_BookInfoService;
        //private readonly IUser_BookSectionService _user_BookSectionService;
        private readonly IArticle_CommentService _article_CommentService;
        private readonly ICommentService _commentService;
        private readonly IComment_LikeService _comment_LikeService;
        private readonly IComment_DislikeService _comment_DislikeService;
        //private readonly IBookSectionService _bookSectionService;
        #endregion

        #region Ctor
        public ArticleController(IArticleService articleService,
            IFollower_FollowedService follower_FollowedService,
            //IUser_BookInfoService user_BookInfoService,
            //IUser_BookSectionService user_BookSectionService,
            IArticle_CommentService article_CommentService,
            ICommentService commentService,
            IComment_LikeService comment_LikeService,
            IComment_DislikeService comment_DislikeService
            /*IBookSectionService bookSectionService*/)
        {
            this._articleService = articleService;
            this._follower_FollowedService = follower_FollowedService;
            //this._user_BookInfoService = user_BookInfoService;
            //this._user_BookSectionService = user_BookSectionService;
            this._article_CommentService = article_CommentService;
            this._commentService = commentService;
            this._comment_LikeService = comment_LikeService;
            this._comment_DislikeService = comment_DislikeService;
            //this._bookSectionService = bookSectionService;
        }
        #endregion

        #region Methods

        #region Get: 获取指定ID的文章信息
        public ResponseData Get(int id)
        {
            ResponseData responseData = null;
            ArticleViewModel viewModel = null;

            try
            {
                // 未登录用户返回基本文章数据
                #region 未登录用户返回 基本文章数据
                // TODO: 未处理文章不存在情况
                Article article = this._articleService.Find(id);
                int creatorFansNum = this._follower_FollowedService.Count(m => m.FollowedId == article.AuthorId && !m.IsDeleted);
                //int learnViewNum = this._user_BookInfoService.Count(m => m.BookInfoId == article.ID);

                viewModel = new ArticleViewModel
                {
                    ID = article.ID,
                    Title = article.Title,
                    Desc = article.Description,
                    CreateTime = article.CreateTime.ToTimeStamp13(),
                    IsOpen = article.OpenStatus == Article.OStatus.All,
                    LastUpdateTime = article.LastUpdateTime.ToTimeStamp13(),
                    PicUrl = article.PicUrl.ToHttpAbsoluteUrl(),
                    Author = new ArticleViewModel.AuthorViewModel
                    {
                        ID = article.AuthorId,
                        UserName = article.Author?.UserName ?? "",
                        Avatar = article.Author?.Avatar.ToHttpAbsoluteUrl(),
                        FansNum = creatorFansNum
                    },
                    Stat = new ArticleViewModel.StatViewModel
                    {
                        CommentNum = article.CommentNum,
                        LikeNum = article.LikeNum,
                        DislikeNum = article.DislikeNum,
                        FavNum = article.Favorite_Articles?.Count ?? 0,
                        ShareNum = article.ShareNum,
                        //ViewNum = learnViewNum
                    },
                    // NOTE: 未登录用户 为 未对此文章加入学习，加入学习时间为 0，前端可通过判断这个知道是否加入学习此文章
                    //JoinTime = 0,
                    //VideoInfos = new List<ArticleViewModel.VideoInfoViewModel>()
                };
                //IList<BookSection> videoInfos = article.BookSections.OrderBy(m => m.SortCode).Where(m => !m.IsDeleted).ToList();
                //int pageNum = 1;
                //foreach (var item in videoInfos)
                //{
                //    viewModel.VideoInfos.Add(new BookInfoViewModel.VideoInfoViewModel
                //    {
                //        ID = item.ID,
                //        Title = item.Title,
                //        Page = pageNum,
                //        PlayUrl = item.Content.ToHttpAbsoluteUrl()
                //    });
                //    pageNum++;
                //}
                #endregion

                #region 登录用户 附加 播放历史
                //        int currentUserId = AccountManager.GetCurrentAccount().UserId;
                //        if (currentUserId != 0)
                //        {
                //            // 登录用户 附加 播放历史
                //            User_BookInfo learner_CourseBox = this._user_BookInfoService.Find(m => m.ReaderId == currentUserId && m.BookInfoId == id && !m.IsDeleted);
                //            if (learner_CourseBox != null)
                //            {
                //                User_BookSection learner_LastPlay_VideoInfo = this._user_BookSectionService.Find(m => m.ReaderId == currentUserId && m.BookSectionId == learner_CourseBox.LastViewSectionId && !m.IsDeleted);
                //                if (learner_LastPlay_VideoInfo != null)
                //                {
                //                    viewModel.LastPlayVideoInfo = new BookInfoViewModel.VideoInfoViewModel
                //                    {
                //                        ID = learner_LastPlay_VideoInfo.ID,
                //                        Page = learner_LastPlay_VideoInfo.BookSection.SortCode,
                //                        PlayUrl = learner_LastPlay_VideoInfo.BookSection.Content.ToHttpAbsoluteUrl(),
                //                        Title = learner_LastPlay_VideoInfo.BookSection.Title,

                //                        ProgressAt = learner_LastPlay_VideoInfo.ProgressAt,
                //                        LastPlayAt = learner_LastPlay_VideoInfo.LastViewAt
                //                    };
                //                }
                //                viewModel.JoinTime = learner_CourseBox.CreateTime.ToTimeStamp13();
                //            }
                //        }
                //        //#endregion

                //        responseData = new ResponseData
                //        {
                //            Code = 1,
                //            Message = "获取文章信息成功",
                //            Data = viewModel
                //        };
                //    }
                //    catch (Exception ex)
                //    {
                //        responseData = new ResponseData
                //        {
                //            Code = -1,
                //            Message = "获取文章信息失败",
                //            Data = viewModel
                //        };
                //    }
                #endregion
            }
            catch { }

            return responseData;
        }



        #region Post: 创建新文章
        [NeedAuth]
        public ResponseData Post(ArticleViewModel model)
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
                this._articleService.Create(new Article
                {
                    Title = model.Title,
                    Description = model.Desc,
                    Author = new UserInfo { ID = ((UserIdentity)User).ID },
                    CreateTime = DateTime.Now,
                    LastUpdateTime = DateTime.Now,
                    OpenStatus = model.IsOpen ? Article.OStatus.All : Article.OStatus.Self,
                    PicUrl = model.PicUrl
                });

                responseData = new ResponseData
                {
                    Code = 1,
                    Message = "创建文章成功"
                };
            }
            catch (Exception ex)
            {
                responseData = new ResponseData
                {
                    Code = -1,
                    Message = "创建文章失败" +
                    ""
                };
            }

            return responseData;
        }
        #endregion

        #region Put: 更新文章基本信息
        [NeedAuth]
        public ResponseData Put(int id, [FromBody] ArticleViewModel model)
        {
            ResponseData responseData = null;
            try
            {
                //CourseBoxService courseBoxService = Container.Instance.Resolve<CourseBoxService>();
                if (this._articleService.Contains(m => m.ID == id && !m.IsDeleted))
                {
                    Article article = this._articleService.Find(id);
                    article.Title = model.Title;
                    article.Description = model.Desc;
                    article.LastUpdateTime = DateTime.Now;
                    article.OpenStatus = model.IsOpen ? Article.OStatus.All : Article.OStatus.Self;
                    if (string.IsNullOrEmpty(model.PicUrl))
                    {
                        article.PicUrl = model.PicUrl;
                    }
                    this._articleService.Update(article);

                    responseData = new ResponseData
                    {
                        Code = 1,
                        Message = "文章基本信息更新成功"
                    };
                }
                else
                {
                    responseData = new ResponseData
                    {
                        Code = -1,
                        Message = "该文章不存在"
                    };
                }
            }
            catch (Exception)
            {
                responseData = new ResponseData
                {
                    Code = -1,
                    Message = "文章基本信息更新失败"
                };
            }

            return responseData;
        }
        #endregion


        #region 我创建的文章列表
        [HttpGet]
        [Route(nameof(ICreateArticleList))]
        public ResponseData ICreateArticleList()
        {
            ResponseData responseData = null;
            IList<ArticleViewModel> viewModel = new List<ArticleViewModel>();

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
                IList<Article> iCreateCourseBoxList = this._articleService.Filter(m => m.AuthorId == currentUserId).OrderByDescending(m => m.CreateTime).ToList();

                foreach (var item in iCreateCourseBoxList)
                {
                    viewModel.Add(new ArticleViewModel
                    {
                        ID = item.ID,
                        Title = item.Title,
                        Desc = item.Description,
                        CreateTime = item.CreateTime.ToTimeStamp13(),
                        IsOpen = item.OpenStatus == Article.OStatus.All,
                        LastUpdateTime = item.LastUpdateTime.ToTimeStamp13(),
                        PicUrl = item.PicUrl,
                        Author = new ArticleViewModel.AuthorViewModel
                        {
                            ID = item.AuthorId,
                            UserName = item.Author.UserName,
                            Avatar = item.Author.Avatar
                        }
                    });
                }

                responseData = new ResponseData
                {
                    Code = 1,
                    Message = "成功获取 我创建的文章列表",
                    Data = viewModel
                };
            }

            return responseData;
        }
        #endregion

        #region 此文章盒的评论列表
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

                // 当前文章的所有评论
                IList<Comment> comments = this._article_CommentService.Filter(m => m.ArticleId == id && !m.IsDeleted).Select(m => m.Comment).ToList();

                // 当前文章的一级评论
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
                viewModel.ArticleId = courseBoxId;

                // TODO: 未做文章是否存在等有效性效验

                //IList<Comment> comments = Container.Instance.Resolve<CourseBox_CommentService>().Query(new List<ICriterion>
                //{
                //    Expression.Eq("CourseBox.ID", courseBoxId)
                //}).Select(m => m.Comment).OrderByDescending(m => m.CreateTime).ToList();
                IList<Comment> comments = this._article_CommentService.Filter(
                    m => m.ArticleId == courseBoxId
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

        #region 评论文章
        [HttpPost]
        [NeedAuth]
        [Route("Comment")]
        public ResponseData Comment(CommentInputModel inputModel)
        {
            ResponseData responseData = null;
            try
            {
                if (this._articleService.Contains(m => m.ID == inputModel.ArticleId))
                {
                    // 评论文章  
                    // 1. CourseBox.CommentNum + 1  当前文章 评论数 + 1 
                    Article article = this._articleService.Find(inputModel.ArticleId);
                    article.CommentNum = article.CommentNum + 1;

                    this._articleService.Update(article);
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

                    this._article_CommentService.Create(new Article_Comment
                    {
                        CommentId = comment.ID,
                        ArticleId = inputModel.ArticleId,
                    });

                    responseData = new ResponseData
                    {
                        Code = 1,
                        Message = "评论成功"
                    };
                }
                else
                {
                    // 文章不存在
                    responseData = new ResponseData
                    {
                        Code = -2,
                        Message = "评论的文章不存在"
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

        #region 是我创建的文章?
        [HttpGet]
        [Route("IsICreate")]
        public bool IsICreate(int id)
        {
            return IsICreateCourseBox(id);
        }
        #endregion

        #region Helpers

        #region 是我创建的文章?
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
            IArticleService bookInfoService = Core.ContainerManager.Resolve<IArticleService>();
            int currentUserId = AccountManager.GetCurrentUserInfo().ID;
            IList<Article> iCreateCourseBoxList = bookInfoService.Filter(m => m.AuthorId == currentUserId && !m.IsDeleted).OrderByDescending(m => m.CreateTime).ToList();
            if (iCreateCourseBoxList.Select(m => m.ID).Contains(courseBoxId))
            {
                isICreate = true;
            }

            return isICreate;
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

        #endregion
    }
}
