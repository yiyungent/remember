namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// 实体类：学习者-视频课件
    /// </summary>
    public partial class Learner_VideoInfo : BaseEntity
    {
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// 最后访问IP
        /// </summary>
        [StringLength(30)]
        public string LastAccessIp { get; set; }

        /// <summary>
        /// 最后播放时间
        /// eg: 我最后是在 2019-12-12 22:21 时播放了此视频
        /// </summary>
        public DateTime? LastPlayTime { get; set; }

        /// <summary>
        /// 此学习者在此课件-学习进度
        /// <para>学习进度：视频调整播放位置，以前看完过此视频，则学习进度依然为满格状态，不变，而最新播放位置则不同</para>
        /// 毫秒
        /// 最大的视频播放位置
        /// </summary>
        public long? ProgressAt { get; set; }

        /// <summary>
        /// 此学习者在此视频课件-最新播放位置
        /// 毫秒
        /// </summary>
        public long? LastPlayAt { get; set; }

        #region Relationships

        /// <summary>
        /// 学习者
        /// </summary>
        [ForeignKey("Learner")]
        public int? LearnerId { get; set; }
        [ForeignKey("LearnerId")]
        public virtual UserInfo Learner { get; set; }

        /// <summary>
        /// 视频课件
        /// </summary>
        [ForeignKey("VideoInfo")]
        public int? VideoInfoId { get; set; } 
        [ForeignKey("VideoInfoId")]
        public virtual VideoInfo VideoInfo { get; set; }

        #endregion

        #region Helpers

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
    }
}
