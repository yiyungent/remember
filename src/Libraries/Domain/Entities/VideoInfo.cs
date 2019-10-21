namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// 视频课件
    /// </summary>
    public partial class VideoInfo : BaseEntity
    {
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [StringLength(100)]
        [Column(TypeName = "text")]
        public string Title { get; set; }

        /// <summary>
        /// 视频: 视频url
        /// </summary>
        [Column(TypeName = "text")]
        [Required]
        [StringLength(2000)]
        public string PlayUrl { get; set; }

        /// <summary>
        /// 视频：字幕url
        /// </summary>
        [Column(TypeName = "text")]
        [StringLength(100)]
        public string SubTitleUrl { get; set; }

        /// <summary>
        /// 持续时间
        /// 毫秒
        /// 视频：此课件总需播放时间
        /// 帖子：阅读完它需时间
        /// </summary>
        public long Duration { get; set; }

        /// <summary>
        /// 视频文件大小
        /// 字节B
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        /// 排序码
        /// 此视频属于课程的第几集/页
        /// </summary>
        public int Page { get; set; }

        #region Relationships

        /// <summary>
        /// 所在 CourseBox
        ///     多对一
        /// </summary>
        [ForeignKey("CourseBox")]
        public int? CourseBoxId { get; set; }
        [ForeignKey("CourseBoxId")]
        public virtual CourseBox CourseBox { get; set; }

        #endregion
    }
}
