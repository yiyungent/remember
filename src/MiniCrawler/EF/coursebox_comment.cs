namespace MiniCrawler.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("rememberdb.coursebox_comment")]
    public partial class coursebox_comment
    {
        public int ID { get; set; }

        public int? CourseBoxId { get; set; }

        public int? CommentId { get; set; }

        public virtual comment comment { get; set; }

        public virtual coursebox coursebox { get; set; }
    }
}
