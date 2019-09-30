namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("r_moeci_com.comment")]
    public partial class Comment
    {
        public int ID { get; set; }

        public int? Status { get; set; }

        [Column(TypeName = "text")]
        [StringLength(65535)]
        public string Content { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? LastUpdateTime { get; set; }

        public int? LikeNum { get; set; }

        public int? DislikeNum { get; set; }

        public int AuthorId { get; set; }

        public int? ParentId { get; set; }
    }
}
