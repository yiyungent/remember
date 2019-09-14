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
        #region Get: 获取指定ID的课程内容
        /// <summary>
        /// 需登陆 && (属于 我学习的课程 && 属于 我创建的课程)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [NeedAuth]
        public ResponseData Get(int id)
        {
            ResponseData responseData = null;

            CourseInfoService courseInfoService = Container.Instance.Resolve<CourseInfoService>();

            if (courseInfoService.Exist(id))
            {
                CourseInfo dbModel = courseInfoService.GetEntity(id);
                int courseBoxId = dbModel.CourseBox.ID;

                if (CourseBoxController.IsICreateCourseBox(courseBoxId) || CourseBoxController.IsILearnCourseBox(courseBoxId))
                {
                    // 我学习的课程列表和我创建的课程列表中有此课程 -> 可以访问
                    CourseInfoViewModel viewModel = new CourseInfoViewModel()
                    {
                        ID = dbModel.ID,
                        Title = dbModel.Title,
                        Content = dbModel.Content,
                        CourseInfoType = (int)dbModel.CourseInfoType,
                        CourseBoxId = dbModel.CourseBox.ID,
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
                        Code = -2,
                        Message = "你没有学习或创建此课程，无权访问",
                    };
                }
            }
            else
            {
                responseData = new ResponseData
                {
                    Code = -1,
                    Message = "不存在此课程内容"
                };
            }

            return responseData;
        }
        #endregion

        #region 课件详细历史记录
        [NeedAuth]
        public ResponseData History(int courseInfoId)
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
