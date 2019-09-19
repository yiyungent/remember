using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.UserInfoVM
{
    public class UserInfoViewModel
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string UserName { get; set; }

        public string Desc { get; set; }

        public string Avatar { get; set; }

        /// <summary>
        /// 粉丝数
        /// </summary>
        public int FansNum { get; set; }

        /// <summary>
        /// 关注数-我关注了多少人
        /// </summary>
        public int FollowNum { get; set; }

        /// <summary>
        /// 文章数-动态
        /// </summary>
        public int ArticleNum { get; set; }
    }
}