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
        /// <summary>
        /// 内容
        /// </summary>
        [Display(Name = "内容")]
        [Property(Length = 2200, NotNull = false)]
        public string Content { get; set; }

        #region Relationships

        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        [Property]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 最近更新
        /// </summary>
        [Display(Name = "最近更新")]
        [Property]
        public DateTime LastUpdateTime { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        [Display(Name = "作者")]
        [BelongsTo(Column = "AuthorId", NotNull = true)]
        public UserInfo Author { get; set; }

        /// <summary>
        /// 回复评论（父级评论）
        /// </summary>
        [BelongsTo(Column = "ReplyCommentId", NotNull = false)]
        public Comment ReplyComment { get; set; }

        /// <summary>
        /// 评论类型
        /// </summary>
        [Property]
        public CommentType? CommentType { get; set; }

        /// <summary>
        /// 评论对象ID
        ///     例如评论某卡片，即是此被评论卡片的ID
        /// </summary>
        [Property(NotNull = true)]
        public int CommentTargetId { get; set; }

        #endregion

    }

    /// <summary>
    /// 评论类型（属于在何处的评论）
    /// </summary>
    public enum CommentType
    {
        CardInfo = 1,
        CardBox = 2
    }
}
