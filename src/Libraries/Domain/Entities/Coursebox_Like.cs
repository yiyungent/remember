namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("r_moeci_com.coursebox_like")]
    public partial class Coursebox_Like
    {
        public int ID { get; set; }

        public int? Status { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? CourseBoxId { get; set; }

        public int? UserInfoId { get; set; }
    }
}
