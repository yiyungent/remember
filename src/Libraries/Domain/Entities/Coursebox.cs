namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;

    public partial class CourseBox : BaseEntity
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
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 最近更新时间
        /// </summary>
        public DateTime? LastUpdateTime { get; set; }

        /// <summary>
        /// 是否公开
        /// </summary>
        public bool? IsOpen { get; set; }

        /// <summary>
        /// 有效日期 - 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 有效日期 - 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        public int? LikeNum { get; set; }

        public int? DislikeNum { get; set; }

        public int? CommentNum { get; set; }

        public int? ShareNum { get; set; }

        #region Relationships

        /// <summary>
        /// 课程的创建者
        ///     多对一
        /// </summary>
        [ForeignKey("Creator")]
        public int? CreatorId { get; set; }
        [ForeignKey("CreatorId")]
        public virtual UserInfo Creator { get; set; }

        /// <summary>
        /// 课程包含的视频的列表
        ///     一对多
        /// </summary>
        public virtual ICollection<VideoInfo> VideoInfos { get; set; }

        /// <summary>
        /// 属于哪些收藏夹
        /// </summary>
        public virtual ICollection<Favorite_CourseBox> Favorite_CourseBoxes { get; set; }

        #endregion

        #region Helpers

        /// <summary>
        /// 学习此课程总需时间
        /// </summary>
        public long Duration
        {
            get
            {
                long duration = 0;
                if (this.VideoInfos != null && this.VideoInfos.Count >= 1)
                {
                    duration = this.VideoInfos.Select(m => m.Duration ?? 0).Sum();
                }

                return duration;
            }
        }

        #endregion
    }
}
