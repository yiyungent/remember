using Core;
using Domain;
using Domain.Entities;
using Framework.Common;
using Framework.Infrastructure.Concrete;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Areas.Admin.Models.Common;
using WebUI.Extensions;

namespace WebUI.Areas.Admin.Models.UserInfoVM
{
    public class UserInfoViewModel
    {
        #region Properties

        /// <summary>
        /// UID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [Display(Name = "用户名")]
        [Required(ErrorMessage = "用户名不能为空")]
        public string InputUserName { get; set; }

        /// <summary>
        /// 用户头像Url地址
        /// </summary>
        public string InputAvatar { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [Display(Name = "邮箱")]
        public string InputEmail { get; set; }

        [Display(Name = "描述")]
        public string InputDescription { get; set; }

        [Display(Name = "密码")]
        public string InputPassword { get; set; }

        [Display(Name = "角色")]
        public List<OptionModel> RoleOptions { get; set; }

        #endregion

        #region Ctor
        public UserInfoViewModel()
        {
            this.RoleOptions = new List<OptionModel>();
            //this.RoleOptions.Add(new OptionModel
            //{
            //    ID = 0,
            //    IsSelected = true,
            //    Text = "请选择角色"
            //});
            //IList<RoleInfo> allRole = Container.Instance.Resolve<RoleInfoService>().GetAll();
            IList<RoleInfo> allRole = ContainerManager.Resolve<IRoleInfoService>().All().ToList();
            allRole = allRole.Where(m => m.Name != "游客").ToList();
            foreach (RoleInfo item in allRole)
            {
                this.RoleOptions.Add(new OptionModel
                {
                    ID = item.ID,
                    IsSelected = false,
                    Text = item.Name
                });
            }
        }
        #endregion

        #region 数据库模型->视图模型
        public static explicit operator UserInfoViewModel(UserInfo dbModel)
        {
            IList<RoleInfo> allRole = ContainerManager.Resolve<IRoleInfoService>().All().ToList();
            allRole = allRole.Where(m => m.Name != "游客").ToList();

            List<OptionModel> roleOptions = new List<OptionModel>();
            foreach (RoleInfo role in allRole)
            {
                roleOptions.Add(new OptionModel
                {
                    ID = role.ID,
                    Text = role.Name,
                    IsSelected = dbModel.RoleInfos.Contains(role, new RoleInfoEqualityComparer())
                });
            }
            UserInfoViewModel viewModel = new UserInfoViewModel
            {
                ID = dbModel.ID,
                InputUserName = dbModel.UserName,
                InputDescription = dbModel.Description,
                InputAvatar = dbModel.Avatar,
                InputEmail = dbModel.Email,
                RoleOptions = roleOptions,
            };

            return viewModel;
        }
        #endregion

    }
}