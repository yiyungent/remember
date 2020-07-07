using Core;
using Domain;
using Domain.Entities;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Areas.Admin.Models;
using WebUI.Areas.Admin.Models.Common;

namespace WebUI.Areas.Admin.Controllers
{
    public class CatInfoController : Controller
    {
        #region Fields
        private readonly ICatInfoService _catInfoService;
        #endregion

        #region Ctor
        public CatInfoController(ICatInfoService catInfoService)
        {
            this._catInfoService = catInfoService;
        }
        #endregion

        #region 列表
        public ActionResult Index()
        {
            return View();
        }
        #endregion

        #region 编辑
        [HttpGet]
        public ViewResult Edit(int id)
        {
            CatInfo viewModel = this._catInfoService.Find(m => m.ID == id);
            int parentId = viewModel.ParentId ?? 0;
            ViewBag.DDLParent = InitDDLForParent(viewModel, parentId);

            return View(viewModel);
        }

        [HttpPost]
        public JsonResult Edit(CatInfo inputModel)
        {
            try
            {
                CatInfo dbModel = this._catInfoService.Find(m => m.ID == inputModel.ID);
                // 上级
                if (inputModel.ParentId == null || inputModel.ParentId == 0)
                {
                    inputModel.ParentId = null;
                }
                // 设置修改后的值
                dbModel.Name = inputModel.Name;
                dbModel.SortCode = inputModel.SortCode;
                dbModel.ParentId = inputModel.ParentId;
                this._catInfoService.Update(dbModel);

                return Json(new { code = 1, message = "保存成功" });
            }
            catch (Exception ex)
            {
                return Json(new { code = -1, message = "保存失败" });
            }
        }
        #endregion

        #region 添加
        //[HttpGet]
        //public ViewResult Create()
        //{
        //    UserInfoViewModel viewModel = new UserInfoViewModel();

        //    return View(viewModel);
        //}

        //[HttpPost]
        //public JsonResult Create(UserInfoViewModel inputModel)
        //{
        //    try
        //    {
        //        // 数据格式效验
        //        if (ModelState.IsValid)
        //        {
        //            int currentUserId = AccountManager.GetCurrentAccount().UserId;

        //            #region 数据有效效验
        //            if (string.IsNullOrEmpty(inputModel.InputPassword?.Trim()))
        //            {
        //                return Json(new { code = -2, message = "请填写初始密码" });
        //            }
        //            // 查找 已经有此用户名的用户
        //            string inputUserName = inputModel.InputUserName?.Trim();
        //            if (this._userInfoService.Contains(m => m.UserName == inputUserName && !m.IsDeleted))
        //            {
        //                return Json(new { code = -3, message = "用户名已存在，请使用其他用户名" });
        //            }
        //            // 查找 已经绑定此邮箱的 (非本正编辑) 的用户
        //            if (!string.IsNullOrEmpty(inputModel.InputEmail))
        //            {
        //                //bool isExist = Container.Instance.Resolve<UserInfoService>().Count(Expression.Eq("Email", inputModel.InputEmail?.Trim())) > 0;
        //                string inputEmail = inputModel.InputEmail?.Trim();
        //                bool isExist = this._userInfoService.Contains(m => m.Email == inputEmail && !m.IsDeleted);
        //                if (isExist)
        //                {
        //                    return Json(new { code = -3, message = "邮箱已经被其他用户绑定，请绑定其它邮箱" });
        //                }
        //            }
        //            #endregion

        //            // 输入模型 - > 数据库模型
        //            UserInfo dbModel = new UserInfo();
        //            dbModel.Password = EncryptHelper.MD5Encrypt32(inputModel.InputPassword);
        //            dbModel.UserName = inputModel.InputUserName?.Trim();
        //            //dbModel.Avatar = inputModel.InputAvatar?.Trim();
        //            dbModel.Email = inputModel.InputEmail?.Trim();
        //            dbModel.Description = inputModel.InputDescription?.Trim();
        //            dbModel.CreateTime = DateTime.Now;

        //            // 自动生成头像
        //            Identicon
        //            .FromValue(EncryptHelper.MD5Encrypt32(dbModel.UserName), size: 100)
        //            .SaveAsPng(Server.MapPath("/upload/images/avatars/" + dbModel.UserName + ".png"));
        //            dbModel.Avatar = ":WebUISite:/upload/images/avatars/" + dbModel.UserName + ".png";
        //            this._userInfoService.Create(dbModel);
        //            // 注意：添加 用户（如果要支持在添加的时候选择角色），则一定要先创建用户，因为 Role_Users 需要用户外键

        //            #region 角色选项
        //            if (inputModel.RoleOptions != null)
        //            {
        //                IList<int> roleIdList = new List<int>();
        //                foreach (OptionModel option in inputModel.RoleOptions)
        //                {
        //                    roleIdList.Add(option.ID);
        //                }
        //                this._role_UserService.UserAssignRoles(dbModel.ID, roleIdList, currentUserId);
        //            }
        //            else
        //            {
        //                // 删除此用户的所有角色
        //                this._role_UserService.UserAssignRoles(dbModel.ID, new List<int>(), currentUserId);
        //            }
        //            #endregion

        //            return Json(new { code = 1, message = "添加成功" });
        //        }
        //        else
        //        {
        //            string errorMessage = ModelState.GetErrorMessage();
        //            return Json(new { code = -1, message = errorMessage });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { code = -2, message = "添加失败" });
        //    }
        //}
        #endregion

        #region 删除
        [HttpPost]
        public JsonResult Delete(int id)
        {
            try
            {
                var dbModel = this._catInfoService.Find(m => m.ID == id && !m.IsDeleted);
                if (dbModel != null)
                {
                    dbModel.IsDeleted = true;
                    dbModel.DeletedAt = DateTime.Now;
                    this._catInfoService.Update(dbModel);

                    return Json(new { code = 1, message = "删除成功" });
                }
                else
                {
                    return Json(new { code = 1, message = $"删除失败,不存在id为{id}的分区" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = 1, message = "删除失败" });
            }
        }
        #endregion

        #region Helpers
        private IList<SelectListItem> InitDDLForParent(CatInfo self, int parentId)
        {
            IList<SelectListItem> ret = new List<SelectListItem>();
            ret.Add(new SelectListItem()
            {
                Text = "请选择",
                Value = "0",
                Selected = (0 == parentId),
            });

            IList<CatInfo> all = this._catInfoService.All().ToList();
            // 找出自己及后代的ID
            IList<int> idRange = new List<int>();
            GetIdRange(self, idRange, all);

            // not in 查询
            var find = from m in all
                       where idRange.Contains(m.ID) == false
                       select m;
            // 方案一：不考虑层级的实现
            //foreach (var item in find)
            //{
            //    ret.Add(new SelectListItem()
            //    {
            //        Text = item.Name,
            //        Value = item.ID.ToString(),
            //        Selected = (item.ID == parentId)
            //    });
            //}
            // 方案二：考虑层级的实现
            // 1.一级菜单
            var first = from m in find
                        where m.ParentId == null || m.ParentId == 0
                        orderby m.SortCode
                        select m;
            foreach (var item in first)
            {
                AddSelfAndChildrenForDDL("", item, ret, find.ToList(), parentId);
            }

            return ret;
        }

        private void AddSelfAndChildrenForDDL(string pref, CatInfo self, IList<SelectListItem> target, IList<CatInfo> all, int parentId)
        {
            // 1.添加自己
            target.Add(new SelectListItem()
            {
                Text = pref + self.Name,
                Value = self.ID.ToString(),
                Selected = (self.ID == parentId)
            });
            // 2.递归添加子女
            var child = from m in all
                            //where m.ParentMenu != null && m.ParentMenu.ID == self.ID
                        where m.ParentId != null && m.ParentId != 0 && m.Parent.ID == self.ID
                        orderby m.SortCode
                        select m;
            foreach (var item in child)
            {
                AddSelfAndChildrenForDDL(pref + "--", item, target, all, parentId);
            }
        }

        /// <summary>
        /// 递归方法：添加自己及其子女
        /// </summary>
        /// <param name="self"></param>
        /// <param name="idRange"></param>
        /// <param name="all"></param>
        private void GetIdRange(CatInfo self, IList<int> idRange, IList<CatInfo> all)
        {
            // 添加自己
            idRange.Add(self.ID);

            // 关于子女循环
            // 第二种解决方法：
            if (self.Children == null) return;
            foreach (var item in self.Children)
            {
                // 递归调用---添加自己和自己的子女
                GetIdRange(item, idRange, all);
            }
        }
        #endregion
    }
}