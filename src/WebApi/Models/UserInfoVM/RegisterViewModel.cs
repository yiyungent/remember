using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApi.Models.UserInfoVM
{
    public class RegisterViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Name { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }
    }

    public class RegisterResult
    {
        public int Code { get; set; }

        public string Message { get; set; }

        /// <summary>
        /// JTW Encode 后字符串
        /// </summary>
        public string ApiToken { get; set; }
    }
}