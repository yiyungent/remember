using Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Areas.Account.Models
{
    public class AccountModel
    { }

    public class LoginModel
    {
        [Display(Name = "账号")]
        [Required(ErrorMessage = "账号不能为空")]
        public string LoginAccount { get; set; }

        [Display(Name = "密码")]
        [Required(ErrorMessage = "密码不能为空")]
        [HiddenInput]
        public string Password { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        [Display(Name = "验证码")]
        public string ValidateCode { get; set; }

        /// <summary>
        /// 记住密码
        /// </summary>
        [Display(Name = "记住我")]
        public bool IsRememberMe { get; set; }
    }
}