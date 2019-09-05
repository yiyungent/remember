using Castle.ActiveRecord;
using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    [ActiveRecord]
    public class CourseSection : BaseEntity<CourseSection>
    {
        public string Title { get; set; }

        public string Content { get; set; }

        #region Relationships

        /// <summary>
        /// 所属课程
        /// </summary>
        [Display(Name = "课程")]
        [BelongsTo(Column = "CourseId")]
        public Course Course { get; set; }

        #endregion
    }
}
