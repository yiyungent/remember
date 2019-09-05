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
    public class CourseBoxTable : BaseEntity<CourseBoxTable>
    {
        /// <summary>
        /// 读者
        /// </summary>
        [BelongsTo(Column = "ReaderId")]
        public UserInfo Reader { get; set; }

        /// <summary>
        /// 课程盒
        /// </summary>
        [BelongsTo(Column = "CourseBoxId")]
        public CourseBox CourseBox { get; set; }

        /// <summary>
        /// 加入学习的时间
        /// </summary>
        [Property(NotNull = false)]
        public DateTime JoinTime { get; set; }

        /// <summary>
        /// 学习进度，存储json
        /// {
        ///     CourseInfoId: 2, // 最近学习到的知识卡ID
        ///     PlayTime: 2444 // 若为视频，则为上次视频播放时间
        /// }
        /// </summary>
        [Property(Length = 300, NotNull = false)]
        public string StudyProgress { get; set; }
    }
}
