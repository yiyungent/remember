using Castle.ActiveRecord;
using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Domain
{
    /// <summary>
    /// 实体类：课程-评论
    /// </summary>
    [ActiveRecord]
    [Serializable]
    public class CourseBox_Comment : BaseEntity<CourseBox_Comment>
    {
        #region Relationships

        [BelongsTo(Column = "CourseBoxId")]
        public CourseBox CourseBox { get; set; }

        [BelongsTo(Column = "CommentId")]
        public Comment Comment { get; set; }

        #endregion
    }
}
