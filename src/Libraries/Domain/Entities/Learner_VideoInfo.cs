namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("r_moeci_com.learner_videoinfo")]
    public partial class Learner_VideoInfo
    {
        public int ID { get; set; }

        public int? Status { get; set; }

        [StringLength(100)]
        public string LastAccessIp { get; set; }

        public DateTime? LastPlayTime { get; set; }

        public long? ProgressAt { get; set; }

        public long? LastPlayAt { get; set; }

        public int? LearnerId { get; set; }

        public int? VideoInfoId { get; set; }
    }
}
