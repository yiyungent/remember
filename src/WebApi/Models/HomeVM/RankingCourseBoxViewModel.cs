using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Models.CourseBoxVM;

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
    }

    public class RankingCourseBoxItem
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
        public string Description { get; set; }

        /// <summary>
        /// 创建者
        /// </summary>
        public UserInfoVM.UserInfoViewModel Creator { get; set; }

        /// <summary>
        /// 封面图
        /// </summary>
        public string PicUrl { get; set; }
    }
}