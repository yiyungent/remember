using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.UserInfoVM
{
    public class RelationViewModel
    {
        public IList<RelationItem> Relations { get; set; }

        public sealed class RelationItem
        {
            public int uid { get; set; }

            //public long CreateTime { get; set; }

            /// <summary>
            /// 0: 没有关系
            /// 1: 我单方面关注他
            /// 2: 他单方面关注我
            /// 3: 互相关注
            /// </summary>
            public int Relation { get; set; }
        }
    }
}