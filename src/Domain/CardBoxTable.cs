using Castle.ActiveRecord;
using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    /// <summary>
    /// 实体类：卡片盒表（关系）
    /// </summary>
    [ActiveRecord]
    [Serializable]
    public class CardBoxTable : BaseEntity<CardBoxTable>
    {
        /// <summary>
        /// 读者
        /// </summary>
        [BelongsTo(Column = "ReaderId")]
        public UserInfo Reader { get; set; }

        /// <summary>
        /// 卡片盒
        /// </summary>
        [BelongsTo(Column = "CardBoxId")]
        public CardBox CardBox { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [Property(NotNull = false)]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [Property(NotNull = false)]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 学习进度，存储json
        /// {
        ///     CardInfoId: 2, // 最近学习到的知识卡ID
        ///     PlayTime: 2444 // 若为视频，则为上次视频播放时间
        /// }
        /// </summary>
        [Property(Length = 300, NotNull = false)]
        public string StudyProgress { get; set; }
    }
}
