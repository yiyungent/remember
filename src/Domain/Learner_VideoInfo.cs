using Castle.ActiveRecord;
using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    /// <summary>
    /// 实体类：学习者-视频课件
    /// </summary>
    [ActiveRecord]
    [Serializable]
    public class Learner_VideoInfo : BaseEntity<Learner_VideoInfo>
    {
        #region Properties

        /// <summary>
        /// 最后访问IP
        /// </summary>
        [Property(Length = 100, NotNull = false)]
        public string LastAccessIp { get; set; }

        /// <summary>
        /// 最后播放时间
        /// eg: 我最后是在 2019-12-12 22:21 时播放了此视频
        /// </summary>
        [Property(NotNull = false)]
        public DateTime LastPlayTime { get; set; }

        /// <summary>
        /// 此学习者在此课件-学习进度
        /// <para>学习进度：视频调整播放位置，以前看完过此视频，则学习进度依然为满格状态，不变，而最新播放位置则不同</para>
        /// 毫秒
        /// 最大的视频播放位置
        /// </summary>
        [Property(NotNull = false)]
        public long ProgressAt { get; set; }

        /// <summary>
        /// 此学习者在此视频课件-最新播放位置
        /// 毫秒
        /// </summary>
        [Property(NotNull = false)]
        public long LastPlayAt { get; set; }

        /// <summary>
        /// 进度百分比
        /// 为1则已完成
        /// </summary>
        public float Percent
        {
            get
            {
                float percent = 0;
                if (this.VideoInfo != null && this.VideoInfo.Duration != 0)
                {
                    percent = (float)ProgressAt / (float)this.VideoInfo.Duration;
                }

                return percent;
            }
        }

        #endregion

        #region Relationships

        /// <summary>
        /// 学习者
        /// </summary>
        [BelongsTo(Column = "LearnerId")]
        public UserInfo Learner { get; set; }

        /// <summary>
        /// 视频课件
        /// </summary>
        [BelongsTo(Column = "VideoInfoId")]
        public VideoInfo VideoInfo { get; set; }

        #endregion
    }
}
