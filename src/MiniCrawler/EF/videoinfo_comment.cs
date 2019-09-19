namespace MiniCrawler.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("rememberdb.videoinfo_comment")]
    public partial class videoinfo_comment
    {
        public int ID { get; set; }

        public int? VideoInfoId { get; set; }

        public int? CommentId { get; set; }

        public virtual comment comment { get; set; }

        public virtual videoinfo videoinfo { get; set; }
    }
}
