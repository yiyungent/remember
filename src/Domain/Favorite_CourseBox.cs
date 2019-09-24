using Castle.ActiveRecord;
using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    /// <summary>
    /// 收藏夹-课程
    /// </summary>
    [ActiveRecord]
    [Serializable]
    public class Favorite_CourseBox : BaseEntity<Favorite_CourseBox>
    {
        #region Relationships

        [BelongsTo(Column = "FavoriteId")]
        public Favorite Favorite { get; set; }

        [BelongsTo(Column = "CourseBoxId")]
        public CourseBox CourseBox { get; set; }

        /// <summary>
        /// 收藏此课程的时间
        /// </summary>
        [Property]
        public DateTime CreateTime { get; set; }

        #endregion
    }
}
