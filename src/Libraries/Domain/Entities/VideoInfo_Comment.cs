namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("r_moeci_com.videoinfo_comment")]
    public partial class VideoInfo_Comment
    {
        public int ID { get; set; }

        public int? Status { get; set; }

        public int? VideoInfoId { get; set; }

        public int? CommentId { get; set; }
    }
}
