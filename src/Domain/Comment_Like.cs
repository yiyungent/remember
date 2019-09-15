using Castle.ActiveRecord;
using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    /// <summary>
    /// 评论-赞的人
    /// </summary>
    [ActiveRecord]
    public class Comment_Like : BaseEntity<Comment_Like>
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
        /// 评论
        /// </summary>
        [BelongsTo(Column = "CommentId")]
        public Comment Comment { get; set; }

        /// <summary>
        /// 赞此评论的人
        /// </summary>
        [BelongsTo(Column = "UserInfoId")]
        public UserInfo UserInfo { get; set; }

        #endregion
    }
}
