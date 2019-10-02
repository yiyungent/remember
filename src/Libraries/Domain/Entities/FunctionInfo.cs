namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class FunctionInfo : BaseEntity
    {
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// 权限键（唯一标识）
        /// </summary>
        [Required]
        [StringLength(50)]
        public string AuthKey { get; set; }

        /// <summary>
        /// 操作名称
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Column(TypeName = "text")]
        [StringLength(50)]
        public string Remark { get; set; }

        #region Relationships

        /// <summary>
        /// 系统菜单
        ///     多对一关系
        /// </summary>
        [ForeignKey("Sys_Menu")]
        public int? MenuId { get; set; }
        [ForeignKey("MenuId")]
        public virtual Sys_Menu Sys_Menu { get; set; }

        /// <summary>
        /// 角色-权限
        /// </summary>
        public virtual ICollection<Role_Function> Role_Functions { get; set; }

        #endregion

        #region Helper

        /// <summary>
        /// 根据 AuthKey 获取 AreaName, ControllerName, ActionName
        /// </summary>
        public AreaCAItem AreaCAItem
        {
            get
            {
                string[] arr = this.AuthKey.Trim().Split(new char[] { '.' }, 3);
                string areaName = "", controllerName = "", actionName = "";
                if (this.AuthKey.Trim().StartsWith("."))
                {
                    areaName = "";
                    if (arr.Length == 3)
                    {
                        controllerName = arr[1];
                        actionName = arr[2];
                    }
                    else
                    {
                        controllerName = arr[0];
                        actionName = arr[1];
                    }
                }
                else
                {
                    areaName = arr[0];
                    controllerName = arr[1];
                    actionName = arr[2];
                }

                return new AreaCAItem
                {
                    AreaName = areaName.Trim(),
                    ControllerName = controllerName.Trim(),
                    ActionName = actionName.Trim()
                };
            }
        }

        #endregion
    }
}
