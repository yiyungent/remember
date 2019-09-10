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
    public class CourseBoxComment : BaseEntity<CourseBoxComment>
    {
        /// <summary>
        /// 内容
        /// </summary>
        [Display(Name = "内容")]
        [Property(Length = 2200, NotNull = false)]
        public string Content { get; set; }

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


        #region Relationships

        /// <summary>
        /// 作者
        /// </summary>
        [Display(Name = "作者")]
        [BelongsTo(Column = "AuthorId", NotNull = true)]
        public UserInfo Author { get; set; }

        /// <summary>
        /// 评论的课程盒
        /// </summary>
        [BelongsTo(Column = "CourseBoxId", NotNull = true)]
        public CourseBox CourseBox { get; set; }

        #endregion
    }
}
