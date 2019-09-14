using Castle.ActiveRecord;
using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    /// <summary>
    /// 实体类：学习者-课件
    /// </summary>
    [ActiveRecord]
    [Serializable]
    public class Learner_CourseInfo : BaseEntity<Learner_CourseInfo>
    {
        #region Properties

        /// <summary>
        /// 此学习者在此课件-学习进度
        /// <para>学习进度：视频调整播放位置，以前看完过此视频，则学习进度依然为满格状态，不变，而最新播放位置则不同</para>
        /// 毫秒
        /// 
        /// 视频：视频播放位置
        /// 帖子：记录看时间
        /// </summary>
        [Property(NotNull = false)]
        public long ProgressAt { get; set; }

        /// <summary>
        /// 此学习者在此课件-最新播放位置
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
                if (this.CourseInfo != null && this.CourseInfo.Duration != 0)
                {
                    percent = (float)ProgressAt / (float)this.CourseInfo.Duration;
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
        /// 课件
        /// </summary>
        [BelongsTo(Column = "CourseInfoId")]
        public CourseInfo CourseInfo { get; set; }

        #endregion
    }
}
