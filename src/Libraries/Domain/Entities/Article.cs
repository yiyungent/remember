namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Article : BaseEntity
    {
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [StringLength(30)]
        public string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [Column(TypeName = "text")]
        [StringLength(8000)]
        public string Content { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 最近更新
        /// </summary>
        public DateTime? LastUpdateTime { get; set; }

        /// <summary>
        /// 自定义Url
        /// </summary>
        [StringLength(1000)]
        [Column(TypeName = "text")]
        public string CustomUrl { get; set; }

        #region Relationships

        /// <summary>
        /// 作者
        /// </summary>
        [ForeignKey("Author")]
        public int? AuthorId { get; set; }
        /// <summary>
        /// 作者
        /// </summary>
        [ForeignKey("AuthorId")]
        public virtual UserInfo Author { get; set; }

        #endregion
    }
}
