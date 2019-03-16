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
        public string dbhost { get; set; }

        /// <summary>
        /// 数据库名
        /// </summary>
        [Display(Name = "数据库名")]
        [Required(ErrorMessage = "数据库名不能为空")]
        public string dbname { get; set; }

        /// <summary>
        /// 数据库用户名
        /// </summary>
        [Display(Name = "数据库用户名")]
        [Required(ErrorMessage = "数据库用户名不能为空")]
        public string dbuser { get; set; }

        /// <summary>
        /// 数据库密码
        /// </summary>
        [Display(Name = "数据库密码")]
        [Required(ErrorMessage = "数据库密码不能为空")]
        public string dbpw { get; set; }

        /// <summary>
        /// 管理员账号
        /// </summary>
        [Display(Name = "管理员账号")]
        [Required(ErrorMessage = "管理员账号不能为空")]
        [MaxLength(30, ErrorMessage = "管理员账号不能超过30个字符")]
        public string username { get; set; }

        /// <summary>
        /// 管理员密码
        /// </summary>
        [Display(Name = "管理员密码")]
        [Required(ErrorMessage = "密码不能为空")]
        [MinLength(11, ErrorMessage = "密码11位及以上")]
        public string password { get; set; }

        [Display(Name = "重复密码")]
        [Required(ErrorMessage = "重复密码不能为空")]
        [Compare("password", ErrorMessage = "两次输入不一致")]
        public string passwordConfirm { get; set; }
    }
}