using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Framework.Models
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
        /// 签发时间
        /// Unix时间戳
        /// </summary>
        public long Create { get; set; }

        /// <summary>
        /// 过期时间
        /// Unix时间戳
        /// </summary>
        public long Expire { get; set; }
    }
}