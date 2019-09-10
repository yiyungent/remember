using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApi.Models.UserInfoVM
{
    public class LoginViewModel
    {
        /// <summary>
        /// 登录账号: 用户名/邮箱/手机
        /// </summary>
        [Required]
        public string LoginAccount { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class LoginResult
    {
        public int Code { get; set; }

        public string Message { get; set; }

        /// <summary>
        /// JTW Encode 后字符串
        /// </summary>
        public string ApiToken { get; set; }
    }
}