using Castle.ActiveRecord;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Remember.Domain
{
    [ActiveRecord]
    public class SysMenu : BaseEntity<SysMenu>
    {
        [Display(Name = "名称")]
        [Property(Length = 100, NotNull = true)]
        public string Name { get; set; }

        [Display(Name = "类名")]
        [Property(Length = 100)]
        public string ClassName { get; set; }

        [Display(Name = "控制器")]
        [Property(Length = 100)]
        public string ControllerName { get; set; }

        [Display(Name = "动作")]
        [Property(Length = 100)]
        public string ActionName { get; set; }

        [Display(Name = "排序码")]
        [Property(Length = 100)]
        public int SortCode { get; set; }

        /// <summary>
        /// 上级菜单
        ///     多对一：只对应一个上级菜单
        /// </summary>
        [Display(Name = "上级菜单")]
        // BelongsTo 属于------多对一，多个属于一个下
        [BelongsTo(Column = "ParentId")]
        public SysMenu ParentMenu { get; set; }

        /// <summary>
        /// 下级菜单
        ///     一对多：一个菜单项下有很多子菜单项
        /// </summary>
        [Display(Name = "下级菜单 ")]
        // 一对多，一个下有很多-----HasMany
        // 注意是  ColumnKey    其设置为 当前类对应表需增加的字段，就算不是一个表自关联，而是两个表的一对多，这正是 Key的含义
        [HasMany(ColumnKey = "ParentId")]
        public IList<SysMenu> Children { get; set; }

        /// <summary>
        /// 操作列表
        ///     一对多关系
        /// </summary>
        [Display(Name = "操作列表")]
        [HasMany(ColumnKey = "MenuId")]
        public IList<SysFunction> SysFunctionList { get; set; }
    }
}
