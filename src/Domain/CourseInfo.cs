using Castle.ActiveRecord;
using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    /// <summary>
    /// 课件
    /// </summary>
    [ActiveRecord]
    public class CourseInfo : BaseEntity<CourseInfo>
    {
        #region Properities

        /// <summary>
        /// 标题
        /// </summary>
        [Property(Length = 200, NotNull = false)]
        public string Title { get; set; }

        /// <summary>
        /// 内容
        /// 视频:视频url
        /// 富文本贴：内容
        /// </summary>
        [Property(Length = 3000, NotNull = true)]
        public string Content { get; set; }

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
        /// 第N页
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// 课程内容类型
        /// </summary>
        [Property(NotNull = false)]
        public CourseInfoType CourseInfoType { get; set; }

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

    /// <summary>
    /// 课程内容类型
    /// </summary>
    public enum CourseInfoType
    {
        /// <summary>
        /// 视频
        /// </summary>
        Video = 2,

        /// <summary>
        /// <summary>
        /// 富文本贴
        /// </summary>
        RichText = 4
    }
}
