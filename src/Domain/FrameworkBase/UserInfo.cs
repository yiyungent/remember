using Castle.ActiveRecord;
using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public partial class UserInfo
    {
        /// <summary>
        /// 用户名(唯一，一经创建不可改，可作为登录使用)
        /// </summary>
        [Display(Name = "用户名")]
        [Property(Length = 30, NotNull = true, Unique = true)]
        [Required(ErrorMessage = "请输入账号")]
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Display(Name = "密码")]
        [Property(Length = 64, NotNull = true)]
        public string Password { get; set; }

        /// <summary>
        /// 刷新Toke
        /// </summary>
        [Property(Length = 255, NotNull = false)]
        public string RefreshToken { get; set; }

        /// <summary>
        /// 最后登录时间
        /// </summary>
        [Property(NotNull = true)]
        public DateTime LastLoginTime { get; set; }

        /// <summary>
        /// 选择的主体模板
        /// </summary>
        [Display(Name = "选择的主体模板")]
        [Property(Length = 20, NotNull = false)]
        public string TemplateName { get; set; }

        /// <summary>
        /// 状态
        ///     0: 正常
        ///     1: 禁用
        /// </summary>
        [Display(Name = "状态")]
        [Property]
        public int Status { get; set; }

        #region Relationship

        /// <summary>
        /// 担任角色列表
        ///     多对多关系
        /// </summary>
        [Display(Name = "担任角色列表")]
        [HasAndBelongsToMany(Table = "Role_User", ColumnKey = "UserId", ColumnRef = "RoleId")]
        public IList<RoleInfo> RoleInfoList { get; set; }

        #endregion

    }
}
