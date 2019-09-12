using Castle.ActiveRecord;
using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    [ActiveRecord]
    public class CourseInfo : BaseEntity<CourseInfo>
    {
        /// <summary>
        /// 标题
        /// </summary>
        [Property(Length = 30, NotNull = false)]
        public string Title { get; set; }

        /// <summary>
        /// 内容
        /// 视频:视频url
        /// 富文本贴：内容
        /// </summary>
        [Display(Name = "内容")]
        [Property(Length = 3000, NotNull = true)]
        public string Content { get; set; }

        /// <summary>
        /// 课程内容类型
        /// </summary>
        [Property(NotNull = false)]
        public CourseInfoType CourseInfoType { get; set; }

        #region Relationships

        /// <summary>
        /// 父级CourseInfo
        /// </summary>
        [BelongsTo(Column = "ParentId")]
        public CourseInfo Parent { get; set; }

        /// <summary>
        /// 所在 CourseBox
        ///     多对一
        /// </summary>
        [Display(Name = "所在 CourseBox")]
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
