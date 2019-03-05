using Castle.ActiveRecord;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Remember.Domain
{
    /// <summary>
    /// 实体类：用户
    /// </summary>
    [ActiveRecord]
    public class SysUser : BaseEntity<SysUser>
    {
        [Display(Name = "用户名")]
        [Property(Length = 30, NotNull = true)]
        public string Name { get; set; }

        [Display(Name = "登录账号")]
        [Property(Length = 30, NotNull = true)]
        public string LoginAccount { get; set; }

        [Display(Name = "密码")]
        [Property(Length = 64, NotNull = true)]
        public string Password { get; set; }

        /// <summary>
        /// 状态
        ///     0: 正常
        ///     1: 禁用
        /// </summary>
        [Display(Name = "状态")]
        [Property]
        public int Status { get; set; }

        /// <summary>
        /// 担任角色列表
        ///     多对多：新建一张表，连接这两张表
        /// </summary>
        [Display(Name = "担任角色列表")]
        // 当前此类   有 HasMany(一对多)   并且   属于 BelongsTo(多对一)      多个指定类
        // -----     Has                  And    BelongsTo                  Many
        // Table 第三张（连接）表名，
        // ColumnKey = 此类的表的主键在第三张表上的名字---外键  指向  此类表主键
        // ColumnRef = 另一类的表的主键在第三张表上的名字----外键  指向   另一类表主键
        [HasAndBelongsToMany(Table = "Role_User", ColumnKey = "UserId", ColumnRef = "RoleId")]
        public IList<SysRole> SysRoleList { get; set; }
    }
}
