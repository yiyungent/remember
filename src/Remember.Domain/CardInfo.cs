using Castle.ActiveRecord;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Remember.Domain
{
    /// <summary>
    /// 实体类：知识卡片
    /// </summary>
    [ActiveRecord]
    public class CardInfo : BaseEntity<CardInfo>
    {
        /// <summary>
        /// 内容
        /// </summary>
        [Display(Name = "内容")]
        [Property(Length = 3000, NotNull = true)]
        public string Content { get; set; }

        /// <summary>
        /// 所在 CardBox
        ///     多对一
        /// </summary>
        [Display(Name = "所在 CardBox")]
        [BelongsTo(Column = "CardBoxId")]
        public CardBox CardBox { get; set; }
    }
}
