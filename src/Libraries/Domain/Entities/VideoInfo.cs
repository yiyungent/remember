namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("r_moeci_com.videoinfo")]
    public partial class VideoInfo
    {
        public int ID { get; set; }

        public int? Status { get; set; }

        [StringLength(200)]
        public string Title { get; set; }

        [Column(TypeName = "text")]
        [Required]
        [StringLength(65535)]
        public string PlayUrl { get; set; }

        [Column(TypeName = "text")]
        [StringLength(65535)]
        public string SubTitleUrl { get; set; }

        public long? Duration { get; set; }

        public long? Size { get; set; }

        public int? Page { get; set; }

        public int? CourseBoxId { get; set; }
    }
}
