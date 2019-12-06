namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// 文库-章节内容
    /// </summary>
    public partial class BookSection : BaseEntity
    {
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// 章节标题
        /// </summary>
        [StringLength(100)]
        [Column(TypeName = "text")]
        public string Title { get; set; }

        /// <summary>
        /// 文字内容
        /// </summary>
        [Column(TypeName = "text")]
        [Required]
        [StringLength(8000)]
        public string Content { get; set; }

        /// <summary>
        /// 持续时间
        /// 秒
        /// 阅读完它所需时间
        /// </summary>
        public long Duration { get; set; }

        /// <summary>
        /// 排序码
        /// 第几章节
        /// </summary>
        public int SortCode { get; set; }

        /// <summary>
        /// 删除时间：为null，则未删除
        /// </summary>
        public DateTime? DeletedAt { get; set; }

        /// <summary>
        /// 是否被删除
        /// </summary>
        public bool IsDeleted { get; set; }

        #region Relationships

        /// <summary>
        /// 所在 BookInfo
        ///     多对一
        /// </summary>
        [ForeignKey("BookInfo")]
        public int BookInfoId { get; set; }
        [ForeignKey("BookInfoId")]
        public virtual BookInfo BookInfo { get; set; }

        #endregion
    }
}
