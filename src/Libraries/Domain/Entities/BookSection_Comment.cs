namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// 实体类：文库章节-评论
    /// </summary>
    public partial class BookSection_Comment : BaseEntity
    {
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// 删除时间：为null，则未删除
        /// </summary>
        public DateTime? DeletedAt { get; set; }

        /// <summary>
        /// 是否被删除
        /// </summary>
        public bool IsDeleted { get; set; }

        #region Relationships

        [ForeignKey("BookSection")]
        public int BookSectionId { get; set; }
        [ForeignKey("BookSectionId")]
        public virtual BookSection BookSection { get; set; }

        [ForeignKey("Comment")]
        public int CommentId { get; set; }
        [ForeignKey("CommentId")]
        public virtual Comment Comment { get; set; }

        #endregion
    }
}
