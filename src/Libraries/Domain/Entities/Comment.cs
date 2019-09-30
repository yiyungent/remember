namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Comment : BaseEntity
    {
        public int ID { get; set; }

        [Column(TypeName = "text")]
        [StringLength(65535)]
        public string Content { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? LastUpdateTime { get; set; }

        public int? LikeNum { get; set; }

        public int? DislikeNum { get; set; }

        #region Relationships

        [ForeignKey("Author")]
        public int AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        public virtual UserInfo Author { get; set; }

        [ForeignKey("Parent")]
        public int? ParentId { get; set; }
        [ForeignKey("ParentId")]
        public virtual Comment Parent { get; set; }

        #endregion
    }
}
