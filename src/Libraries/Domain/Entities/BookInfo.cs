namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;

    /// <summary>
    /// 文库
    /// </summary>
    public partial class BookInfo : BaseEntity
    {
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// 课程名
        /// </summary>
        [Required]
        [Column(TypeName = "text")]
        [StringLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [Column(TypeName = "text")]
        [StringLength(2000)]
        public string Description { get; set; }

        /// <summary>
        /// 封面图
        /// </summary>
        [StringLength(100)]
        [Column(TypeName = "text")]
        public string PicUrl { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 最近更新时间
        /// </summary>
        public DateTime LastUpdateTime { get; set; }

        /// <summary>
        /// 是否公开
        /// </summary>
        public bool IsOpen { get; set; }

        public int LikeNum { get; set; }

        public int DislikeNum { get; set; }

        public int CommentNum { get; set; }

        public int ShareNum { get; set; }

        #region Relationships

        /// <summary>
        /// 文库的创建者
        ///     多对一
        /// </summary>
        [ForeignKey("Creator")]
        public int CreatorId { get; set; }
        [ForeignKey("CreatorId")]
        public virtual UserInfo Creator { get; set; }

        /// <summary>
        /// 课程包含的视频的列表
        ///     一对多
        /// </summary>
        public virtual ICollection<BookSection> BookSections { get; set; }

        /// <summary>
        /// 属于哪些收藏夹
        /// </summary>
        public virtual ICollection<Favorite_BookInfo> Favorite_BookInfos { get; set; }

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

        /// <summary>
        /// 学习此课程总需时间
        /// </summary>
        [NotMapped]
        public long Duration
        {
            get
            {
                long duration = 0;
                if (this.BookSections != null && this.BookSections.Count >= 1)
                {
                    duration = this.BookSections.Select(m => m.Duration).Sum();
                }

                return duration;
            }
        }

        [NotMapped]
        public IList<Favorite> Favorites
        {
            get
            {
                IList<Favorite> favorites = new List<Favorite>();
                if (this.Favorite_BookInfos != null && this.Favorite_BookInfos.Count >= 1)
                {
                    favorites = this.Favorite_BookInfos.Select(m => m.Favorite).ToList();
                }

                return favorites;
            }
        }

        #endregion
    }
}
