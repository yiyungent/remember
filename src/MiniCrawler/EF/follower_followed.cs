namespace MiniCrawler.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("rememberdb.follower_followed")]
    public partial class follower_followed
    {
        public int ID { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? FollowerId { get; set; }

        public int? FollowedId { get; set; }

        public virtual userinfo userinfo { get; set; }

        public virtual userinfo userinfo1 { get; set; }
    }
}
