using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.CourseBoxVM
{
    public class StudyInfoViewModel
    {
        /// <summary>
        /// 加入学习的时间
        /// Unix时间戳:秒
        /// </summary>
        public long JoinTime { get; set; }


        /// <summary>
        /// 学习时间:花费时间
        /// 秒
        /// </summary>
        public long SpendTime { get; set; }

        /// <summary>
        /// 学习进度，存储json
        /// {
        ///     CourseInfoId: 2, // 最近学习到的知识卡ID
        ///     
        /// }
        /// </summary>
        public string StudyProgress { get; set; }
    }
}