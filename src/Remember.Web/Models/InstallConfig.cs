using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Remember.Web.Models
{
    /// <summary>
    /// 安装配置
    /// </summary>
    public class InstallConfig
    {
        /// <summary>
        /// 数据库主机
        /// </summary>
        [Display(Name = "数据库主机")]
        [Required(ErrorMessage = "数据库主机不能为空")]
        public string Dbhost { get; set; }

        /// <summary>
        /// 数据库名
        /// </summary>
        [Display(Name = "数据库名")]
        [Required(ErrorMessage = "数据库名不能为空")]
        public string Dbname { get; set; }

        /// <summary>
        /// 数据库用户名
        /// </summary>
        [Display(Name = "数据库用户名")]
        [Required(ErrorMessage = "数据库用户名不能为空")]
        public string Dbuser { get; set; }

        /// <summary>
        /// 数据库密码
        /// </summary>
        [Display(Name = "数据库密码")]
        [Required(ErrorMessage = "数据库密码不能为空")]
        public string Dbpw { get; set; }

        /// <summary>
        /// 管理员账号
        /// </summary>
        [Display(Name = "管理员账号")]
        [Required(ErrorMessage = "管理员账号不能为空")]
        [MaxLength(30, ErrorMessage = "管理员账号不能超过30个字符")]
        public string Username { get; set; }

        /// <summary>
        /// 管理员密码
        /// </summary>
        [Display(Name = "管理员密码")]
        [RegularExpression(@"^(?![A-Za-z0-9]+$)(?![a-z0-9\\W]+$)(?![A-Za-z\\W]+$)(?![A-Z0-9\\W]+$)[a-zA-Z0-9\\W]{8,}$", ErrorMessage = "管理员密码必须包含大写字母、小写字母、数字、特殊符号 且不低于8位")]
        public string Password { get; set; }

        [Display(Name = "重复密码")]
        [Compare("Password",ErrorMessage = "两次输入不一致")]
        public string PasswordConfirm { get; set; }
    }
}