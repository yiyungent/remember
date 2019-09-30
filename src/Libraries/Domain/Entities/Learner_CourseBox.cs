namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("r_moeci_com.learner_coursebox")]
    public partial class Learner_CourseBox : BaseEntity
    {
        public int ID { get; set; }

        public int? Status { get; set; }

        public DateTime? JoinTime { get; set; }

        public long? SpendTime { get; set; }

        public int? LastPlayVideoInfoId { get; set; }

        public int? LearnerId { get; set; }

        public int? CourseBoxId { get; set; }
    }
}
