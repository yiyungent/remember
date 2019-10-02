namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// 实体类：课程-参与者
    /// </summary>
    public partial class CourseBox_Participant : BaseEntity
    {
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// 是否被参与者同意
        /// <para>
        /// 投稿者发起投稿时，默认为0,即不同意，然后当参与者同意后（为1），才予以在此课程显示此参与者
        /// </para>
        /// </summary>
        public bool? IsAgreed { get; set; }

        /// <summary>
        /// 参与者同意参与 时间
        /// </summary>
        public DateTime? AgreeTime { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        #region Relationships

        /// <summary>
        /// 课程
        /// </summary>
        [ForeignKey("CourseBox")]
        public int? CourseBoxId { get; set; }
        [ForeignKey("CourseBoxId")]
        public virtual CourseBox CourseBox { get; set; }

        /// <summary>
        /// 参与者
        /// </summary>
        [ForeignKey("Participant")]
        public int? ParticipantId { get; set; }
        [ForeignKey("ParticipantId")]
        public virtual UserInfo Participant { get; set; }

        /// <summary>
        /// 参与者信息
        /// </summary>
        [ForeignKey("ParticipantInfo")]
        public int? ParticipantInfoId { get; set; }
        [ForeignKey("ParticipantInfoId")]
        public virtual ParticipantInfo ParticipantInfo { get; set; }

        #endregion
    }
}
