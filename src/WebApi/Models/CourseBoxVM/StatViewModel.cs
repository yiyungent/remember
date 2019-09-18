using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.CourseBoxVM
{
    /// <summary>
    /// 课程盒-统计
    /// </summary>
    public class StatViewModel
    {
        /// <summary>
        /// 课程盒ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 学习数
        /// </summary>
        public int Learn { get; set; }

        /// <summary>
        /// 评论数
        /// </summary>
        public int Comment { get; set; }

        /// <summary>
        /// 收藏数
        /// </summary>
        public int Favorite { get; set; }

        /// <summary>
        /// 分享数
        /// </summary>
        public int Share { get; set; }

        /// <summary>
        /// 点赞数
        /// </summary>
        public int Like { get; set; }
    }
}