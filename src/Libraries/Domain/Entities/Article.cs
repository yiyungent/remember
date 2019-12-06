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
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 最近更新
        /// </summary>
        public DateTime LastUpdateTime { get; set; }

        /// <summary>
        /// 自定义Url
        /// </summary>
        [StringLength(1000)]
        [Column(TypeName = "text")]
        public string CustomUrl { get; set; }

        /// <summary>
        /// 文章状态
        /// </summary>
        public AStatus ArticleStatus { get; set; }

        #region Relationships

        /// <summary>
        /// 作者
        /// </summary>
        [ForeignKey("Author")]
        public int AuthorId { get; set; }
        /// <summary>
        /// 作者
        /// </summary>
        [ForeignKey("AuthorId")]
        public virtual UserInfo Author { get; set; }

        /// <summary>
        /// 删除时间：为null，则未删除
        /// </summary>
        public DateTime? DeletedAt { get; set; }

        /// <summary>
        /// 是否被删除
        /// </summary>
        public bool IsDeleted { get; set; }

        #endregion

        #region Helpers

        public enum AStatus
        {
            /// <summary>
            /// 被发布
            /// </summary>
            Published = 0,

            /// <summary>
            /// 编辑中（草稿状态）
            /// </summary>
            Editing = 1,
        }

        #endregion
    }
}
