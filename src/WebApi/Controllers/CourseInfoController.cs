using Common;
using Core;
using Domain;
using NHibernate.Criterion;
using Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using WebApi.Attributes;
using WebApi.Infrastructure;
using WebApi.Models.Common;
using WebApi.Models.CourseInfoVM;

namespace WebApi.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/CourseInfo")]
    public class CourseInfoController : ApiController
    {
        #region Get: 获取指定ID的课程内容-课件
        /// <summary>
        /// 需登陆 && (属于 我学习的课程 || 属于 我创建的课程)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [NeedAuth]
        public ResponseData Get(int id)
        {
            ResponseData responseData = null;

            Learner_CourseInfo learner_CourseInfo = Container.Instance.Resolve<Learner_CourseInfoService>().Query(new List<ICriterion>
            {
                Expression.And(
                    Expression.Eq("Learner.ID", ((UserIdentity)User.Identity).ID),
                    Expression.Eq("CourseInfo.ID", id)
                )
            }).FirstOrDefault();

            if (learner_CourseInfo != null)
            {
                // 属于我学习的课程
                CourseInfo courseInfo = learner_CourseInfo.CourseInfo;
                int courseBoxId = courseInfo.CourseBox.ID;

                // 我学习的课程列表中有此课程 -> 可以访问
                CourseInfoViewModel viewModel = new CourseInfoViewModel()
                {
                    ID = courseInfo.ID,
                    Title = courseInfo.Title,
                    Content = courseInfo.Content,
                    CourseInfoType = (int)courseInfo.CourseInfoType,
                    CourseBoxId = courseBoxId,
                    LastAccessCountry = learner_CourseInfo.LastAccessCountry,
                    LastAccessIp = learner_CourseInfo.LastAccessIp,
                    LastAccessTime = learner_CourseInfo.LastAccessTime.ToTimeStamp13(),
                    LastPlayAt = learner_CourseInfo.LastPlayAt,
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
                CourseInfo courseInfo = Container.Instance.Resolve<CourseInfoService>().GetEntity(id);
                bool isMeCreate = courseInfo.CourseBox.Creator.ID == ((UserIdentity)User.Identity).ID;
                if (isMeCreate)
                {
                    // 未加入学习，但是我创建的课程
                    CourseInfoViewModel viewModel = new CourseInfoViewModel()
                    {
                        ID = courseInfo.ID,
                        Title = courseInfo.Title,
                        Content = courseInfo.Content,
                        CourseInfoType = (int)courseInfo.CourseInfoType,
                        CourseBoxId = courseInfo.CourseBox.ID,
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
                Learner_CourseInfo learner_CourseInfo = Container.Instance.Resolve<Learner_CourseInfoService>().Query(new List<ICriterion>
                {
                    Expression.And(
                        Expression.Eq("Learner.ID", ((UserIdentity)User.Identity).ID),
                        Expression.Eq("CourseInfo.ID", id)
                    )
                }).FirstOrDefault();
                if (learner_CourseInfo == null)
                {
                    // 第一次学习此课件
                    Container.Instance.Resolve<Learner_CourseInfoService>().Create(new Learner_CourseInfo
                    {
                        CourseInfo = new CourseInfo { ID = id },
                        Learner = new UserInfo { ID = ((UserIdentity)User.Identity).ID },
                        LastAccessIp = HttpContext.Current.Request.UserHostName,
                        LastAccessTime = DateTime.Now,
                        LastPlayAt = playPos,
                        ProgressAt = playPos
                    });
                }
                else
                {
                    // 再次学习此课件
                    learner_CourseInfo.LastAccessIp = HttpContext.Current.Request.UserHostName;
                    learner_CourseInfo.LastAccessTime = DateTime.Now;
                    learner_CourseInfo.LastPlayAt = playPos;
                    if (playPos > learner_CourseInfo.ProgressAt)
                    {
                        // 如果最新播放位置大于学习进度，则学习进度更新 为 当前播放位置
                        learner_CourseInfo.ProgressAt = playPos;
                    }
                    Container.Instance.Resolve<Learner_CourseInfoService>().Edit(learner_CourseInfo);
                }
            }
            catch (Exception ex)
            {
            }

            return responseData;
        }
        #endregion

        #region 课件详细历史记录
        [NeedAuth]
        [HttpGet]
        [Route("History")]
        public ResponseData History(int courseInfoId)
        {
            ResponseData responseData = null;
            try
            {
                HistoryViewModel viewModel = null;

                Learner_CourseInfo learner_CourseInfo = Container.Instance.Resolve<Learner_CourseInfoService>().Query(new List<ICriterion>
                {
                    Expression.And(
                        Expression.Eq("CourseInfo.ID", courseInfoId),
                        Expression.Eq("Learner.ID", ((UserIdentity)User.Identity).ID)
                    )
                }).FirstOrDefault();

                if (learner_CourseInfo != null)
                {
                    viewModel = new HistoryViewModel()
                    {
                        Ip = learner_CourseInfo.LastAccessIp,
                        Country = learner_CourseInfo.LastAccessCountry,
                        LastAccessTime = learner_CourseInfo.LastAccessTime.ToTimeStamp13(),
                        CourseBoxId = learner_CourseInfo.CourseInfo.CourseBox.ID,
                        Duration = learner_CourseInfo.CourseInfo.Duration,
                        ProgressAt = learner_CourseInfo.ProgressAt,
                        LastPlayAt = learner_CourseInfo.LastPlayAt
                    };
                }

                responseData = new ResponseData
                {
                    Code = 1,
                    Message = "获取课件详细历史记录成功",
                    Data = viewModel
                };
            }
            catch (Exception ex)
            {
                responseData = new ResponseData
                {
                    Code = -1,
                    Message = "获取课件详细历史记录失败"
                };
            }

            return responseData;
        }
        #endregion

        #region 添加课程内容-视频
        [NeedAuth]
        [HttpPost]
        [Route("AddVideo")]
        public ResponseData AddVideo(int id, [FromBody]VideoViewModel model)
        {
            ResponseData responseData = null;
            try
            {
                if (CourseBoxController.IsICreateCourseBox(id))
                {
                    CourseInfoService courseInfoService = Container.Instance.Resolve<CourseInfoService>();
                    CourseBoxService courseBoxService = Container.Instance.Resolve<CourseBoxService>();
                    CourseBox courseBox = courseBoxService.GetEntity(id);

                    courseInfoService.Create(new CourseInfo
                    {
                        Title = model.Title,
                        Content = model.Url,
                        CourseBox = courseBox,
                        CourseInfoType = CourseInfoType.Video
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

        #region 添加课程内容-富文本贴
        /// <summary>
        /// 添加课程内容-富文本贴
        /// </summary>
        /// <param name="id">课程盒ID，在此课程盒内添加</param>
        /// <param name="model"></param>
        /// <returns></returns>
        [NeedAuth]
        [HttpPost]
        [Route("AddRichText")]
        public ResponseData AddRichText(int id, [FromBody]RichTextViewModel model)
        {
            ResponseData responseData = null;
            try
            {
                if (CourseBoxController.IsICreateCourseBox(id))
                {
                    CourseInfoService courseInfoService = Container.Instance.Resolve<CourseInfoService>();
                    CourseBoxService courseBoxService = Container.Instance.Resolve<CourseBoxService>();
                    CourseBox courseBox = courseBoxService.GetEntity(id);

                    courseInfoService.Create(new CourseInfo
                    {
                        Title = model.Title,
                        Content = model.Content,
                        CourseBox = courseBox,
                        CourseInfoType = CourseInfoType.RichText
                    });

                    responseData = new ResponseData
                    {
                        Code = 1,
                        Message = "添加成功"
                    };
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
                    Message = "添加失败"
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
                if (CourseBoxController.IsICreateCourseBox(id))
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

        #region 是我创建的课程内容?
        [HttpGet]
        [Route("IsICreate")]
        public bool IsICreate(int id)
        {
            CourseInfoService courseInfoService = Container.Instance.Resolve<CourseInfoService>();
            if (courseInfoService.Exist(id))
            {
                CourseBox courseBox = courseInfoService.GetEntity(id).CourseBox;
                if (CourseBoxController.IsICreateCourseBox(courseBox.ID))
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

        #region 是我学习的课程内容?
        [HttpGet]
        [Route("IsILearn")]
        public bool IsILearn(int id)
        {
            CourseInfoService courseInfoService = Container.Instance.Resolve<CourseInfoService>();
            if (courseInfoService.Exist(id))
            {
                CourseBox courseBox = courseInfoService.GetEntity(id).CourseBox;
                if (CourseBoxController.IsILearnCourseBox(courseBox.ID))
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
