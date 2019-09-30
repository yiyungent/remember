namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("r_moeci_com.coursebox_participant")]
    public partial class Coursebox_Participant
    {
        public int ID { get; set; }

        public int? Status { get; set; }

        public bool? IsAgreed { get; set; }

        public DateTime? AgreeTime { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? CourseBoxId { get; set; }

        public int? ParticipantId { get; set; }

        public int? ParticipantInfoId { get; set; }
    }
}
