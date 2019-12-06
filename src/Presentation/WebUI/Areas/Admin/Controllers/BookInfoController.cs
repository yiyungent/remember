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
using WebUI.Areas.Admin.Models.BookInfoVM;
using WebUI.Extensions;
using WebUI.Models.Common;
using Framework.Attributes;

namespace WebUI.Areas.Admin.Controllers
{
    public class BookInfoController : Controller
    {
        #region Fields
        private readonly IBookInfoService _bookInfoService;
        private readonly IBookSectionService _bookSectionService;
        #endregion

        #region Ctor
        public BookInfoController(IBookInfoService BookInfoService,
            IBookSectionService BookSectionService)
        {
            this._bookInfoService = BookInfoService;
            this._bookSectionService = BookSectionService;
        }
        #endregion

        #region 文库列表
        public ViewResult Index(int pageIndex = 1, int pageSize = 8)
        {
            Query(pageIndex, pageSize, out IList<BookInfo> list, out int totalCount);

            ListViewModel<BookInfo> viewModel = new ListViewModel<BookInfo>(list, pageIndex: pageIndex, pageSize: pageSize, totalCount: totalCount);
            TempData["RedirectUrl"] = Request.RawUrl;

            return View(viewModel);
        }

        private void Query(int pageIndex, int pageSize, out IList<BookInfo> list, out int totalCount)
        {
            // 输入的查询关键词
            string query = Request["q"]?.Trim() ?? "";
            // 查询类型
            QueryType queryType = new QueryType();
            queryType.Val = Request["type"]?.Trim() ?? "name";
            switch (queryType.Val.ToLower())
            {
                case "name":
                    queryType.Text = "文库名";
                    list = this._bookInfoService.Filter<int>(pageIndex, pageSize, out totalCount, m => m.Name.Contains(query) && !m.IsDeleted, m => m.ID, false).ToList();
                    break;
                case "desc":
                    queryType.Text = "文库描述";
                    list = this._bookInfoService.Filter<int>(pageIndex, pageSize, out totalCount, m => m.Description.Contains(query) && !m.IsDeleted, m => m.ID, false).ToList();
                    break;
                case "id":
                    queryType.Text = "ID";
                    int BookInfoId = int.Parse(query);
                    list = this._bookInfoService.Filter<int>(pageIndex, pageSize, out totalCount, m => m.ID == BookInfoId && !m.IsDeleted, m => m.ID, false).ToList();
                    break;
                default:
                    queryType.Text = "文库名";
                    list = this._bookInfoService.Filter<int>(pageIndex, pageSize, out totalCount, m => m.Name.Contains(query) && !m.IsDeleted, m => m.ID, false).ToList();
                    break;
            }
            ViewBag.Query = query;
            ViewBag.QueryType = queryType;
        }
        #endregion

        #region 添加文库-基本信息
        [HttpGet]
        public ViewResult Create()
        {
            BookInfoCreateViewModel viewModel = new BookInfoCreateViewModel();

            return View(viewModel);
        }

        [HttpPost]
        public JsonResult Create(BookInfoCreateViewModel inputModel)
        {
            try
            {
                string picUrl = "https://static.runoob.com/images/mix/img_fjords_wide.jpg";
                if (!string.IsNullOrEmpty(inputModel.PicUrl))
                {
                    picUrl = inputModel.PicUrl;
                }
                this._bookInfoService.Create(new BookInfo
                {
                    CreateTime = DateTime.Now,
                    LastUpdateTime = DateTime.Now,
                    CreatorId = AccountManager.GetCurrentUserInfo().ID,
                    Description = inputModel.Description,
                    Name = inputModel.Name,
                    PicUrl = ":WebUISite:" + picUrl,
                });

                return Json(new { code = 1, message = "创建成功" });
            }
            catch (Exception ex)
            {
                return Json(new { code = -1, message = "创建失败" });
            }
        }
        #endregion

        #region 上传文库封面图
        [AuthKey("Admin.BookInfo.Create")]
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

        #region 删除文库
        public JsonResult Delete(int id)
        {
            try
            {
                var dbModel = this._bookInfoService.Find(m => m.ID == id && !m.IsDeleted);
                dbModel.IsDeleted = true;
                dbModel.DeletedAt = DateTime.Now;
                this._bookInfoService.Update(dbModel);

                return Json(new { code = 1, message = "删除成功" });
            }
            catch (Exception ex)
            {
                return Json(new { code = -1, message = "删除失败" });
            }
        }
        #endregion


        #region 编辑文库基本信息
        [HttpGet]
        public ViewResult Edit(int id)
        {
            //BookInfo viewModel = Container.Instance.Resolve<BookInfoService>().GetEntity(id);
            BookInfo viewModel = this._bookInfoService.Find(m => m.ID == id && !m.IsDeleted);
            // 如果为本站上的图片，则会有标记，去除标记
            viewModel.PicUrl = viewModel.PicUrl.Replace(":WebUISite:", "");

            string picUrl = viewModel.PicUrl.ToHttpAbsoluteUrl();
            Core.Common.FileHelper.GetRemoteImageInfo(picUrl, out long size, out string widthxHeight);
            ViewBag.PicUrl = picUrl;
            ViewBag.PicSize = size;

            return View(viewModel);
        }

        [HttpPost]
        public JsonResult Edit(BookInfo inputModel)
        {
            try
            {
                //BookInfo dbModel = Container.Instance.Resolve<BookInfoService>().GetEntity(inputModel.ID);
                BookInfo dbModel = this._bookInfoService.Find(m => m.ID == inputModel.ID && !m.IsDeleted);
                dbModel.Name = inputModel.Name;
                dbModel.Description = inputModel.Description;
                dbModel.LastUpdateTime = DateTime.Now;
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

                //Container.Instance.Resolve<BookInfoService>().Edit(dbModel);
                this._bookInfoService.Update(dbModel);

                return Json(new { code = 1, message = "编辑课程信息成功" });
            }
            catch (Exception ex)
            {
                return Json(new { code = -1, message = "编辑课程信息失败" });
            }
        }
        #endregion

        #region 展示此文库的章节列表
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">文库ID BookInfo.ID</param>
        /// <returns></returns>
        [HttpGet]
        public ViewResult Sections(int id, int pageIndex = 1, int pageSize = 6)
        {
            //IList<ICriterion> queryConditions = new List<ICriterion>();
            // 此课程的视频课件
            //queryConditions.Add(Expression.Eq("BookInfo.ID", id));

            IList<BookSection> list = this._bookSectionService.Filter<int>(pageIndex, pageSize, out int totalCount, m => m.BookInfoId == id && !m.IsDeleted, m => m.ID, false).ToList();
            ListViewModel<BookSection> viewModel = new ListViewModel<BookSection>(list, pageIndex: pageIndex, pageSize: pageSize, totalCount: totalCount);
            TempData["RedirectUrl"] = Request.RawUrl;

            // 按排序码排序
            viewModel.List = viewModel.List.OrderBy(m => m.SortCode).ToList();

            //ViewBag.BookInfo = Container.Instance.Resolve<BookInfoService>().GetEntity(id);
            ViewBag.BookInfo = this._bookInfoService.Find(m => m.ID == id && !m.IsDeleted);


            return View(viewModel);
        }
        #endregion

        #region 为文库添加章节
        [HttpGet]
        public ViewResult AddSection(int bookId)
        {
            BookInfo viewModel = this._bookInfoService.Find(m => m.ID == bookId && !m.IsDeleted);

            return View(viewModel);
        }

        [HttpPost]
        public JsonResult AddVideo(int BookInfoId, BookSection inputModel)
        {
            try
            {
                #region 输入数据处理

                // 判断是否包含 http
                // 包含 http，则为其它网络资源（可能为上传到阿里云oss的），否则为上传到本服务器的资源，可直接使用相对地址
                if (inputModel.Content.ToLower().Contains("http://") || inputModel.Content.ToLower().Contains("https://"))
                {
                    inputModel.Content = inputModel.Content;
                }
                else
                {
                    // 打上文件在本服务器的标记
                    inputModel.Content = ":WebUISite:" + inputModel.Content;
                }

                #endregion

                BookInfo BookInfo = this._bookInfoService.Find(m => m.ID == BookInfoId && !m.IsDeleted);
                // TODO: 正确处理输入第几集
                //int videoCount = BookInfo.BookSections?.Count ?? 0;
                //int page = inputModel.Page;
                //if (inputModel.Page<0||inputModel)
                //{

                //}

                this._bookSectionService.Create(new BookSection
                {
                    BookInfoId = BookInfoId, // 注意！！！
                    SortCode = inputModel.SortCode,
                    Content = inputModel.Content,
                    Title = inputModel.Title,
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



        #region 删除文库章节
        /// <summary>
        /// 删除视频课件
        /// </summary>
        /// <param name="id">BookSection.ID</param>
        /// <returns></returns>
        public JsonResult DeleteVideo(int id)
        {
            try
            {
                //if (Container.Instance.Resolve<BookSectionService>().Exist(id))
                if (this._bookSectionService.Contains(m => m.ID == id && !m.IsDeleted))
                {
                    //BookSection BookSection = Container.Instance.Resolve<BookSectionService>().GetEntity(id);
                    BookSection BookSection = this._bookSectionService.Find(m => m.ID == id && !m.IsDeleted);
                    string playUrl = BookSection.Content;
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
                    //BookSection.Status = Domain.Base.StatusEnum.Deleted;
                    //Container.Instance.Resolve<BookSectionService>().Edit(BookSection);
                    BookSection.IsDeleted = true;
                    BookSection.DeletedAt = DateTime.Now;
                    this._bookSectionService.Update(BookSection);
                    // 相关学习记录删除
                    //IList<Learner_BookSection> learner_BookSections = Container.Instance.Resolve<Learner_BookSectionService>().Query(new List<ICriterion>
                    //{
                    //    Expression.Eq("BookSection.ID", BookSection.ID)
                    //});
                    //foreach (var item in learner_BookSections)
                    //{
                    //    item.Status = Domain.Base.StatusEnum.Deleted;
                    //    Container.Instance.Resolve<Learner_BookSectionService>().Edit(item);
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

        #region 编辑文库章节
        [HttpGet]
        public ViewResult EditSection(int id)
        {
            //BookSection viewModel = Container.Instance.Resolve<BookSectionService>().GetEntity(id);
            BookSection viewModel = this._bookSectionService.Find(m => m.ID == id && !m.IsDeleted);

            // 如果为本站上的文件，则会有标记，去除标记,转化为相对地址
            viewModel.Content = viewModel.Content.Replace(":WebUISite:", "");

            return View(viewModel);
        }

        [HttpPost]
        public JsonResult EditVideo(BookSection inputModel)
        {
            try
            {
                #region 输入数据处理

                // 判断是否包含 http
                // 包含 http，则为其它网络资源（可能为上传到阿里云oss的），否则为上传到本服务器的资源，可直接使用相对地址
                if (inputModel.Content.ToLower().Contains("http://") || inputModel.Content.ToLower().Contains("https://"))
                {
                    inputModel.Content = inputModel.Content;
                }
                else
                {
                    // 打上文件在本服务器的标记
                    inputModel.Content = ":WebUISite:" + inputModel.Content;
                }

                #endregion

                //BookSection dbModel = Container.Instance.Resolve<BookSectionService>().GetEntity(inputModel.ID);
                BookSection dbModel = this._bookSectionService.Find(m => m.ID == inputModel.ID && !m.IsDeleted);
                dbModel.Title = inputModel.Title;
                dbModel.Content = inputModel.Content;
                dbModel.SortCode = inputModel.SortCode;
                dbModel.Duration = inputModel.Duration;


                //Container.Instance.Resolve<BookSectionService>().Edit(dbModel);
                this._bookSectionService.Update(dbModel);

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

        #region 导入文库-解析equb等文档内容，转换为文库以及文库章节
        /// <summary>
        /// 上传视频-为某课程上传视频
        /// </summary>
        /// <param name="id">课程ID</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ImportBook(int id)
        {
            try
            {
                // 该课程存在吗？
                if (this._bookInfoService.Contains(m => m.ID == id && !m.IsDeleted))
                {
                    int BookInfoCreatorId = this._bookInfoService.Find(m => m.ID == id && !m.IsDeleted).CreatorId;

                    // 保存到此课程的文件夹
                    string basePath = $"~/Upload/temp/BookInfo/{id}/";

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
                        url = $"/Upload/videos/BookInfo/{id}/" + saveFileName
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