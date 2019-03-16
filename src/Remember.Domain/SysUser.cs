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

        /// <summary>
        /// 用户创建的 CardBox 列表
        ///     一对多
        /// </summary>
        [Display(Name = "用户创建的 CardBox")]
        [HasMany(ColumnKey = "Creator_UserId")]
        public IList<CardBox> CreateCardBoxList { get; set; }

        /// <summary>
        /// 用户阅读的 CardBox 列表    (不包括他创建的，列表中是其它用户创建的 CardBox)
        ///     多对多
        ///         一个用户可以阅读多个 CardBox，一个 CardBox 也可以被多个用户阅读(共享)
        /// </summary>
        [Display(Name = "用户阅读的 CardBox")]
        [HasAndBelongsToMany(Table = "User_CardBox_ForRead", ColumnKey = "UserId", ColumnRef = "CardBoxId")]
        public IList<CardBox> ReadCardBoxList { get; set; }
    }
}
