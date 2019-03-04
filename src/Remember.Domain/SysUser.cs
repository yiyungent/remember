using Castle.ActiveRecord;
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
        [Property]
        public string Name { get; set; }

        [Display(Name = "登录账号")]
        [Property]
        public string LoginAccount { get; set; }

        [Display(Name = "密码")]
        [Property]
        public string Password { get; set; }

        /// <summary>
        /// 状态
        ///     0: 正常
        ///     1: 禁用
        /// </summary>
        [Display(Name = "状态")]
        [Property]
        public int Status { get; set; }
    }
}
