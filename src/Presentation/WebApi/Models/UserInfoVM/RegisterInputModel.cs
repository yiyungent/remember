using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApi.Models.UserInfoVM
{
    public class RegisterInputModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        /// <summary>
        /// 注册：邮箱和手机号至少要提供一个
        /// </summary>
        [Required]
        public string Email { get; set; }

        /// <summary>
        /// 注册：邮箱和手机号至少要提供一个
        /// </summary>
        public string Phone { get; set; }
    }
}