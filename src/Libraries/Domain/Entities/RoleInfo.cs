namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// 实体类: 角色
    /// </summary>
    public partial class RoleInfo : BaseEntity
    {
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// 角色名
        /// </summary>
        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        [Column(TypeName = "text")]
        [StringLength(30)]
        public string Remark { get; set; }


        #region Relationships

        /// <summary>
        /// 角色-用户
        /// </summary>
        public virtual ICollection<Role_User> Role_Users { get; set; }

        /// <summary>
        /// 角色-菜单
        /// </summary>
        public virtual ICollection<Role_Menu> Role_Menus { get; set; }

        /// <summary>
        /// 角色-权限
        /// </summary>
        public virtual ICollection<Role_Function> Role_Functions { get; set; }

        #endregion
    }
}
