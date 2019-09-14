using Castle.ActiveRecord;
using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Domain
{
    /// <summary>
    /// 实体类：课件-评论
    /// </summary>
    [ActiveRecord]
    [Serializable]
    public class CourseInfo_Comment : BaseEntity<CourseInfo_Comment>
    {
        #region Relationships

        [BelongsTo(Column = "CourseInfoId")]
        public CourseInfo CourseInfo { get; set; }

        [BelongsTo(Column = "CommentId")]
        public Comment Comment { get; set; }

        #endregion
    }
}
