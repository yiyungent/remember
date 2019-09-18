using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.CourseInfoVM
{
    public class CourseInfoViewModel
    {
        public int ID { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public int CourseInfoType { get; set; }

        public int CourseBoxId { get; set; }

        /// <summary>
        /// 最后访问IP
        /// </summary>
        public string LastAccessIp { get; set; }

        /// <summary>
        /// 最后访问时间
        /// (毫秒)
        /// </summary>
        public long LastAccessTime { get; set; }

        /// <summary>
        /// 此学习者在此课件-学习进度
        /// <para>学习进度：视频调整播放位置，以前看完过此视频，则学习进度依然为满格状态，不变，而最新播放位置则不同</para>
        /// 毫秒
        /// 
        /// 视频：视频播放位置
        /// 帖子：记录看时间
        /// (毫秒)
        /// </summary>
        public long ProgressAt { get; set; }

        /// <summary>
        /// 此学习者在此课件-最新播放位置
        /// 毫秒
        /// </summary>
        public long LastPlayAt { get; set; }
    }
}