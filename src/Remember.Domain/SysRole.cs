using Castle.ActiveRecord;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Remember.Domain
{
    /// <summary>
    /// 实体类：角色
    /// </summary>
    [ActiveRecord]
    public class SysRole : BaseEntity<SysRole>
    {
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

        /// <summary>
        /// 用户列表
        ///     多对多
        /// </summary>
        [Display(Name = "用户列表")]
        [HasAndBelongsToMany(Table = "Role_User", ColumnKey = "RoleId", ColumnRef = "UserId")]
        public IList<SysUser> SysUserList { get; set; }
    }
}
