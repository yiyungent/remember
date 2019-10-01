namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RoleInfo : BaseEntity
    {
        public int ID { get; set; }

        /// <summary>
        /// 角色名
        /// </summary>
        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        public bool? IsLog { get; set; }

        [StringLength(255)]
        public string Remark { get; set; }


        #region Relationships

        /// <summary>
        /// 用户列表
        ///     多对多关系
        /// </summary>
        public virtual ICollection<UserInfo> UserInfos { get; set; }

        /// <summary>
        /// 角色菜单权限
        ///     多对多关系
        /// </summary>
        public virtual ICollection<Sys_Menu> Sys_Menus { get; set; }

        /// <summary>
        /// 角色操作权限
        ///     多对多关系
        /// </summary>
        public virtual ICollection<FunctionInfo> FunctionInfos { get; set; }

        #endregion
    }
}
