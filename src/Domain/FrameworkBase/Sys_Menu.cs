using Castle.ActiveRecord;
using Domain.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public partial class Sys_Menu
    {
        /// <summary>
        /// 菜单名
        /// </summary>
        [Display(Name = "名称")]
        [Property(Length = 100, NotNull = true)]
        public string Name { get; set; }

        /// <summary>
        /// 控制器名
        /// </summary>
        [Display(Name = "控制器")]
        [Property(Length = 100)]
        public string ControllerName { get; set; }

        /// <summary>
        /// 动作名
        /// </summary>
        [Display(Name = "动作")]
        [Property(Length = 100)]
        public string ActionName { get; set; }

        /// <summary>
        /// 区域名
        /// </summary>
        [Display(Name = "区域")]
        [Property(Length = 100)]
        public string AreaName { get; set; }

        /// <summary>
        /// 排序码
        /// </summary>
        [Display(Name = "排序码")]
        [Property]
        public int SortCode { get; set; }

        #region Relationship

        /// <summary>
        /// 上级菜单
        ///     多对一关系
        /// </summary>
        [Display(Name = "上级菜单")]
        [BelongsTo(Column = "ParentId")]
        public Sys_Menu ParentMenu { get; set; }

        /// <summary>
        /// 子菜单列表
        ///     一对多关系
        /// </summary>
        [Display(Name = "子菜单列表")]
        [HasMany(ColumnKey = "ParentId")]
        public IList<Sys_Menu> Children { get; set; }

        ///// <summary>
        ///// 操作列表
        /////     一对多关系
        ///// </summary>
        //[Display(Name = "操作列表")]
        //[HasMany(ColumnKey = "MenuId")]
        //public IList<FunctionInfo> FunctionInfoList { get; set; } 

        #endregion
    }
}
