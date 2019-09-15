using Castle.ActiveRecord;
using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    [ActiveRecord]
    public class Comment : BaseEntity<Comment>
    {
        #region Properities

        /// <summary>
        /// 内容
        /// </summary>
        [Property(Length = 2200, NotNull = false)]
        public string Content { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Property]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 最近更新时间
        /// </summary>
        [Property]
        public DateTime LastUpdateTime { get; set; }

        #endregion

        #region Relationships

        /// <summary>
        /// 作者
        /// </summary>
        [BelongsTo(Column = "AuthorId", NotNull = true)]
        public UserInfo Author { get; set; }

        /// <summary>
        /// 回复的评论
        /// </summary>
        [BelongsTo(Column = "ReplyCommentId", NotNull = false)]
        public Comment Reply { get; set; }

        #region 废弃
        ///// <summary>
        ///// 所在课程
        ///// <para>与所在课件二选一，另一个为 null</para>
        ///// </summary>
        //[BelongsTo(Column = "CourseBoxId", NotNull = false)]
        //public CourseBox CourseBox { get; set; }

        ///// <summary>
        ///// 所在课件
        ///// <para>与所在课程二选一，另一个为 null</para>
        ///// </summary>
        //[BelongsTo(Column = "CourseInfoId", NotNull = false)]
        //public CourseInfo CourseInfo { get; set; } 
        #endregion

        #endregion
    }
}
