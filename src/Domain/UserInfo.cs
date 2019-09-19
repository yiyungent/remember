using Castle.ActiveRecord;
using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    /// <summary>
    /// 实体类：用户
    /// </summary>
    [ActiveRecord]
    [Serializable]
    public partial class UserInfo : BaseEntity<UserInfo>
    {
        #region Properties

        /// <summary>
        /// 昵称/展示名(不唯一，可改，不可作为登录使用)
        /// </summary>
        [Display(Name = "展示名")]
        [Property(Length = 30, NotNull = true)]
        public string Name { get; set; }

        /// <summary>
        /// 用户头像Url地址
        /// </summary>
        [Display(Name = "头像")]
        [Property(Length = 50, NotNull = false)]
        public string Avatar { get; set; }

        /// <summary>
        /// 邮箱(唯一，可改，可作为登录使用)
        /// </summary>
        [Display(Name = "邮箱")]
        [Property(Length = 50, NotNull = false, Unique = true)]
        public string Email { get; set; }

        /// <summary>
        /// 手机号(唯一，可改，可作为登录使用)
        /// </summary>
        [Display(Name = "手机号")]
        [Property(Length = 50, NotNull = false, Unique = true)]
        public string Phone { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [Display(Name = "描述")]
        [Property(Length = 300, NotNull = false, Default = "")]
        public string Description { get; set; }

        /// <summary>
        /// 硬币数
        /// </summary>
        [Property(Length = 3000, NotNull = false)]
        public long Coin { get; set; }

        /// <summary>
        /// 注册时间
        /// </summary>
        [Display(Name = "注册时间")]
        [Property(NotNull = true)]
        public DateTime RegTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Property(Length = 300, NotNull = false)]
        public string Remark { get; set; }

        #endregion

        #region Relationships

        ///// <summary>
        ///// 用户的收藏夹列表
        ///// </summary>
        //[HasMany(ColumnKey = "CreatorId")]
        //public IList<Favorite> FavoriteList { get; set; }

        #endregion

    }
}