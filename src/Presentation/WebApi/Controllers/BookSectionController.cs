using Core;
using Core.Common;
using Domain;
using Domain.Entities;
using Framework.Extensions;
using Framework.Infrastructure.Concrete;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
//using System.Web.Http.Cors;
using WebApi.Attributes;
using WebApi.Infrastructure;
using WebApi.Models.Common;
using WebApi.Models.BookSectionVM;

namespace WebApi.Controllers
{
    [RoutePrefix("api/BookSection")]
    public class BookSectionController : BaseApiController
    {
        #region Fields
        private readonly IUser_BookSectionService _user_BookSectionService;
        private readonly IBookSectionService _bookSectionService;
        private readonly IBookInfoService _bookInfoService;
        #endregion

        #region Ctor
        public BookSectionController(IUser_BookSectionService user_BookSectionService, IBookSectionService bookSectionService, IBookInfoService bookInfoService)
        {
            this._user_BookSectionService = user_BookSectionService;
            this._bookSectionService = bookSectionService;
            this._bookInfoService = bookInfoService;
        }
        #endregion

        #region Get: 获取指定ID的视频课件
        /// <summary>
        /// 需登陆 && (属于 我学习的课程 || 属于 我创建的课程)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [NeedAuth]
        public ResponseData Get(int id)
        {
            ResponseData responseData = null;

            int currentUserId = ((UserIdentity)User.Identity).ID;
            //Learner_VideoInfo learner_CourseInfo = Container.Instance.Resolve<Learner_VideoInfoService>().Query(new List<ICriterion>
            //{
            //    Expression.And(
            //        Expression.Eq("Learner.ID", ((UserIdentity)User.Identity).ID),
            //        Expression.Eq("VideoInfo.ID", id)
            //    )
            //}).FirstOrDefault();
            User_BookSection learner_CourseInfo = this._user_BookSectionService.Find(m => m.ReaderId == currentUserId && m.BookSectionId == id && !m.IsDeleted);

            if (learner_CourseInfo != null)
            {
                // 属于我学习的课程
                BookSection courseInfo = learner_CourseInfo.BookSection;
                int courseBoxId = courseInfo.BookInfo.ID;

                // 我学习的课程列表中有此课程 -> 可以访问
                BookSectionViewModel viewModel = new BookSectionViewModel()
                {
                    ID = courseInfo.ID,
                    Title = courseInfo.Title,
                    PlayUrl = courseInfo.Content,
                    BookInfoId = courseBoxId,
                    LastAccessIp = learner_CourseInfo.LastAccessIp,
                    LastAccessTime = learner_CourseInfo.LastViewTime.ToTimeStamp13(),
                    LastViewAt = learner_CourseInfo.LastViewAt,
                    ProgressAt = learner_CourseInfo.ProgressAt
                };

                responseData = new ResponseData
                {
                    Code = 1,
                    Message = "success",
                    Data = viewModel
                };
            }
            else
            {
                // 是否属于我创建的课程
                BookSection courseInfo = this._bookSectionService.Find(id);
                bool isMeCreate = courseInfo.BookInfo.Creator.ID == ((UserIdentity)User.Identity).ID;
                if (isMeCreate)
                {
                    // 未加入学习，但是我创建的课程
                    BookSectionViewModel viewModel = new BookSectionViewModel()
                    {
                        ID = courseInfo.ID,
                        Title = courseInfo.Title,
                        PlayUrl = courseInfo.Content,
                        BookInfoId = courseInfo.BookInfo.ID,
                    };

                    responseData = new ResponseData
                    {
                        Code = 1,
                        Message = "success",
                        Data = viewModel
                    };
                }
                else
                {
                    responseData = new ResponseData
                    {
                        Code = -1,
                        Message = "不存在此课程内容"
                    };
                }
            }

            return responseData;
        }
        #endregion

        #region  推送观看课件历史
        /// <summary>
        /// 推送观看课件历史
        /// </summary>
        /// <param name="id">课件ID</param>
        /// <param name="playPos">播放位置（毫秒）</param>
        [NeedAuth]
        [HttpPost]
        [Route("View")]
        public ResponseData View(int id, long playPos)
        {
            ResponseData responseData = null;

            try
            {
                int currentUserId = ((UserIdentity)User.Identity).ID;
                //Learner_VideoInfo learner_CourseInfo = Container.Instance.Resolve<Learner_VideoInfoService>().Query(new List<ICriterion>
                //{
                //    Expression.And(
                //        Expression.Eq("Learner.ID", ((UserIdentity)User.Identity).ID),
                //        Expression.Eq("VideoInfo.ID", id)
                //    )
                //}).FirstOrDefault();
                User_BookSection learner_CourseInfo = this._user_BookSectionService.Find(m => m.ReaderId == currentUserId && m.BookSectionId == id && !m.IsDeleted);
                if (learner_CourseInfo == null)
                {
                    // 第一次学习此课件
                    this._user_BookSectionService.Create(new User_BookSection
                    {
                        BookSection = new BookSection { ID = id },
                        Reader = new UserInfo { ID = ((UserIdentity)User.Identity).ID },
                        LastAccessIp = HttpContext.Current.Request.UserHostName,
                        LastViewTime = DateTime.Now,
                        LastViewAt = playPos,
                        ProgressAt = playPos
                    });
                }
                else
                {
                    // 再次学习此课件
                    learner_CourseInfo.LastAccessIp = HttpContext.Current.Request.UserHostName;
                    learner_CourseInfo.LastViewTime = DateTime.Now;
                    learner_CourseInfo.LastViewAt = playPos;
                    if (playPos > learner_CourseInfo.ProgressAt)
                    {
                        // 如果最新播放位置大于学习进度，则学习进度更新 为 当前播放位置
                        learner_CourseInfo.ProgressAt = playPos;
                    }
                    this._user_BookSectionService.Update(learner_CourseInfo);
                }
            }
            catch (Exception ex)
            {
            }

            return responseData;
        }
        #endregion

        #region 添加课程内容-视频
        /// <summary>
        /// 添加课程内容-视频
        /// </summary>
        /// <param name="id">课程盒ID</param>
        /// <param name="title"></param>
        /// <param name="playUrl"></param>
        /// <returns></returns>
        [NeedAuth]
        [HttpPost]
        [Route("AddVideo")]
        public ResponseData AddVideo(int id, [FromBody]string title, [FromBody] string playUrl)
        {
            ResponseData responseData = null;
            try
            {
                if (BookInfoController.IsICreateCourseBox(id))
                {
                    //if (Container.Instance.Resolve<CourseBoxService>().Exist(id))
                    if (this._bookInfoService.Contains(m => m.ID == id && !m.IsDeleted))
                    {
                        //Container.Instance.Resolve<VideoInfoService>().Create(new VideoInfo
                        //{
                        //    Title = title,
                        //    PlayUrl = playUrl,
                        //    CourseBox = new CourseBox { ID = id }
                        //});
                        this._bookSectionService.Create(new BookSection
                        {
                            Title = title,
                            Content = playUrl,
                            BookInfoId = id
                        });

                        responseData = new ResponseData
                        {
                            Code = 1,
                            Message = "添加视频成功"
                        };
                    }
                    else
                    {
                        responseData = new ResponseData
                        {
                            Code = -2,
                            Message = "指定的课程不存在"
                        };
                    }
                }
                else
                {
                    responseData = new ResponseData
                    {
                        Code = -2,
                        Message = "无权操作此课程"
                    };
                }

            }
            catch (Exception ex)
            {
                responseData = new ResponseData
                {
                    Code = -1,
                    Message = "添加视频失败"
                };
            }

            return responseData;
        }
        #endregion

        #region 获取此视频的历史记录
        [NeedAuth]
        [HttpGet]
        [Route("VideoHistory")]
        public ResponseData VideoHistory(int videoId)
        {
            ResponseData responseData = null;
            try
            {
                BookSectionHistoryViewModel viewModel = new BookSectionHistoryViewModel();
                int currentUserId = AccountManager.GetCurrentAccount().UserId;
                User_BookSection learner_VideoInfo = this._user_BookSectionService.Find(m => m.ReaderId == currentUserId && m.BookSectionId == videoId && !m.IsDeleted);
                if (learner_VideoInfo != null)
                {
                    viewModel.Reader = new BookSectionHistoryViewModel.ReaderModel
                    {
                        ID = learner_VideoInfo.ReaderId,
                        Avatar = learner_VideoInfo.Reader.Avatar.ToHttpAbsoluteUrl(),
                        UserName = learner_VideoInfo.Reader.UserName
                    };
                    viewModel.Section = new BookSectionHistoryViewModel.SectionModel
                    {
                        ID = learner_VideoInfo.BookSectionId,
                        Title = learner_VideoInfo.BookSection.Title,
                    };
                    viewModel.LastViewAt = learner_VideoInfo.LastViewAt;
                    viewModel.LastViewTime = learner_VideoInfo.LastViewTime.ToTimeStamp13();
                    viewModel.ProgressAt = learner_VideoInfo.ProgressAt;
                }

                responseData = new ResponseData
                {
                    Code = 1,
                    Message = "获取此视频的历史记录成功",
                    Data = viewModel
                };
            }
            catch (Exception ex)
            {
                responseData = new ResponseData
                {
                    Code = -1,
                    Message = "获取此视频的历史记录失败"
                };
            }

            return responseData;
        }
        #endregion

        #region 上传视频-为某课程上传视频
        [HttpPost]
        [NeedAuth]
        [Route("UploadVideo")]
        public ResponseData UploadVideo(int id)
        {
            ResponseData responseData = null;
            try
            {
                if (BookInfoController.IsICreateCourseBox(id))
                {
                    string basePath = "~/Upload/videos/" + User.Identity.Name + "/";

                    // 如果路径含有~，即需要服务器映射为绝对路径，则进行映射
                    basePath = (basePath.IndexOf("~") > -1) ? System.Web.HttpContext.Current.Server.MapPath(basePath) : basePath;
                    HttpPostedFile file = System.Web.HttpContext.Current.Request.Files[0];
                    // 如果目录不存在，则创建目录
                    if (!Directory.Exists(basePath))
                    {
                        Directory.CreateDirectory(basePath);
                    }

                    string fileName = System.Web.HttpContext.Current.Request["name"];
                    if (string.IsNullOrEmpty(fileName))
                    {
                        fileName = file.FileName;
                    }
                    // 文件保存
                    string saveFileName = Guid.NewGuid().ToString() + "." + file.FileName.Split('.')[1];
                    string fullPath = basePath + saveFileName;
                    file.SaveAs(fullPath);

                    // TODO: 临时
                    responseData = new ResponseData
                    {
                        Code = 1,
                        Message = "上传成功",
                        Data = new
                        {
                            Url = "/Upload/videos/" + User.Identity.Name + "/" + saveFileName
                        }
                    };
                }
                else
                {
                    responseData = new ResponseData
                    {
                        Code = -2,
                        Message = "无权上传视频"
                    };
                }
            }
            catch (Exception ex)
            {
                responseData = new ResponseData
                {
                    Code = -1,
                    Message = "上传失败"
                };
            }

            return responseData;
        }
        #endregion

        #region 是我创建的课程内容-视频?
        /// <summary>
        /// 是我创建的课程内容-视频?
        /// </summary>
        /// <param name="id">视频课件ID</param>
        /// <returns></returns>
        [HttpGet]
        [Route("IsICreate")]
        public bool IsICreate(int id)
        {
            if (this._bookSectionService.Contains(m => m.ID == id && !m.IsDeleted))
            {
                int courseBoxId = this._bookSectionService.Find(id).BookInfoId;
                if (BookInfoController.IsICreateCourseBox(courseBoxId))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                // 不存在 false
                return false;
            }
        }
        #endregion

        #region 是我学习的课程内容-视频?
        /// <summary>
        /// 是我学习的课程内容-视频?
        /// </summary>
        /// <param name="id">视频课件ID</param>
        /// <returns></returns>
        [HttpGet]
        [Route("IsILearn")]
        public bool IsILearn(int id)
        {
            if (this._bookSectionService.Contains(m => m.ID == id && !m.IsDeleted))
            {
                int courseBoxId = this._bookSectionService.Find(id).BookInfoId;
                if (BookInfoController.IsILearnCourseBox(courseBoxId))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                // 不存在 false
                return false;
            }
        }
        #endregion

        #region Helpers



        #endregion
    }
}
