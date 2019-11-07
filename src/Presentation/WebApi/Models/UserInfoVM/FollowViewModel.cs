using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.UserInfoVM
{
    public class FollowViewModel
    {
        /// <summary>
        /// 关注者
        /// </summary>
        public User Follower { get; set; }

        /// <summary>
        /// 被关注者
        /// </summary>
        public User Followed { get; set; }

        /// <summary>
        /// 关系
        /// 0: 没有关系
        /// 1: 我单方面关注他
        /// 2: 他单方面关注我
        /// 3: 互相关注
        /// </summary>
        public int Relation { get; set; }

        public sealed class User
        {
            public int ID { get; set; }

            /// <summary>
            /// 关注人数
            /// </summary>
            public int Follow { get; set; }

            /// <summary>
            /// 粉丝数
            /// </summary>
            public int Fans { get; set; }
        }
    }
}