using Castle.ActiveRecord;
using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    [ActiveRecord]
    public class Article : BaseEntity<Article>
    {
        /// <summary>
        /// 标题
        /// </summary>
        [Display(Name = "标题")]
        [Property(Length = 30, NotNull = false)]
        public string Title { get; set; }

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

        /// <summary>
        /// 作者
        /// </summary>
        [Display(Name = "作者")]
        [BelongsTo(Column = "AuthorId")]
        public UserInfo Author { get; set; }

        /// <summary>
        /// 自定义Url
        /// </summary>
        [Display(Name = "自定义Url")]
        [Property(Length = 30, NotNull = false)]
        public string CustomUrl { get; set; }
    }
}
