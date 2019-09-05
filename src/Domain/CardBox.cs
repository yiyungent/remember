using Castle.ActiveRecord;
using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    /// <summary>
    /// 实体类：卡片盒，内有很多卡片
    /// </summary>
    [ActiveRecord]
    [Serializable]
    public class CardBox : BaseEntity<CardBox>
    {
        /// <summary>
        /// 盒名
        /// </summary>
        [Display(Name = "盒名")]
        [Property(Length = 30, NotNull = true)]
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [Display(Name = "描述")]
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

        #region Relationships

        /// <summary>
        /// 卡片盒包含的卡片的列表
        ///     一对多
        /// </summary>
        [Display(Name = "包含卡片的列表")]
        [HasMany(ColumnKey = "CardBoxId")]
        public IList<CardInfo> CardInfoList { get; set; }

        /// <summary>
        /// 卡片盒的创建者
        ///     多对一
        /// </summary>
        [Display(Name = "创建者")]
        [BelongsTo(Column = "CreatorId")]
        public UserInfo Creator { get; set; }

        ///// <summary>
        ///// 此卡片盒的读者 列表
        /////     多对多
        /////         一个卡片盒可供多人阅读，一人可阅读多个卡片盒
        ///// </summary>
        //[Display(Name = "读者列表")]
        //[HasAndBelongsToMany(Table = "User_CardBox_ForRead", ColumnKey = "CardBoxId", ColumnRef = "UserId")]
        //public IList<UserInfo> ReaderList { get; set; }

        #endregion
    }

}
