namespace MiniCrawler.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("rememberdb.coursebox_participant")]
    public partial class coursebox_participant
    {
        public int ID { get; set; }

        public bool? IsAgreed { get; set; }

        public DateTime? AgreeTime { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? CourseBoxId { get; set; }

        public int? ParticipantId { get; set; }

        public int? ParticipantInfoId { get; set; }

        public virtual coursebox coursebox { get; set; }

        public virtual participantinfo participantinfo { get; set; }

        public virtual userinfo userinfo { get; set; }
    }
}
