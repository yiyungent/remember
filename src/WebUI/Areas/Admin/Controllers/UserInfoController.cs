using Core;
using Domain;
using Framework.Factories;
using Framework.HtmlHelpers;
using Framework.Infrastructure.Abstract;
using Framework.Infrastructure.Concrete;
using Framework.Models;
using Framework.Mvc;
using NHibernate.Criterion;
using Service;
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

        #region Ctor
        public UserInfoController()
        {
            ViewBag.PageHeader = "用户管理";
            ViewBag.PageHeaderDescription = "用户管理";
            ViewBag.BreadcrumbList = new List<BreadcrumbItem>
            {
                new BreadcrumbItem("业务管理"),
            };
        }
        #endregion

        #region 列表
        public ViewResult Index(int pageIndex = 1, int pageSize = 6)
        {
            IList<ICriterion> queryConditions = new List<ICriterion>();
            Query(queryConditions);

            ListViewModel<UserInfo> viewModel = new ListViewModel<UserInfo>(queryConditions, pageIndex: pageIndex, pageSize: pageSize);
            TempData["RedirectUrl"] = Request.RawUrl;

            return View(viewModel);
        }

        private void Query(IList<ICriterion> queryConditions)
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
                    queryConditions.Add(Expression.Like("UserName", query, MatchMode.Anywhere));
                    break;
                case "name":
                    queryType.Text = "展示名";
                    queryConditions.Add(Expression.Like("Name", query, MatchMode.Anywhere));
                    break;
                case "id":
                    queryType.Text = "ID";
                    queryConditions.Add(Expression.Eq("ID", int.Parse(query)));
                    break;
                default:
                    queryType.Text = "用户名";
                    queryConditions.Add(Expression.Like("UserName", query, MatchMode.Anywhere));
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
                Container.Instance.Resolve<UserInfoService>().Delete(id);

                return Json(new { code = 1, message = "删除成功" });
            }
            catch (Exception ex)
            {
                return Json(new { code = 1, message = "删除失败" });
            }
        }
        #endregion

        #region 查看
        public ViewResult Detail(int id)
        {
            UserInfo viewModel = Container.Instance.Resolve<UserInfoService>().GetEntity(id);

            return View(viewModel);
        }
        #endregion

        #region 编辑
        [HttpGet]
        public ViewResult Edit(int id)
        {
            UserInfo dbModel = Container.Instance.Resolve<UserInfoService>().GetEntity(id);
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

                    Container.Instance.Resolve<UserInfoService>().Edit(dbModel);

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
                    if (Container.Instance.Resolve<UserInfoService>().Exist(inputModel.InputUserName?.Trim()))
                    {
                        return Json(new { code = -3, message = "用户名已存在，请使用其他用户名" });
                    }
                    // 查找 已经绑定此邮箱的 (非本正编辑) 的用户
                    if (!string.IsNullOrEmpty(inputModel.InputEmail))
                    {
                        bool isExist = Container.Instance.Resolve<UserInfoService>().Count(Expression.Eq("Email", inputModel.InputEmail?.Trim())) > 0;
                        if (isExist)
                        {
                            return Json(new { code = -3, message = "邮箱已经被其他用户绑定，请绑定其它邮箱" });
                        }
                    }
                    #endregion

                    UserInfo dbModel = (UserInfo)inputModel;

                    Container.Instance.Resolve<UserInfoService>().Create(dbModel);

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
            IList<ICriterion> criteria = null;
            if (exceptUserId == 0)
            {
                criteria = new List<ICriterion>
                {
                    Expression.Eq("Email", email)
                };
            }
            else
            {
                criteria = new List<ICriterion>
                {
                     Expression.And(
                        Expression.Eq("Email", email),
                        Expression.Not(Expression.Eq("ID", exceptUserId))
                     )
                };
            }
            UserInfo exist = Container.Instance.Resolve<UserInfoService>().Query(criteria).FirstOrDefault();
            if (exist != null)
            {
                isExist = true;
            }

            return isExist;
        }
        #endregion

        #endregion

    }

}