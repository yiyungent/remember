namespace MiniCrawler.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("rememberdb.learner_coursebox")]
    public partial class learner_coursebox
    {
        public int ID { get; set; }

        public DateTime? JoinTime { get; set; }

        public long? SpendTime { get; set; }

        public int? LastPlayVideoInfoId { get; set; }

        public int? LearnerId { get; set; }

        public int? CourseBoxId { get; set; }

        public virtual coursebox coursebox { get; set; }

        public virtual videoinfo videoinfo { get; set; }

        public virtual userinfo userinfo { get; set; }
    }
}
