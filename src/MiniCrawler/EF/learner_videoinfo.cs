namespace MiniCrawler.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("rememberdb.learner_videoinfo")]
    public partial class learner_videoinfo
    {
        public int ID { get; set; }

        [StringLength(100)]
        public string LastAccessIp { get; set; }

        public DateTime? LastPlayTime { get; set; }

        public long? ProgressAt { get; set; }

        public long? LastPlayAt { get; set; }

        public int? LearnerId { get; set; }

        public int? VideoInfoId { get; set; }

        public virtual videoinfo videoinfo { get; set; }

        public virtual userinfo userinfo { get; set; }
    }
}
