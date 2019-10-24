using Core;
using Domain;
using Domain.Entities;
using Framework.Extensions;
using Framework.Infrastructure.Concrete;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Areas.Admin.Models.Common;
using WebUI.Areas.Admin.Models.CourseBoxVM;
using WebUI.Extensions;
using WebUI.Models.Common;

namespace WebUI.Areas.Admin.Controllers
{
    public class CourseBoxController : Controller
    {
        #region Fields
        private readonly ICourseBoxService _courseBoxService;
        private readonly IVideoInfoService _videoInfoService;
        #endregion

        #region Ctor
        public CourseBoxController(ICourseBoxService courseBoxService,
            IVideoInfoService videoInfoService)
        {
            this._courseBoxService = courseBoxService;
            this._videoInfoService = videoInfoService;
        }
        #endregion

        #region 课程列表
        public ViewResult Index(int pageIndex = 1, int pageSize = 8)
        {
            Query(pageIndex, pageSize, out IList<CourseBox> list, out int totalCount);

            ListViewModel<CourseBox> viewModel = new ListViewModel<CourseBox>(list, pageIndex: pageIndex, pageSize: pageSize, totalCount: totalCount);
            TempData["RedirectUrl"] = Request.RawUrl;

            return View(viewModel);
        }

        private void Query(int pageIndex, int pageSize, out IList<CourseBox> list, out int totalCount)
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
                    list = this._courseBoxService.Filter<int>(pageIndex, pageSize, out totalCount, m => m.Name.Contains(query) && !m.IsDeleted, m => m.ID, false).ToList();
                    break;
                case "desc":
                    queryType.Text = "课程描述";
                    list = this._courseBoxService.Filter<int>(pageIndex, pageSize, out totalCount, m => m.Description.Contains(query) && !m.IsDeleted, m => m.ID, false).ToList();
                    break;
                case "id":
                    queryType.Text = "ID";
                    int courseBoxId = int.Parse(query);
                    list = this._courseBoxService.Filter<int>(pageIndex, pageSize, out totalCount, m => m.ID == courseBoxId && !m.IsDeleted, m => m.ID, false).ToList();
                    break;
                default:
                    queryType.Text = "课程名";
                    list = this._courseBoxService.Filter<int>(pageIndex, pageSize, out totalCount, m => m.Name.Contains(query) && !m.IsDeleted, m => m.ID, false).ToList();
                    break;
            }
            ViewBag.Query = query;
            ViewBag.QueryType = queryType;
        }
        #endregion

        #region 添加课程-基本信息
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
                this._courseBoxService.Create(new CourseBox
                {
                    CreateTime = DateTime.Now,
                    LastUpdateTime = DateTime.Now,
                    CreatorId = AccountManager.GetCurrentUserInfo().ID,
                    Description = inputModel.Description,
                    Name = inputModel.Name,
                    PicUrl = ":WebUISite:" + picUrl,
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
                var dbModel = this._courseBoxService.Find(m => m.ID == id && !m.IsDeleted);
                dbModel.IsDeleted = true;
                dbModel.DeletedAt = DateTime.Now;
                this._courseBoxService.Update(dbModel);

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
            //CourseBox viewModel = Container.Instance.Resolve<CourseBoxService>().GetEntity(id);
            CourseBox viewModel = this._courseBoxService.Find(m => m.ID == id && !m.IsDeleted);
            // 如果为本站上的图片，则会有标记，去除标记
            viewModel.PicUrl = viewModel.PicUrl.Replace(":WebUISite:", "");

            string picUrl = viewModel.PicUrl.ToHttpAbsoluteUrl();
            Core.Common.FileHelper.GetRemoteImageInfo(picUrl, out long size, out string widthxHeight);
            ViewBag.PicUrl = picUrl;
            ViewBag.PicSize = size;

            return View(viewModel);
        }

        [HttpPost]
        public JsonResult Edit(CourseBox inputModel)
        {
            try
            {
                //CourseBox dbModel = Container.Instance.Resolve<CourseBoxService>().GetEntity(inputModel.ID);
                CourseBox dbModel = this._courseBoxService.Find(m => m.ID == inputModel.ID && !m.IsDeleted);
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
                    // 判断是否包含 http
                    // 包含 http，则为其它网络资源（可能为上传到阿里云oss的），否则为上传到本服务器的资源，可直接使用相对地址
                    if (dbModel.PicUrl.ToLower().Contains("http://") || dbModel.PicUrl.ToLower().Contains("https://"))
                    {
                        dbModel.PicUrl = inputModel.PicUrl;
                    }
                    else
                    {
                        // 打上文件在本服务器的标记
                        dbModel.PicUrl = ":WebUISite:" + inputModel.PicUrl;
                    }
                }

                //Container.Instance.Resolve<CourseBoxService>().Edit(dbModel);
                this._courseBoxService.Update(dbModel);

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
            //IList<ICriterion> queryConditions = new List<ICriterion>();
            // 此课程的视频课件
            //queryConditions.Add(Expression.Eq("CourseBox.ID", id));

            IList<VideoInfo> list = this._videoInfoService.Filter<int>(pageIndex, pageSize, out int totalCount, m => m.CourseBoxId == id && !m.IsDeleted, m => m.ID, false).ToList();
            ListViewModel<VideoInfo> viewModel = new ListViewModel<VideoInfo>(list, pageIndex: pageIndex, pageSize: pageSize, totalCount: totalCount);
            TempData["RedirectUrl"] = Request.RawUrl;

            // 按排序码排序
            viewModel.List = viewModel.List.OrderBy(m => m.Page).ToList();

            //ViewBag.CourseBox = Container.Instance.Resolve<CourseBoxService>().GetEntity(id);
            ViewBag.CourseBox = this._courseBoxService.Find(m => m.ID == id && !m.IsDeleted);


            return View(viewModel);
        }
        #endregion

        #region 为课程添加视频课件
        [HttpGet]
        public ViewResult AddVideo(int courseBoxId)
        {
            //CourseBox viewModel = Container.Instance.Resolve<CourseBoxService>().GetEntity(courseBoxId);
            CourseBox viewModel = this._courseBoxService.Find(m => m.ID == courseBoxId && !m.IsDeleted);

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
                #region 输入数据处理

                // 判断是否包含 http
                // 包含 http，则为其它网络资源（可能为上传到阿里云oss的），否则为上传到本服务器的资源，可直接使用相对地址
                if (inputModel.PlayUrl.ToLower().Contains("http://") || inputModel.PlayUrl.ToLower().Contains("https://"))
                {
                    inputModel.PlayUrl = inputModel.PlayUrl;
                }
                else
                {
                    // 打上文件在本服务器的标记
                    inputModel.PlayUrl = ":WebUISite:" + inputModel.PlayUrl;
                }

                #endregion

                CourseBox courseBox = this._courseBoxService.Find(m => m.ID == courseBoxId && !m.IsDeleted);
                // TODO: 正确处理输入第几集
                //int videoCount = courseBox.VideoInfos?.Count ?? 0;
                //int page = inputModel.Page;
                //if (inputModel.Page<0||inputModel)
                //{

                //}

                this._videoInfoService.Create(new VideoInfo
                {
                    CourseBoxId = courseBoxId, // 注意！！！
                    Page = inputModel.Page,
                    PlayUrl = inputModel.PlayUrl,
                    SubTitleUrl = inputModel.SubTitleUrl,
                    Title = inputModel.Title,
                    Size = inputModel.Size,
                    Duration = inputModel.Duration
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
                //if (Container.Instance.Resolve<VideoInfoService>().Exist(id))
                if (this._videoInfoService.Contains(m => m.ID == id && !m.IsDeleted))
                {
                    //VideoInfo videoInfo = Container.Instance.Resolve<VideoInfoService>().GetEntity(id);
                    VideoInfo videoInfo = this._videoInfoService.Find(m => m.ID == id && !m.IsDeleted);
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
                    //videoInfo.Status = Domain.Base.StatusEnum.Deleted;
                    //Container.Instance.Resolve<VideoInfoService>().Edit(videoInfo);
                    videoInfo.IsDeleted = true;
                    videoInfo.DeletedAt = DateTime.Now;
                    this._videoInfoService.Update(videoInfo);
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
            //VideoInfo viewModel = Container.Instance.Resolve<VideoInfoService>().GetEntity(id);
            VideoInfo viewModel = this._videoInfoService.Find(m => m.ID == id && !m.IsDeleted);

            // 如果为本站上的文件，则会有标记，去除标记,转化为相对地址
            viewModel.PlayUrl = viewModel.PlayUrl.Replace(":WebUISite:", "");

            return View(viewModel);
        }

        [HttpPost]
        public JsonResult EditVideo(VideoInfo inputModel)
        {
            try
            {
                #region 输入数据处理

                // 判断是否包含 http
                // 包含 http，则为其它网络资源（可能为上传到阿里云oss的），否则为上传到本服务器的资源，可直接使用相对地址
                if (inputModel.PlayUrl.ToLower().Contains("http://") || inputModel.PlayUrl.ToLower().Contains("https://"))
                {
                    inputModel.PlayUrl = inputModel.PlayUrl;
                }
                else
                {
                    // 打上文件在本服务器的标记
                    inputModel.PlayUrl = ":WebUISite:" + inputModel.PlayUrl;
                }

                #endregion

                //VideoInfo dbModel = Container.Instance.Resolve<VideoInfoService>().GetEntity(inputModel.ID);
                VideoInfo dbModel = this._videoInfoService.Find(m => m.ID == inputModel.ID && !m.IsDeleted);
                dbModel.Title = inputModel.Title;
                dbModel.PlayUrl = inputModel.PlayUrl;
                dbModel.SubTitleUrl = inputModel.SubTitleUrl;
                dbModel.Page = inputModel.Page;
                dbModel.Size = inputModel.Size;
                dbModel.Duration = inputModel.Duration;


                //Container.Instance.Resolve<VideoInfoService>().Edit(dbModel);
                this._videoInfoService.Update(dbModel);

                return Json(new { code = 1, message = "编辑视频课件成功" });
            }
            catch (Exception ex)
            {
                return Json(new { code = -1, message = "编辑视频课件失败" });
            }
        }
        #endregion



        // TODO: 服务端限制 文件类型：扩展名
        // TODO: 支持分片上传
        // TODO：支持站点设置-文件上传-上传方式：1.本地上传，2.阿里云OSS上传，3.FTP

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
                // 该课程存在吗？
                //if (Container.Instance.Resolve<CourseBoxService>().Exist(id))
                if (this._courseBoxService.Contains(m => m.ID == id && !m.IsDeleted))
                {
                    //int courseBoxCreatorId = Container.Instance.Resolve<CourseBoxService>().GetEntity(id).Creator.ID;
                    int courseBoxCreatorId = this._courseBoxService.Find(m => m.ID == id && !m.IsDeleted).CreatorId ?? 0;

                    // 保存到此课程的文件夹
                    string basePath = $"~/Upload/videos/courseBox/{id}/";

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
                    return Json(new ZuiFileUploadResult
                    {
                        result = "ok",
                        message = "上传视频成功",
                        // 返回相对路径，因为就是在这个服务器上传的，所以就返回相对路径，提交时，判断是否是相对路径，如果是，则说明是此服务器上传的，再加上此服务器上传的标记
                        url = $"/Upload/videos/courseBox/{id}/" + saveFileName
                    });
                }
                else
                {
                    return Json(new ZuiFileUploadResult
                    {
                        result = "failed",
                        message = "上传失败，该课程不存在"
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new ZuiFileUploadResult
                {
                    result = "failed",
                    message = "上传失败"
                });
            }
        }
        #endregion

        #region 上传字幕-为某课程上传字幕
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
                //if (Container.Instance.Resolve<CourseBoxService>().Exist(id))
                if (this._courseBoxService.Contains(m => m.ID == id && !m.IsDeleted))
                {
                    //int courseBoxCreatorId = Container.Instance.Resolve<CourseBoxService>().GetEntity(id).Creator.ID;
                    int courseBoxCreatorId = this._courseBoxService.Find(m => m.ID == id && !m.IsDeleted).CreatorId ?? 0;

                    // 保存到此课程的文件夹
                    string basePath = $"~/Upload/subtitles/courseBox/{id}/";

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
                    return Json(new ZuiFileUploadResult
                    {
                        result = "ok",
                        message = "上传成功",
                        url = $"/Upload/subtitles/courseBox/{id}/" + saveFileName
                    });
                }
                else
                {
                    return Json(new ZuiFileUploadResult
                    {
                        result = "failed",
                        message = "上传失败，该课程不存在",
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new ZuiFileUploadResult
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
                int currentUserId = AccountManager.GetCurrentAccount().UserId;

                // 保存到当前用户的文件夹
                string basePath = $"~/Upload/images/{currentUserId}/{DateTime.Now.ToString("yyyy-MM-dd")}/";

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
                string saveFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string fullPath = basePath + saveFileName;
                file.SaveAs(fullPath);

                return Json(new ZuiFileUploadResult
                {
                    result = "ok",
                    message = "上传成功",
                    url = $"/Upload/images/{currentUserId}/{DateTime.Now.ToString("yyyy-MM-dd")}/" + saveFileName
                });
            }
            catch (Exception ex)
            {
                return Json(new ZuiFileUploadResult
                {
                    result = "failed",
                    message = "上传失败"
                });
            }
        }
        #endregion

        // TODO: 当在 编辑课程基本信息 页，删除封面图时，发送请求到后端，删除物理文件，同理，当删除视频文件时，也发送请求到后端
        // 最好的做法时，服务端收到通知后，不立即删除，而是插入一条记录到队列表，并且为未开启状态，当管理员审阅后，才标记开启，这时就会被计划任务删除




        #region Zui文件上传结果类
        public sealed class ZuiFileUploadResult
        {
            /// <summary>
            /// ok
            /// failed
            /// </summary>
            public string result { get; set; }

            /// <summary>
            /// 提示消息
            /// </summary>
            public string message { get; set; }

            /// <summary>
            /// 如果开启显示下载链接，则此为文件下载链接
            /// </summary>
            public string url { get; set; }
        }
        #endregion



    }
}