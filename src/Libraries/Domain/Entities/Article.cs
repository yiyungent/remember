namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;

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
        /// 描述/摘要
        /// </summary>
        [Column(TypeName = "text")]
        [StringLength(1000)]
        public string Description { get; set; }

        /// <summary>
        /// 封面图
        /// </summary>
        [Column(TypeName = "text")]
        [StringLength(1000)]
        public string PicUrl { get; set; }

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
        /// 赞数
        /// </summary>
        public int LikeNum { get; set; }

        /// <summary>
        /// 踩数
        /// </summary>
        public int DislikeNum { get; set; }

        /// <summary>
        /// 分享数
        /// </summary>
        public int ShareNum { get; set; }

        /// <summary>
        /// 评论数
        /// </summary>
        public int CommentNum { get; set; }

        /// <summary>
        /// 文章状态
        /// </summary>
        public AStatus ArticleStatus { get; set; }

        /// <summary>
        /// 评论状态
        /// </summary>
        public CStatus CommentStatus { get; set; }

        /// <summary>
        /// 可见程度
        /// </summary>
        public OStatus OpenStatus { get; set; }

        #region Relationships

        /// <summary>
        /// 作者
        /// </summary>
        [ForeignKey("Author")]
        public int AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        public virtual UserInfo Author { get; set; }

        /// <summary>
        /// 属于哪些收藏夹
        /// </summary>
        public virtual ICollection<Favorite_Article> Favorite_Articles { get; set; }

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

        [NotMapped]
        public IList<Favorite> Favorites
        {
            get
            {
                IList<Favorite> favorites = new List<Favorite>();
                if (this.Favorite_Articles != null && this.Favorite_Articles.Count >= 1)
                {
                    favorites = this.Favorite_Articles.Select(m => m.Favorite).ToList();
                }

                return favorites;
            }
        }



        #endregion

        public enum AStatus
        {
            /// <summary>
            /// 被发布
            /// </summary>
            Publish = 0,

            /// <summary>
            /// 编辑中（草稿状态）
            /// </summary>
            Draft = 1,
        }

        public enum CStatus
        {
            /// <summary>
            /// 允许评论
            /// </summary>
            Open,

            /// <summary>
            /// 关闭评论
            /// </summary>
            Closed,
        }

        public enum OStatus
        {
            /// <summary>
            /// 所有人可见
            /// </summary>
            All,

            /// <summary>
            /// 仅自己可见
            /// </summary>
            Self
        }
    }
}
