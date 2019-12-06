using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Models.BookInfoVM;

namespace WebApi.Models.HomeVM
{
    public class RankingCourseBoxViewModel
    {
        public RankingCourseBoxItem CourseBox { get; set; }

        /// <summary>
        /// 学习人数
        /// </summary>
        public int LearnNum { get; set; }

        /// <summary>
        /// 总共学习时间
        /// </summary>
        public double TotalSpendTime { get; set; }

        /// <summary>
        /// 热门排名-第几名
        /// </summary>
        public int RankingNum { get; set; }


        public sealed class RankingCourseBoxItem
        {
            /// <summary>
            /// 课程ID
            /// </summary>
            public int ID { get; set; }

            /// <summary>
            /// 课程名
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// 描述
            /// </summary>
            public string Desc { get; set; }

            /// <summary>
            /// 创建者
            /// </summary>
            public Creator Creator { get; set; }

            /// <summary>
            /// 封面图
            /// </summary>
            public string PicUrl { get; set; }
        }

        public sealed class Creator
        {
            public int ID { get; set; }

            public string Name { get; set; }

            public string UserName { get; set; }

            public string Desc { get; set; }

            public string Avatar { get; set; }
        }

    }
}