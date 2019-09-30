namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Learner_CourseBox : BaseEntity
    {
        public int ID { get; set; }

        public DateTime? JoinTime { get; set; }

        public long? SpendTime { get; set; }

        #region Relationships

        [ForeignKey("LastPlayVideoInfo")]
        public int? LastPlayVideoInfoId { get; set; }
        [ForeignKey("LastPlayVideoInfoId")]
        public virtual VideoInfo LastPlayVideoInfo { get; set; }

        [ForeignKey("Learner")]
        public int? LearnerId { get; set; }
        [ForeignKey("Learner")]
        public virtual UserInfo Learner { get; set; }

        [ForeignKey("CourseBox")]
        public int? CourseBoxId { get; set; }
        [ForeignKey("CourseBoxId")]
        public virtual CourseBox CourseBox { get; set; }

        #endregion
    }
}
