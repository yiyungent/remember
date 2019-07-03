using Castle.ActiveRecord;
using Domain.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public partial class RoleInfo
    {
        /// <summary>
        /// 角色名
        /// </summary>
        [Display(Name = "角色名")]
        [Property(Length = 30, NotNull = true)]
        public string Name { get; set; }

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
        /// 用户列表
        ///     多对多关系
        /// </summary>
        [Display(Name = "用户列表")]
        [HasAndBelongsToMany(Table = "Role_User", ColumnKey = "RoleId", ColumnRef = "UserId"/*, Lazy = true*/)]
        public IList<UserInfo> UserInfoList { get; set; }

        /// <summary>
        /// 角色菜单权限
        ///     多对多关系
        /// </summary>
        [Display(Name = "角色菜单权限")]
        [HasAndBelongsToMany(Table = "Role_Menu", ColumnKey = "RoleId", ColumnRef = "MenuId"/*, Lazy = true*/)]
        public IList<Sys_Menu> Sys_MenuList { get; set; }

        /// <summary>
        /// 角色操作权限
        ///     多对多关系
        /// </summary>
        [Display(Name = "角色操作权限")]
        [HasAndBelongsToMany(Table = "Role_Function", ColumnKey = "RoleId", ColumnRef = "FunctionId"/*, Lazy = true*/)]
        public IList<FunctionInfo> FunctionInfoList { get; set; }

        #endregion
    }
}
