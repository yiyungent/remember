namespace MiniCrawler.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("rememberdb.comment_dislike")]
    public partial class comment_dislike
    {
        public int ID { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? CommentId { get; set; }

        public int? UserInfoId { get; set; }

        public virtual comment comment { get; set; }

        public virtual userinfo userinfo { get; set; }
    }
}
