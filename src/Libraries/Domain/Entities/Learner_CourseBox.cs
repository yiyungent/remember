namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// 实体类：学习者-课程
    /// </summary>
    public partial class Learner_CourseBox : BaseEntity
    {
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// 加入学习的时间
        /// </summary>
        public DateTime? JoinTime { get; set; }

        /// <summary>
        /// 此学习者在此课程总学习时间: 花费时间
        /// 秒
        /// <para>注意：不一定是学习者在此课程上的所有课件的进度时间，因为课程存在反复看，反复看时间也要计算在内</para>
        /// </summary>
        public long? SpendTime { get; set; }

        #region Relationships

        /// <summary>
        /// 最新播放视频
        /// </summary>
        [ForeignKey("LastPlayVideoInfo")]
        public int? LastPlayVideoInfoId { get; set; }
        [ForeignKey("LastPlayVideoInfoId")]
        public virtual VideoInfo LastPlayVideoInfo { get; set; }

        /// <summary>
        /// 学习者
        /// </summary>
        [ForeignKey("Learner")]
        public int? LearnerId { get; set; }
        [ForeignKey("Learner")]
        public virtual UserInfo Learner { get; set; }

        /// <summary>
        /// 课程盒
        /// </summary>
        [ForeignKey("CourseBox")]
        public int? CourseBoxId { get; set; }
        [ForeignKey("CourseBoxId")]
        public virtual CourseBox CourseBox { get; set; }

        #endregion
    }
}
