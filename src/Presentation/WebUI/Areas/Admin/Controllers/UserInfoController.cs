using Core;
using Domain;
using Domain.Entities;
using Framework.Common;
using Framework.Infrastructure.Concrete;
using Framework.Models;
using Framework.Mvc;
using Jdenticon;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Areas.Admin.Models;
using WebUI.Areas.Admin.Models.Common;
using WebUI.Areas.Admin.Models.UserInfoVM;
using WebUI.Extensions;

namespace WebUI.Areas.Admin.Controllers
{
    public class UserInfoController : Controller
    {
        #region Fields
        private readonly IUserInfoService _userInfoService;
        private readonly IRoleInfoService _roleInfoService;
        private readonly IRole_UserService _role_UserService;
        #endregion

        #region Ctor
        public UserInfoController(IUserInfoService userInfoService, IRoleInfoService roleInfoService, IRole_UserService role_UserService)
        {
            this._userInfoService = userInfoService;
            this._roleInfoService = roleInfoService;
            this._role_UserService = role_UserService;
        }
        #endregion

        #region 列表
        public ViewResult Index(string cat = "all", int pageIndex = 1, int pageSize = 6)
        {
            Query(pageIndex, pageSize, out IList<UserInfo> list, out int totalCount);

            ListViewModel<UserInfo> viewModel = new ListViewModel<UserInfo>(list, pageIndex: pageIndex, pageSize: pageSize, totalCount: totalCount);
            TempData["RedirectUrl"] = Request.RawUrl;

            return View(viewModel);
        }

        private void Query(int pageIndex, int pageSize, out IList<UserInfo> list, out int totalCount)
        {
            // 输入的查询关键词
            string query = Request["q"]?.Trim() ?? "";
            // 查询类型
            QueryType queryType = new QueryType();
            queryType.Val = Request["type"]?.Trim() ?? "username";
            switch (queryType.Val.ToLower())
            {
                case "username":
                    queryType.Text = "用户名";
                    list = this._userInfoService.Filter<int>(pageIndex, pageSize, out totalCount, m => m.UserName.Contains(query) && !m.IsDeleted, m => m.ID, false).ToList();
                    break;
                case "id":
                    queryType.Text = "ID";
                    if (int.TryParse(query, out int userId))
                    {
                        list = this._userInfoService.Filter<int>(pageIndex, pageSize, out totalCount, m => m.ID == userId && !m.IsDeleted, m => m.ID, false).ToList();
                    }
                    else if (string.IsNullOrEmpty(query))
                    {
                        list = this._userInfoService.Filter<int>(pageIndex, pageSize, out totalCount, m => !m.IsDeleted, m => m.ID, false).ToList();
                    }
                    else
                    {
                        list = new List<UserInfo>();
                        totalCount = 0;
                    }
                    break;
                default:
                    queryType.Text = "用户名";
                    list = this._userInfoService.Filter<int>(pageIndex, pageSize, out totalCount, m => m.UserName.Contains(query) && !m.IsDeleted, m => m.ID, false).ToList();
                    break;
            }
            ViewBag.Query = query;
            ViewBag.QueryType = queryType;
        }
        #endregion

        #region 删除
        public JsonResult Delete(int id)
        {
            try
            {
                var dbModel = this._userInfoService.Find(m => m.ID == id && !m.IsDeleted);
                dbModel.IsDeleted = true;
                dbModel.DeletedAt = DateTime.Now;
                this._userInfoService.Update(dbModel);

                return Json(new { code = 1, message = $"删除 {dbModel.UserName} 成功" });
            }
            catch (Exception ex)
            {
                return Json(new { code = 1, message = "删除失败" });
            }
        }
        #endregion

        #region 批量删除
        public JsonResult BatchDelete(string ids)
        {
            try
            {
                int count = this._userInfoService.BatchDelete(ids);

                return Json(new { code = 1, message = $"成功删除 {count} 名用户" });
            }
            catch (Exception ex)
            {
                return Json(new { code = 1, message = "删除失败" });
            }
        }
        #endregion

        #region 查看
        public ViewResult Details(int id)
        {
            UserInfo viewModel = this._userInfoService.Find(m => m.ID == id && !m.IsDeleted);

            return View(viewModel);
        }
        #endregion

        #region 编辑
        [HttpGet]
        public ViewResult Edit(int id)
        {
            //UserInfo dbModel = Container.Instance.Resolve<UserInfoService>().GetEntity(id);
            UserInfo dbModel = this._userInfoService.Find(m => m.ID == id && !m.IsDeleted);
            UserInfoViewModel viewModel = (UserInfoViewModel)dbModel;

            return View(viewModel);
        }

        [HttpPost]
        public JsonResult Edit(UserInfoViewModel inputModel)
        {
            try
            {
                // 数据格式效验
                if (ModelState.IsValid)
                {
                    int currentUserId = AccountManager.GetCurrentAccount().UserId;

                    #region 数据有效效验

                    #region 绑定邮箱
                    // 查找 已经有此用户名的用户
                    string inputUserName = inputModel.InputUserName?.Trim();
                    if (this._userInfoService.Contains(m => m.UserName == inputUserName && m.ID != inputModel.ID && !m.IsDeleted))
                    {
                        return Json(new { code = -3, message = "用户名已存在，请使用其他用户名" });
                    }
                    // 查找 已经绑定此邮箱的 (非本正编辑) 的用户
                    if (!string.IsNullOrEmpty(inputModel.InputEmail))
                    {
                        if (IsExistEmail(inputModel.InputEmail, inputModel.ID))
                        {
                            return Json(new { code = -3, message = "邮箱已经被其他用户绑定，请绑定其他邮箱" });
                        }
                    }
                    #endregion

                    #endregion

                    // 输入模型->数据库模型
                    UserInfo dbModel = this._userInfoService.Find(m => m.ID == inputModel.ID && !m.IsDeleted);
                    if (!string.IsNullOrEmpty(inputModel.InputPassword))
                    {
                        dbModel.Password = EncryptHelper.MD5Encrypt32(inputModel.InputPassword);
                    }
                    dbModel.UserName = inputModel.InputUserName?.Trim();
                    //dbModel.Avatar = inputModel.InputAvatar?.Trim();
                    dbModel.Email = inputModel.InputEmail?.Trim();
                    dbModel.Description = inputModel.InputDescription?.Trim();

                    #region 角色选项
                    if (inputModel.RoleOptions != null)
                    {
                        IList<int> roleIdList = new List<int>();
                        foreach (OptionModel option in inputModel.RoleOptions)
                        {
                            roleIdList.Add(option.ID);
                        }
                        this._role_UserService.UserAssignRoles(inputModel.ID, roleIdList, currentUserId);
                    }
                    else
                    {
                        // 删除此用户的所有角色
                        this._role_UserService.UserAssignRoles(inputModel.ID, new List<int>(), currentUserId);
                    }
                    #endregion
                    //Container.Instance.Resolve<UserInfoService>().Edit(dbModel);
                    this._userInfoService.Update(dbModel);


                    return Json(new { code = 1, message = "保存成功" });
                }
                else
                {
                    string errorMessage = ModelState.GetErrorMessage();
                    return Json(new { code = -1, message = errorMessage });
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = -2, message = "保存失败" });
            }
        }
        #endregion

        #region 添加
        [HttpGet]
        public ViewResult Create()
        {
            UserInfoViewModel viewModel = new UserInfoViewModel();

            return View(viewModel);
        }

        [HttpPost]
        public JsonResult Create(UserInfoViewModel inputModel)
        {
            try
            {
                // 数据格式效验
                if (ModelState.IsValid)
                {
                    int currentUserId = AccountManager.GetCurrentAccount().UserId;

                    #region 数据有效效验
                    if (string.IsNullOrEmpty(inputModel.InputPassword?.Trim()))
                    {
                        return Json(new { code = -2, message = "请填写初始密码" });
                    }
                    // 查找 已经有此用户名的用户
                    string inputUserName = inputModel.InputUserName?.Trim();
                    if (this._userInfoService.Contains(m => m.UserName == inputUserName && !m.IsDeleted))
                    {
                        return Json(new { code = -3, message = "用户名已存在，请使用其他用户名" });
                    }
                    // 查找 已经绑定此邮箱的 (非本正编辑) 的用户
                    if (!string.IsNullOrEmpty(inputModel.InputEmail))
                    {
                        //bool isExist = Container.Instance.Resolve<UserInfoService>().Count(Expression.Eq("Email", inputModel.InputEmail?.Trim())) > 0;
                        string inputEmail = inputModel.InputEmail?.Trim();
                        bool isExist = this._userInfoService.Contains(m => m.Email == inputEmail && !m.IsDeleted);
                        if (isExist)
                        {
                            return Json(new { code = -3, message = "邮箱已经被其他用户绑定，请绑定其它邮箱" });
                        }
                    }
                    #endregion

                    // 输入模型 - > 数据库模型
                    UserInfo dbModel = new UserInfo();
                    dbModel.Password = EncryptHelper.MD5Encrypt32(inputModel.InputPassword);
                    dbModel.UserName = inputModel.InputUserName?.Trim();
                    //dbModel.Avatar = inputModel.InputAvatar?.Trim();
                    dbModel.Email = inputModel.InputEmail?.Trim();
                    dbModel.Description = inputModel.InputDescription?.Trim();
                    dbModel.CreateTime = DateTime.Now;

                    // 自动生成头像
                    Identicon
                    .FromValue(EncryptHelper.MD5Encrypt32(dbModel.UserName), size: 100)
                    .SaveAsPng(Server.MapPath("/upload/images/avatars/" + dbModel.UserName + ".png"));
                    dbModel.Avatar = ":WebUISite:/upload/images/avatars/" + dbModel.UserName + ".png";
                    this._userInfoService.Create(dbModel);
                    // 注意：添加 用户（如果要支持在添加的时候选择角色），则一定要先创建用户，因为 Role_Users 需要用户外键

                    #region 角色选项
                    if (inputModel.RoleOptions != null)
                    {
                        IList<int> roleIdList = new List<int>();
                        foreach (OptionModel option in inputModel.RoleOptions)
                        {
                            roleIdList.Add(option.ID);
                        }
                        this._role_UserService.UserAssignRoles(dbModel.ID, roleIdList, currentUserId);
                    }
                    else
                    {
                        // 删除此用户的所有角色
                        this._role_UserService.UserAssignRoles(dbModel.ID, new List<int>(), currentUserId);
                    }
                    #endregion

                    return Json(new { code = 1, message = "添加成功" });
                }
                else
                {
                    string errorMessage = ModelState.GetErrorMessage();
                    return Json(new { code = -1, message = errorMessage });
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = -2, message = "添加失败" });
            }
        }
        #endregion

        #region 用户画像
        [HttpGet]
        public ViewResult FaceStat(int id)
        {
            //IList<User_BookInfo> user_BookInfos = ContainerManager.Resolve<IUser_BookInfoService>().Filter(1, 10, out int totalCount_CourseBox, m => m.ReaderId == id && !m.IsDeleted, m => m.CreateTime, false).ToList();

            //IList<User_BookSection> user_BookSections = ContainerManager.Resolve<IUser_BookSectionService>().Filter(1, 10, out int totalCount_VideoInfo, m => m.ReaderId == id && !m.IsDeleted, m => m.LastViewAt, false).ToList();

            //IList<User_BookInfoViewModel> user_BookInfoViewModels = new List<User_BookInfoViewModel>();
            //foreach (var item in user_BookInfos)
            //{
            //    User_BookInfoViewModel viewModelItem = new User_BookInfoViewModel
            //    {
            //        ID = item.ID,
            //        JoinTime = item.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"),
            //        CourseBox = new User_BookInfoViewModel.CourseBoxModel
            //        {
            //            Name = item.BookInfo.Name
            //        },
            //        LastPlayVideoInfo = new User_BookInfoViewModel.VideoInfoModel
            //        {
            //            Page = item.LastViewSection.SortCode,
            //            Title = item.LastViewSection.Title
            //        },
            //        Score = 0
            //    };
            //    var relate_Learner_VideoInfos = user_BookSections.Where(m => m.BookSection.BookInfoId == item.BookInfoId);
            //    long totalLen = relate_Learner_VideoInfos.Select(m => m.BookSection).Select(m => m.Duration).Sum();
            //    long playLen = relate_Learner_VideoInfos.Select(m => m.ProgressAt).Sum();

            //    if (totalLen > 0)
            //    {
            //        viewModelItem.Score = (int)(playLen / totalLen) * 100;
            //    }

            //    user_BookInfoViewModels.Add(viewModelItem);
            //}


            //ViewBag.Learner_CourseBoxes = user_BookInfoViewModels;
            //ViewBag.Learner_VideoInfos = user_BookSections;

            return View();
        }
        #endregion

        #region Helpers

        #region 检查邮箱是否已(被其它用户)绑定
        public bool IsExistEmail(string email, int exceptUserId = 0)
        {
            bool isExist = false;
            if (exceptUserId == 0)
            {
                isExist = this._userInfoService.Contains(m => m.Email == email && !m.IsDeleted);
            }
            else
            {
                isExist = this._userInfoService.Contains(m =>
                    m.Email == email
                    && m.ID != exceptUserId
                    && !m.IsDeleted
                );
            }

            return isExist;
        }
        #endregion

        #endregion

    }

}