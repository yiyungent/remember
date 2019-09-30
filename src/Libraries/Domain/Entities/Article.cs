namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("r_moeci_com.article")]
    public partial class Article : BaseEntity
    {
        public int ID { get; set; }

        public int? Status { get; set; }

        [StringLength(30)]
        public string Title { get; set; }

        [Column(TypeName = "text")]
        [StringLength(65535)]
        public string Content { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? LastUpdateTime { get; set; }

        [StringLength(30)]
        public string CustomUrl { get; set; }

        #region Relationships

        [ForeignKey("Author")]
        public int? AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        public virtual UserInfo Author { get; set; } 

        #endregion
    }
}
