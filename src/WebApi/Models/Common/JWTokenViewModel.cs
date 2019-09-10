using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.Common
{
    public class JWTokenViewModel
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime Expire { get; set; }
    }
}