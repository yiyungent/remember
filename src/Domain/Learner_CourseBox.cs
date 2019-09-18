using Castle.ActiveRecord;
using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Domain
{
    /// <summary>
    /// 实体类：学习者-课程
    /// </summary>
    [ActiveRecord]
    [Serializable]
    public class Learner_CourseBox : BaseEntity<Learner_CourseBox>
    {
        #region Properties

        /// <summary>
        /// 加入学习的时间
        /// </summary>
        [Property(NotNull = false)]
        public DateTime JoinTime { get; set; }

        /// <summary>
        /// 此学习者在此课程总学习时间: 花费时间
        /// 秒
        /// <para>注意：不一定是学习者在此课程上的所有课件的进度时间，因为课程存在反复看，反复看时间也要计算在内</para>
        /// </summary>
        [Property(NotNull = false)]
        public long SpendTime { get; set; }

        /// <summary>
        /// 最新播放视频
        /// </summary>
        [BelongsTo(Column = "LastPlayVideoInfoId")]
        public VideoInfo LastPlayVideoInfo { get; set; }

        #endregion

        #region Relationships

        /// <summary>
        /// 学习者
        /// </summary>
        [BelongsTo(Column = "LearnerId")]
        public UserInfo Learner { get; set; }

        /// <summary>
        /// 课程盒
        /// </summary>
        [BelongsTo(Column = "CourseBoxId")]
        public CourseBox CourseBox { get; set; }

        #endregion
    }
}
