namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Learner_VideoInfo : BaseEntity
    {
        public int ID { get; set; }

        [StringLength(100)]
        public string LastAccessIp { get; set; }

        public DateTime? LastPlayTime { get; set; }

        public long? ProgressAt { get; set; }

        public long? LastPlayAt { get; set; }

        #region Relationships

        [ForeignKey("Learner")]
        public int? LearnerId { get; set; }
        [ForeignKey("LearnerId")]
        public virtual UserInfo Learner { get; set; }

        [ForeignKey("VideoInfo")]
        public int? VideoInfoId { get; set; } 
        [ForeignKey("VideoInfoId")]
        public virtual VideoInfo VideoInfo { get; set; }

        #endregion
    }
}
