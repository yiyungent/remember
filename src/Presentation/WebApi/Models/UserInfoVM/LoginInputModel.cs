using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApi.Models.UserInfoVM
{
    public class LoginInputModel
    {
        /// <summary>
        /// 登录账号: 用户名/邮箱/手机
        /// </summary>
        [Required]
        public string LoginAccount { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class LoginResultViewModel
    {
        /// <summary>
        /// 加密后的JWToken
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// JTW 过期时间
        /// Unix时间戳
        /// </summary>
        public long Expire { get; set; }
    }
}