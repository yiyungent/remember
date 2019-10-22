using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.VideoInfoVM
{
    public class VideoHistoryViewModel
    {

        public LearnerModel Learner { get; set; }

        public VideoModel Video { get; set; }

        /// <summary>
        /// 最后播放时间
        /// eg: 我最后是在 2019-12-12 22:21 时播放了此视频
        /// 毫秒
        /// </summary>
        public long LastPlayTime { get; set; }

        /// <summary>
        /// 此学习者在此课件-学习进度
        /// <para>学习进度：视频调整播放位置，以前看完过此视频，则学习进度依然为满格状态，不变，而最新播放位置则不同</para>
        /// 毫秒
        /// 最大的视频播放位置
        /// </summary>
        public long ProgressAt { get; set; }

        /// <summary>
        /// 此学习者在此视频课件-最新播放位置
        /// 毫秒
        /// </summary>
        public long LastPlayAt { get; set; }

        public sealed class VideoModel
        {
            public int ID { get; set; }

            public string Title { get; set; }
        }

        public sealed class LearnerModel
        {
            public int ID { get; set; }

            public string UserName { get; set; }

            public string Avatar { get; set; }
        }

    }
}