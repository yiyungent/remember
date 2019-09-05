using Castle.ActiveRecord;
using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    [ActiveRecord]
    public class Course : BaseEntity<Course>
    {
        /// <summary>
        /// 课程名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 课程描述
        /// </summary>
        public string Description { get; set; }

        #region Relationships

        /// <summary>
        /// 课程所包括的具体课程贴列表
        ///     一对多
        /// </summary>
        [HasMany(ColumnKey = "CourseId")]
        public IList<CourseSection> CourseSectionList { get; set; }

        #endregion
    }
}
