using Core;
using Domain;
using Domain.Entities;
using Framework.Common;
using Framework.Factories;
using Framework.HtmlHelpers;
using Framework.Infrastructure.Abstract;
using Framework.Infrastructure.Concrete;
using Framework.Models;
using Framework.Mvc;
using Jdenticon;
using Services.Interface;
using System;
using System.Collections.Generic;
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
        #endregion

        #region Ctor
        public UserInfoController(IUserInfoService userInfoService)
        {
            this._userInfoService = userInfoService;
        }
        #endregion

        #region 列表
        public ViewResult Index(int pageIndex = 1, int pageSize = 6)
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
                    //queryConditions.Add(Expression.Like("UserName", query, MatchMode.Anywhere));
                    list = this._userInfoService.Filter<int>(pageIndex, pageSize, out totalCount, m => m.UserName.Contains(query) && !m.IsDeleted, m => m.ID, false).ToList();
                    break;
                case "id":
                    queryType.Text = "ID";
                    list = this._userInfoService.Filter<int>(pageIndex, pageSize, out totalCount, m => m.ID == int.Parse(query) && !m.IsDeleted, m => m.ID, false).ToList();
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

                return Json(new { code = 1, message = "删除成功" });
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
            //UserInfo viewModel = Container.Instance.Resolve<UserInfoService>().GetEntity(id);
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

                    #region 数据有效效验

                    #region 绑定邮箱
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
                    UserInfo dbModel = (UserInfo)inputModel;
                    #region 角色选项
                    if (inputModel.RoleOptions != null)
                    {
                        IList<int> roleIdList = new List<int>();
                        foreach (OptionModel option in inputModel.RoleOptions)
                        {
                            roleIdList.Add(option.ID);
                        }
                        //IList<RoleInfo> selectedRole = Container.Instance.Resolve<RoleInfoService>().Query(new List<ICriterion>
                        //{
                        //    Expression.In("ID", roleIdList.ToArray())
                        //});
                        //dbModel.RoleInfoList = selectedRole;
                        IList<RoleInfo> selectedRole = ContainerManager.Resolve<IRoleInfoService>().Filter(m =>
                            roleIdList.Contains(m.ID)
                        ).ToList();
                        foreach (var item in selectedRole)
                        {
                            ContainerManager.Resolve<IRole_UserService>().Create(new Role_User
                            {
                                UserInfoId = inputModel.ID,
                                RoleInfoId = item.ID,
                                CreateTime = DateTime.Now
                            });
                        }
                    }
                    else
                    {
                        //dbModel.RoleInfoList = null;
                        // 删除此用户的所有角色
                        IList<Role_User> role_Users = ContainerManager.Resolve<IRole_UserService>().Filter(m => m.UserInfoId == inputModel.ID).ToList();
                        foreach (var item in role_Users)
                        {
                            item.IsDeleted = true;
                            item.DeletedAt = DateTime.Now;
                            ContainerManager.Resolve<IRole_UserService>().Update(item);
                        }
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

        #region 新增
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
                    #region 数据有效效验
                    if (string.IsNullOrEmpty(inputModel.InputPassword?.Trim()))
                    {
                        return Json(new { code = -2, message = "请填写初始密码" });
                    }
                    // 查找 已经有此用户名的用户
                    //if (Container.Instance.Resolve<UserInfoService>().Exist(inputModel.InputUserName?.Trim()))
                    string inputUserName = inputModel.InputUserName?.Trim();
                    if (this._userInfoService.Contains(m => m.UserName.Contains(inputUserName) && !m.IsDeleted))
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

                    UserInfo dbModel = (UserInfo)inputModel;
                    #region 角色选项
                    if (inputModel.RoleOptions != null)
                    {
                        IList<int> roleIdList = new List<int>();
                        foreach (OptionModel option in inputModel.RoleOptions)
                        {
                            roleIdList.Add(option.ID);
                        }
                        //IList<RoleInfo> selectedRole = Container.Instance.Resolve<RoleInfoService>().Query(new List<ICriterion>
                        //{
                        //    Expression.In("ID", roleIdList.ToArray())
                        //});
                        //dbModel.RoleInfoList = selectedRole;
                        IList<RoleInfo> selectedRole = ContainerManager.Resolve<IRoleInfoService>().Filter(m =>
                            roleIdList.Contains(m.ID)
                        ).ToList();
                        foreach (var item in selectedRole)
                        {
                            ContainerManager.Resolve<IRole_UserService>().Create(new Role_User
                            {
                                UserInfoId = inputModel.ID,
                                RoleInfoId = item.ID,
                                CreateTime = DateTime.Now
                            });
                        }
                    }
                    else
                    {
                        //dbModel.RoleInfoList = null;
                        // 删除此用户的所有角色
                        IList<Role_User> role_Users = ContainerManager.Resolve<IRole_UserService>().Filter(m => m.UserInfoId == inputModel.ID).ToList();
                        foreach (var item in role_Users)
                        {
                            item.IsDeleted = true;
                            item.DeletedAt = DateTime.Now;
                            ContainerManager.Resolve<IRole_UserService>().Update(item);
                        }
                    }
                    #endregion

                    // 自动生成头像
                    Identicon
                    .FromValue(EncryptHelper.MD5Encrypt32(dbModel.UserName), size: 100)
                    .SaveAsPng(Server.MapPath("/upload/images/avatars/" + dbModel.UserName + ".png"));

                    dbModel.Avatar = ":WebUISite:/upload/images/avatars/" + dbModel.UserName + ".png";

                    //Container.Instance.Resolve<UserInfoService>().Create(dbModel);
                    this._userInfoService.Create(dbModel);

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

        #region Helpers

        #region 检查邮箱是否已(被其它用户)绑定
        public bool IsExistEmail(string email, int exceptUserId = 0)
        {
            bool isExist = false;
            if (exceptUserId == 0)
            {
                //criteria = new List<ICriterion>
                //{
                //    Expression.Eq("Email", email)
                //};
                isExist = this._userInfoService.Contains(m => m.Email == email && !m.IsDeleted);
            }
            else
            {
                //criteria = new List<ICriterion>
                //{
                //     Expression.And(
                //        Expression.Eq("Email", email),
                //        Expression.Not(Expression.Eq("ID", exceptUserId))
                //     )
                //};
                isExist = this._userInfoService.Contains(m =>
                    m.Email == email
                    && m.ID == exceptUserId
                    && !m.IsDeleted
                );
            }

            return isExist;
        }
        #endregion

        #endregion

    }

}