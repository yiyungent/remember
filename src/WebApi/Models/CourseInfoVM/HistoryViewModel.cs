using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.CourseInfoVM
{
    public class HistoryViewModel
    {
        /// <summary>
        /// 访问IP
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        /// 访问国家
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// 最后访问时间
        /// js时间戳(毫秒)
        /// </summary>
        public long LastAccessTime { get; set; }

        ///// <summary>
        ///// 访问者
        ///// </summary>
        //public UserInfoViewModel Visitor { get; set; }

        /// <summary>
        /// 在此课件内容内的最新播放位置
        /// 毫秒
        /// </summary>
        public long ProgressAt
        { get; set; }

        ///// <summary>
        ///// 最后访问课件ID
        ///// 视频：必须要点击开始播放才算是访问过，不然不记录为播放历史
        ///// </summary>
        //public int LastCourseInfoId { get; set; }

        /// <summary>
        /// 此课件最新播放位置
        /// </summary>
        public long LastPlayAt { get; set; }

        /// <summary>
        /// 持续时间
        /// 秒
        /// </summary>
        public long Duration { get; set; }

        /// <summary>
        /// 所在课程ID
        /// </summary>
        public int CourseBoxId { get; set; }


        public sealed class UserInfoViewModel
        {
            public int ID { get; set; }

            public string UserName { get; set; }

            public string Name { get; set; }

            public string Avatar { get; set; }

            public int Coin { get; set; }
        }
    }
}