using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.HomeVM
{
    public class LastCourseBoxViewModel
    {
        public int LearnNum { get; set; }

        public CourseBoxItem CourseBox { get; set; }

        public int RankingNum { get; set; }

        public sealed class CourseBoxItem
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

            /// <summary>
            /// js时间戳
            /// </summary>
            public long CreateTime { get; set; }
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