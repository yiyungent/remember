using Castle.ActiveRecord;
using System.ComponentModel.DataAnnotations;

namespace Remember.Domain
{
    [ActiveRecord]
    public class SysMenu : BaseEntity<SysMenu>
    {
        [Display(Name = "名称")]
        [Property]
        public string Name { get; set; }

        [Display(Name = "类名")]
        [Property]
        public string ClassName { get; set; }

        [Display(Name = "控制器")]
        [Property]
        public string ControllerName { get; set; }

        [Display(Name = "动作")]
        [Property]
        public string ActionName { get; set; }

        [Display(Name = "排序码")]
        [Property]
        public int SortCode { get; set; }

        [Display(Name = "上级菜单编号")]
        [Property]
        public int ParentId { get; set; }
    }
}
