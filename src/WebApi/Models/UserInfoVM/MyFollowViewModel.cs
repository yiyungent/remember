using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.UserInfoVM
{
    public class MyFollowViewModel
    {

        public List<MyFollowGroup> Groups { get; set; }

        public sealed class MyFollowGroup
        {
            public int ID { get; set; }

            public string GroupName { get; set; }

            /// <summary>
            /// 是否展开
            /// </summary>
            public bool IsFolder { get; set; } = true;

            public List<MyFollowInfo> Users { get; set; }
        }

        public sealed class MyFollowInfo
        {
            /// <summary>
            /// 关注时间
            /// </summary>
            public long CreateTime { get; set; }

            /// <summary>
            /// 关系
            /// 0: 没有关系
            /// 1: 我单方面关注他
            /// 2: 他单方面关注我
            /// 3: 互相关注
            /// </summary>
            public int Relation { get; set; }

            public User User { get; set; }
           
        }

        public sealed class User
        {
            public int ID { get; set; }

            public string UserName { get; set; }

            public string Desc { get; set; }

            public string Avatar { get; set; }
        }

    }
}