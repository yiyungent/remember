using Castle.ActiveRecord;
using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    /// <summary>
    /// 课程-踩的人
    /// </summary>
    [ActiveRecord]
    public class CourseBox_Dislike : BaseEntity<CourseBox_Dislike>
    {
        #region Properties

        /// <summary>
        /// 创建时间
        /// </summary>
        [Property]
        public DateTime CreateTime { get; set; }

        #endregion

        #region Relationships

        /// <summary>
        /// 课程
        /// </summary>
        [BelongsTo(Column = "CourseBoxId")]
        public CourseBox CourseBox { get; set; }

        /// <summary>
        /// 踩此门课程的人
        /// </summary>
        [BelongsTo(Column = "UserInfoId")]
        public UserInfo UserInfo { get; set; }

        #endregion
    }
}
