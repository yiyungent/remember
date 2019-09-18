using Castle.ActiveRecord;
using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    /// <summary>
    /// 视频课件
    /// </summary>
    [ActiveRecord]
    [Serializable]
    public class VideoInfo : BaseEntity<VideoInfo>
    {
        #region Properities

        /// <summary>
        /// 标题
        /// </summary>
        [Property(Length = 200, NotNull = false)]
        public string Title { get; set; }

        /// <summary>
        /// 视频: 视频url
        /// </summary>
        [Property(Length = 1000, NotNull = true)]
        public string PlayUrl { get; set; }

        /// <summary>
        /// 视频：字幕url
        /// </summary>
        [Property(Length = 1000, NotNull = false)]
        public string SubTitleUrl { get; set; }

        /// <summary>
        /// 持续时间
        /// 毫秒
        /// 视频：此课件总需播放时间
        /// 帖子：阅读完它需时间
        /// </summary>
        [Property(Length = 1000, NotNull = false)]
        public long Duration { get; set; }

        /// <summary>
        /// 排序码
        /// 此视频属于课程的第几集/页
        /// </summary>
        [Property(NotNull = false)]
        public int Page { get; set; }

        #endregion

        #region Relationships

        /// <summary>
        /// 所在 CourseBox
        ///     多对一
        /// </summary>
        [BelongsTo(Column = "CourseBoxId")]
        public CourseBox CourseBox { get; set; }

        #endregion
    }
}
