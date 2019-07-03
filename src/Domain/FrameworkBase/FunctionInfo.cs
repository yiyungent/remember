using Castle.ActiveRecord;
using Domain.FrameworkBase.Common;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public partial class FunctionInfo
    {
        /// <summary>
        /// 权限键（唯一标识）
        /// </summary>
        [Display(Name = "权限键")]
        [Property(Length = 50, NotNull = true, Unique = true)]
        public string AuthKey { get; set; }

        /// <summary>
        /// 操作名称
        /// </summary>
        [Display(Name = "操作名称")]
        [Property(Length = 30, NotNull = true)]
        public string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "备注")]
        [Property(Length = 300, NotNull = false)]
        public string Remark { get; set; }

        #region Relationship

        /// <summary>
        /// 系统菜单
        ///     多对一关系
        /// </summary>
        [Display(Name = "系统菜单")]
        [BelongsTo(Column = "MenuId")]
        public Sys_Menu Sys_Menu { get; set; }

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
