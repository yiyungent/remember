namespace MiniCrawler.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("rememberdb.videoinfo")]
    public partial class videoinfo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public videoinfo()
        {
            learner_coursebox = new HashSet<learner_coursebox>();
            learner_videoinfo = new HashSet<learner_videoinfo>();
            videoinfo_comment = new HashSet<videoinfo_comment>();
        }

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

        public int? Page { get; set; }

        public int? CourseBoxId { get; set; }

        public virtual coursebox coursebox { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<learner_coursebox> learner_coursebox { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<learner_videoinfo> learner_videoinfo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<videoinfo_comment> videoinfo_comment { get; set; }
    }
}
