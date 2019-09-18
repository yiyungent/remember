using Castle.ActiveRecord;
using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Domain
{
    [ActiveRecord]
    [Serializable]
    public class CourseBox : BaseEntity<CourseBox>
    {
        #region Properties

        /// <summary>
        /// 课程名
        /// </summary>
        [Property(Length = 200, NotNull = true)]
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [Property(Length = 500, NotNull = false)]
        public string Description { get; set; }

        /// <summary>
        /// 封面图
        /// </summary>
        [Property(Length = 100, NotNull = false)]
        public string PicUrl { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Property(NotNull = false)]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 最近更新时间
        /// </summary>
        [Property(NotNull = false)]
        public DateTime LastUpdateTime { get; set; }

        /// <summary>
        /// 是否公开
        /// </summary>
        [Property(NotNull = false)]
        public bool IsOpen { get; set; }

        /// <summary>
        /// 有效日期 - 开始时间
        /// </summary>
        [Property(NotNull = false)]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 有效日期 - 结束时间
        /// </summary>
        [Property(NotNull = false)]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 有效学习天数
        /// </summary>
        [Property(NotNull = false)]
        public int LearnDay { get; set; }

        [Property(NotNull = false)]
        public int LikeNum { get; set; }

        [Property(NotNull = false)]
        public int DislikeNum { get; set; }

        [Property(NotNull = false)]
        public int CommentNum { get; set; }

        [Property(NotNull = false)]
        public int ShareNum { get; set; }

        /// <summary>
        /// 学习此课程总需时间
        /// </summary>
        public long Duration
        {
            get
            {
                long duration = 0;
                if (this.VideoInfos != null && this.VideoInfos.Count >= 1)
                {
                    duration = this.VideoInfos.Select(m => m.Duration).Sum();
                }

                return duration;
            }
        }

        #endregion

        #region Relationships

        /// <summary>
        /// 课程包含的视频的列表
        ///     一对多
        /// </summary>
        [HasMany(ColumnKey = "CourseBoxId")]
        public IList<VideoInfo> VideoInfos { get; set; }

        /// <summary>
        /// 课程的创建者
        ///     多对一
        /// </summary>
        [BelongsTo(Column = "CreatorId")]
        public UserInfo Creator { get; set; }

        /// <summary>
        /// 属于哪些收藏夹
        /// </summary>
        [HasAndBelongsToMany(ColumnKey = "CourseBoxId", ColumnRef = "FavoriteId", Table = "Favorite_CourseBox")]
        public IList<Favorite> FavoriteList { get; set; }

        #endregion
    }
}
