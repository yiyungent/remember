using Castle.ActiveRecord;
using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    /// <summary>
    /// 收藏夹
    /// </summary>
    [ActiveRecord]
    [Serializable]
    public class Favorite : BaseEntity<Favorite>
    {
        #region Properities

        /// <summary>
        /// 收藏夹名
        /// </summary>
        [Property(Length = 500, NotNull = false)]
        public string Name { get; set; }

        /// <summary>
        /// 收藏夹描述
        /// </summary>
        [Property(Length = 1000, NotNull = false)]
        public string Description { get; set; }

        /// <summary>
        /// 是否公开
        /// </summary>
        [Property]
        public bool IsOpen { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Property]
        public DateTime CreateTime { get; set; }

        #endregion

        #region Relationships

        /// <summary>
        /// 收藏夹的创建者
        /// </summary>
        [BelongsTo(Column = "CreatorId")]
        public UserInfo Creator { get; set; }

        /// <summary>
        /// 收藏的课程列表
        /// </summary>
        [HasAndBelongsToMany(ColumnKey = "FavoriteId", ColumnRef = "CourseBoxId", Table = "Favorite_CourseBox")]
        public IList<CourseBox> CourseBoxList { get; set; }

        /// <summary>
        /// 收藏的卡片盒列表
        /// </summary>
        [HasAndBelongsToMany(ColumnKey = "FavoriteId", ColumnRef = "CardBoxId", Table = "Favorite_CardBox")]
        public IList<CardBox> CardBoxList { get; set; }

        #endregion
    }
}
