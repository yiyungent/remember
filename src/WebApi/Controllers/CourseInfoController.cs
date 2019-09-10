using Core;
using Domain;
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
using WebApi.Models.Common;
using WebApi.Models.CourseInfoVM;

namespace WebApi.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/CourseInfo")]
    public class CourseInfoController : ApiController
    {
        #region Get: 获取指定ID的课程内容
        public ResponseData Get(int id)
        {
            ResponseData responseData = null;
            CourseInfoService courseInfoService = Container.Instance.Resolve<CourseInfoService>();
            if (courseInfoService.Exist(id))
            {
                CourseInfo dbModel = courseInfoService.GetEntity(id);
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
                    Message = "成功",
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
                CourseInfoService courseInfoService = Container.Instance.Resolve<CourseInfoService>();
                CourseBoxService courseBoxService = Container.Instance.Resolve<CourseBoxService>();
                CourseBox courseBox = courseBoxService.GetEntity(id);

                courseInfoService.Create(new CourseInfo
                {
                    Title = model.Title,
                    Content = model.Url,
                    CourseBox = courseBox
                });

                responseData = new ResponseData
                {
                    Code = 1,
                    Message = "添加视频成功"
                };
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
                CourseInfoService courseInfoService = Container.Instance.Resolve<CourseInfoService>();
                CourseBoxService courseBoxService = Container.Instance.Resolve<CourseBoxService>();
                CourseBox courseBox = courseBoxService.GetEntity(id);

                courseInfoService.Create(new CourseInfo
                {
                    Title = model.Title,
                    Content = model.Content,
                    CourseBox = courseBox
                });

                responseData = new ResponseData
                {
                    Code = 1,
                    Message = "添加成功"
                };
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

        #region 上传视频
        [HttpPost]
        //[NeedAuth]
        [Route("UploadVideo")]
        public ResponseData UploadVideo()
        {
            ResponseData responseData = null;
            try
            {
                //string basePath = "~/Upload/videos/" + User.Identity.Name + "/";
                string basePath = "~/Upload/videos/" + "admin" + "/";

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
                        //Url = "/Upload/videos/" + User.Identity.Name + "/" + saveFileName
                        Url = "/Upload/videos/" + "admin" + "/" + saveFileName
                    }
                };
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

    }
}
