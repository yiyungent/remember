using Common;
using Core;
using Domain;
using Framework.Infrastructure.Concrete;
using NHibernate.Criterion;
using Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Areas.Admin.Models.Common;
using WebUI.Areas.Admin.Models.CourseBoxVM;
using WebUI.Extensions;

namespace WebUI.Areas.Admin.Controllers
{
    public class CourseBoxController : Controller
    {
        #region Ctor
        public CourseBoxController()
        {
        }
        #endregion

        #region 课程列表
        public ViewResult Index(int pageIndex = 1, int pageSize = 8)
        {
            IList<ICriterion> queryConditions = new List<ICriterion>();
            Query(queryConditions);

            ListViewModel<CourseBox> viewModel = new ListViewModel<CourseBox>(queryConditions, pageIndex: pageIndex, pageSize: pageSize);
            TempData["RedirectUrl"] = Request.RawUrl;

            return View(viewModel);
        }

        private void Query(IList<ICriterion> queryConditions)
        {
            // 输入的查询关键词
            string query = Request["q"]?.Trim() ?? "";
            // 查询类型
            QueryType queryType = new QueryType();
            queryType.Val = Request["type"]?.Trim() ?? "name";
            switch (queryType.Val.ToLower())
            {
                case "name":
                    queryType.Text = "课程名";
                    queryConditions.Add(Expression.Like("Name", query, MatchMode.Anywhere));
                    break;
                case "desc":
                    queryType.Text = "课程描述";
                    queryConditions.Add(Expression.Like("Description", query, MatchMode.Anywhere));
                    break;
                case "id":
                    queryType.Text = "ID";
                    queryConditions.Add(Expression.Eq("ID", int.Parse(query)));
                    break;
                default:
                    queryType.Text = "课程名";
                    queryConditions.Add(Expression.Like("Name", query, MatchMode.Anywhere));
                    break;
            }
            ViewBag.Query = query;
            ViewBag.QueryType = queryType;
        }
        #endregion

        #region 创建课程-基本信息
        [HttpGet]
        public ViewResult Create()
        {
            CourseBoxCreateViewModel viewModel = new CourseBoxCreateViewModel();

            return View(viewModel);
        }

        [HttpPost]
        public JsonResult Create(CourseBoxCreateViewModel inputModel)
        {
            try
            {
                string picUrl = "https://static.runoob.com/images/mix/img_fjords_wide.jpg";
                if (!string.IsNullOrEmpty(inputModel.PicUrl))
                {
                    picUrl = inputModel.PicUrl;
                }
                Container.Instance.Resolve<CourseBoxService>().Create(new CourseBox
                {
                    CreateTime = DateTime.Now,
                    LastUpdateTime = DateTime.Now,
                    Creator = AccountManager.GetCurrentUserInfo(),
                    Description = inputModel.Description,
                    Name = inputModel.Name,
                    PicUrl = picUrl,
                    StartTime = inputModel.StartTime,
                    EndTime = inputModel.EndTime,
                });

                return Json(new { code = 1, message = "创建课程成功" });
            }
            catch (Exception ex)
            {
                return Json(new { code = -1, message = "创建课程失败" });
            }
        }
        #endregion

        #region 删除课程
        public JsonResult Delete(int id)
        {
            try
            {
                Container.Instance.Resolve<CourseBoxService>().Delete(id);

                return Json(new { code = 1, message = "删除成功" });
            }
            catch (Exception ex)
            {
                return Json(new { code = -1, message = "删除失败" });
            }
        }
        #endregion


        #region 编辑课程基本信息
        [HttpGet]
        public ViewResult Edit(int id)
        {
            CourseBox viewModel = Container.Instance.Resolve<CourseBoxService>().GetEntity(id);
            string picUrl = viewModel.PicUrl.ToHttpAbsoluteUrl();
            FileHelper.GetRemoteImageInfo(picUrl, out long size, out string widthxHeight);
            ViewBag.PicUrl = picUrl;
            ViewBag.PicSize = size;

            return View(viewModel);
        }

        [HttpPost]
        public JsonResult Edit(CourseBox inputModel)
        {
            try
            {
                CourseBox dbModel = Container.Instance.Resolve<CourseBoxService>().GetEntity(inputModel.ID);
                dbModel.Name = inputModel.Name;
                dbModel.Description = inputModel.Description;
                dbModel.LastUpdateTime = DateTime.Now;
                dbModel.StartTime = inputModel.StartTime;
                dbModel.EndTime = inputModel.EndTime;
                if (string.IsNullOrEmpty(inputModel.PicUrl))
                {
                    // 默认课程封面
                    dbModel.PicUrl = "https://static.runoob.com/images/mix/img_fjords_wide.jpg";
                }
                else
                {
                    dbModel.PicUrl = inputModel.PicUrl;
                }

                Container.Instance.Resolve<CourseBoxService>().Edit(dbModel);

                return Json(new { code = 1, message = "编辑课程信息成功" });
            }
            catch (Exception ex)
            {
                return Json(new { code = -1, message = "编辑课程信息失败" });
            }
        }
        #endregion

        #region 展示此课程的视频课件列表
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">课程ID CourseBOx.ID</param>
        /// <returns></returns>
        [HttpGet]
        public ViewResult Videos(int id, int pageIndex = 1, int pageSize = 6)
        {
            IList<ICriterion> queryConditions = new List<ICriterion>();
            // 此课程的视频课件
            queryConditions.Add(Expression.Eq("CourseBox.ID", id));

            ListViewModel<VideoInfo> viewModel = new ListViewModel<VideoInfo>(queryConditions, pageIndex: pageIndex, pageSize: pageSize);
            TempData["RedirectUrl"] = Request.RawUrl;

            // 按排序码排序
            viewModel.List = viewModel.List.OrderBy(m => m.Page).ToList();

            ViewBag.CourseBox = Container.Instance.Resolve<CourseBoxService>().GetEntity(id);

            return View(viewModel);
        }
        #endregion

        #region 为课程添加视频课件
        [HttpGet]
        public ViewResult AddVideo(int courseBoxId)
        {
            CourseBox viewModel = Container.Instance.Resolve<CourseBoxService>().GetEntity(courseBoxId);


            #region 七牛云上传 token 生成

            //Qiniu.Util.Mac mac = new Qiniu.Util.Mac("-qcYpNjgUPskDq5-0LlBsKrWERYhKVZIjx4EL3uY", "bapt2y5mwQIjtMhA1FqeCKSvZVEIuzGfIU5Sk4RA");
            //Qiniu.Storage.PutPolicy putPolicy = new Qiniu.Storage.PutPolicy();
            //putPolicy.Scope = "rem-static";
            //// 自定义七牛返回消息
            //putPolicy.ReturnBody = "{\"key\":\"$(key)\",\"hash\":\"$(etag)\",\"fsiz\":$(fsize),\"bucket\":\"$(bucket)\",\"name\":\"$(x:name)\"}";
            //string token = Qiniu.Util.Auth.CreateUploadToken(mac, putPolicy.ToJsonString());
            //ViewBag.UpToken = token;

            #endregion



            return View(viewModel);
        }

        [HttpPost]
        public JsonResult AddVideo(int courseBoxId, VideoInfo inputModel)
        {
            try
            {
                Container.Instance.Resolve<VideoInfoService>().Create(new VideoInfo
                {
                    CourseBox = new CourseBox { ID = courseBoxId }, // 注意！！！
                    Page = inputModel.Page,
                    PlayUrl = inputModel.PlayUrl,
                    SubTitleUrl = inputModel.SubTitleUrl,
                    Title = inputModel.Title,
                    Size = inputModel.Size
                });


                return Json(new { code = 1, message = "添加视频课件成功" });
            }
            catch (Exception ex)
            {
                return Json(new { code = -1, message = "添加视频课件失败" });
            }
        }
        #endregion



        #region 删除视频课件
        /// <summary>
        /// 删除视频课件
        /// </summary>
        /// <param name="id">VideoInfo.ID</param>
        /// <returns></returns>
        public JsonResult DeleteVideo(int id)
        {
            try
            {
                if (Container.Instance.Resolve<VideoInfoService>().Exist(id))
                {
                    VideoInfo videoInfo = Container.Instance.Resolve<VideoInfoService>().GetEntity(id);
                    string playUrl = videoInfo.PlayUrl;
                    if (playUrl.Contains(":WebUI:"))
                    {
                        string filePath = Server.MapPath(playUrl.Replace(":WebUI:", ""));
                        System.IO.File.Delete(filePath);
                    }
                    else if (playUrl.Contains(":WebApi:"))
                    {
                        // TODO:调取 WebApi 删除
                    }
                    else
                    {
                        // TODO:无法删除视频物理文件
                    }
                    // TODO: 删除数据库记录， 此处应该使用事务，如果视频文件未成功删除，那么回滚记录
                    // 或者后期出一个清理功能，清理未被引用的垃圾文件，eg：未被视频课件playUrl引用的视频文件
                    videoInfo.Status = Domain.Base.StatusEnum.Deleted;
                    Container.Instance.Resolve<VideoInfoService>().Edit(videoInfo);
                    // 相关学习记录删除
                    //IList<Learner_VideoInfo> learner_VideoInfos = Container.Instance.Resolve<Learner_VideoInfoService>().Query(new List<ICriterion>
                    //{
                    //    Expression.Eq("VideoInfo.ID", videoInfo.ID)
                    //});
                    //foreach (var item in learner_VideoInfos)
                    //{
                    //    item.Status = Domain.Base.StatusEnum.Deleted;
                    //    Container.Instance.Resolve<Learner_VideoInfoService>().Edit(item);
                    //}
                    // TODO: 要删除的记录太多，不可能让用户等待这么久删除，做站点优化-清除冗余数据功能

                    return Json(new { code = 1, message = "删除视频课件成功" });
                }
                else
                {
                    return Json(new { code = -2, message = "删除视频课件失败，该视频不存在" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = -1, message = "删除视频课件失败" });
            }
        }
        #endregion

        #region 编辑视频课件
        [HttpGet]
        public ViewResult EditVideo(int id)
        {
            VideoInfo viewModel = Container.Instance.Resolve<VideoInfoService>().GetEntity(id);

            return View(viewModel);
        }

        [HttpPost]
        public JsonResult EditVideo(VideoInfo inputModel)
        {
            try
            {
                VideoInfo dbModel = Container.Instance.Resolve<VideoInfoService>().GetEntity(inputModel.ID);
                dbModel.Title = inputModel.Title;
                dbModel.PlayUrl = inputModel.PlayUrl;
                dbModel.SubTitleUrl = inputModel.SubTitleUrl;
                dbModel.Page = inputModel.Page;
                dbModel.Size = inputModel.Size;


                Container.Instance.Resolve<VideoInfoService>().Edit(dbModel);

                return Json(new { code = 1, message = "编辑视频课件成功" });
            }
            catch (Exception ex)
            {
                return Json(new { code = -1, message = "编辑视频课件失败" });
            }
        }
        #endregion








        #region 上传视频-为某课程上传视频
        /// <summary>
        /// 上传视频-为某课程上传视频
        /// </summary>
        /// <param name="id">课程ID</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UploadVideo(int id)
        {
            try
            {
                if (Container.Instance.Resolve<CourseBoxService>().Exist(id))
                {
                    int courseBoxCreatorId = Container.Instance.Resolve<CourseBoxService>().GetEntity(id).Creator.ID;

                    // 保存到课程创建者的文件夹
                    string basePath = "~/Upload/videos/" + courseBoxCreatorId + "/";

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
                    return Json(new
                    {
                        result = "ok",
                        message = "上传视频成功",
                        url = (":WebUISite:/Upload/videos/" + courseBoxCreatorId + "/" + saveFileName)
                    });
                }
                else
                {
                    return Json(new
                    {
                        result = "failed",
                        message = "上传失败，该课程不存在"
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    result = "failed",
                    message = "上传失败"
                });
            }
        }
        #endregion

        #region 上传视频-为某课程上传字幕
        /// <summary>
        /// 上传视频-为某课程上传视频
        /// </summary>
        /// <param name="id">课程ID</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UploadSubTitle(int id)
        {
            try
            {
                if (Container.Instance.Resolve<CourseBoxService>().Exist(id))
                {
                    int courseBoxCreatorId = Container.Instance.Resolve<CourseBoxService>().GetEntity(id).Creator.ID;

                    // 保存到课程创建者的文件夹
                    string basePath = "~/Upload/subtitles/" + courseBoxCreatorId + "/";

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
                    return Json(new
                    {
                        result = "ok",
                        message = "上传成功",
                        url = (":WebUISite:/Upload/subtitles/" + courseBoxCreatorId + "/" + saveFileName)
                    });
                }
                else
                {
                    return Json(new
                    {
                        result = "failed",
                        message = "上传失败，该课程不存在",
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    result = "failed",
                    message = "上传失败"
                });
            }
        }
        #endregion

        #region 上传课程封面图
        [HttpPost]
        public JsonResult UploadPic()
        {
            try
            {
                int currentUserId = AccountManager.GetCurrentUserInfo().ID;

                // 保存到课程创建者的文件夹
                string basePath = "~/Upload/images/" + currentUserId + "/";

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
                return Json(new
                {
                    result = "ok",
                    message = "上传成功",
                    url = (":WebUISite:/Upload/images/" + currentUserId + "/" + saveFileName)
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    result = "failed",
                    message = "上传失败"
                });
            }
        }
        #endregion

        // TODO: 当在 编辑课程基本信息 页，删除封面图时，发送请求到后端，删除物理文件

    }
}