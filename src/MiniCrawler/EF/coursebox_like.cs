namespace MiniCrawler.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("rememberdb.coursebox_like")]
    public partial class coursebox_like
    {
        public int ID { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? CourseBoxId { get; set; }

        public int? UserInfoId { get; set; }

        public virtual coursebox coursebox { get; set; }

        public virtual userinfo userinfo { get; set; }
    }
}
