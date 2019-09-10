using Castle.ActiveRecord;
using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    [ActiveRecord]
    public class CourseBox : BaseEntity<CourseBox>
    {
        /// <summary>
        /// 课程名
        /// </summary>
        [Display(Name = "课程名")]
        [Property(Length = 30, NotNull = true)]
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [Display(Name = "描述")]
        [Property(Length = 500, NotNull = false)]
        public string Description { get; set; }

        /// <summary>
        /// 封面图
        /// </summary>
        [Property(Length = 100, NotNull = false)]
        public string PicUrl { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Property(NotNull = false)]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 最近更新时间
        /// </summary>
        [Property(NotNull = false)]
        public DateTime LastUpdateTime { get; set; }

        /// <summary>
        /// 是否公开
        /// </summary>
        [Property(NotNull = false)]
        public bool IsOpen { get; set; }

        /// <summary>
        /// 有效日期 - 开始时间
        /// </summary>
        [Property(NotNull = false)]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 有效日期 - 结束时间
        /// </summary>
        [Property(NotNull = false)]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 有效学习天数
        /// </summary>
        [Property(NotNull = false)]
        public int LearnDay { get; set; }

        #region Relationships

        /// <summary>
        /// 课程盒包含的课程内容的列表
        ///     一对多
        /// </summary>
        [Display(Name = "包含课程内容的列表")]
        [HasMany(ColumnKey = "CourseBoxId")]
        public IList<CourseInfo> CourseInfoList { get; set; }

        /// <summary>
        /// 课程的创建者
        ///     多对一
        /// </summary>
        [Display(Name = "创建者")]
        [BelongsTo(Column = "CreatorId")]
        public UserInfo Creator { get; set; }

        /// <summary>
        /// 课程盒表 列表
        /// </summary>
        [HasMany(ColumnKey = "CourseBoxId")]
        public IList<CourseBoxTable> CourseBoxTableList { get; set; }

        #endregion
    }
}
