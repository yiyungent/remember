namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class VideoInfo : BaseEntity
    {
        public int ID { get; set; }

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

        #region Relationships

        [ForeignKey("CourseBox")]
        public int? CourseBoxId { get; set; }
        [ForeignKey("CourseBoxId")]
        public virtual CourseBox CourseBox { get; set; }

        #endregion
    }
}
