using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel.DataAnnotations;

namespace Remember.Domain
{
    public class SysUser
    {
        [Display(Name = "用户编号")]
        public int ID { get; set; }

        [Display(Name = "用户名")]
        public string Name { get; set; }

        [Display(Name = "登录账号")]
        public string LoginAccount { get; set; }

        [Display(Name = "密码")]
        public string Password { get; set; }

        /// <summary>
        /// 状态
        ///     0: 正常
        ///     1: 禁用
        /// </summary>
        [Display(Name = "状态")]
        public int Status { get; set; }
    }
}
