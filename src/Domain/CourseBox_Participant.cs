using Castle.ActiveRecord;
using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Domain
{
    /// <summary>
    /// 实体类：课程-参与者
    /// </summary>
    [ActiveRecord]
    [Serializable]
    public class CourseBox_Participant : BaseEntity<CourseBox_Participant>
    {
        #region Properties

        /// <summary>
        /// 是否被参与者同意
        /// <para>
        /// 投稿者发起投稿时，默认为0,即不同意，然后当参与者同意后（为1），才予以在此课程显示此参与者
        /// </para>
        /// </summary>
        [Property]
        public bool IsAgreed { get; set; }

        /// <summary>
        /// 参与者同意参与 时间
        /// </summary>
        [Property(NotNull = false)]
        public DateTime AgreeTime { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Property(NotNull = false)]
        public DateTime CreateTime { get; set; }

        #endregion

        #region Relationships

        /// <summary>
        /// 课程
        /// </summary>
        [BelongsTo(Column = "CourseBoxId")]
        public CourseBox CourseBox { get; set; }

        /// <summary>
        /// 参与者
        /// </summary>
        [BelongsTo(Column = "ParticipantId")]
        public UserInfo Participant { get; set; }

        /// <summary>
        /// 参与者信息
        /// </summary>
        [BelongsTo(Column = "ParticipantInfoId")]
        public ParticipantInfo ParticipantInfo { get; set; }

        #endregion
    }
}
