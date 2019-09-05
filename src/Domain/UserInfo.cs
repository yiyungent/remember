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
        /// <summary>
        /// 展示名(不唯一，可改，不可作为登录使用)
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

        [Display(Name = "描述")]
        [Property(Length = 300, NotNull = false)]
        public string Description { get; set; }

        /// <summary>
        /// 注册时间
        /// </summary>
        [Display(Name = "注册时间")]
        [Property(NotNull = true)]
        public DateTime RegTime { get; set; }

        #region Relationships

        ///// <summary>
        ///// 用户创建的 CardBox 列表
        /////     一对多
        ///// </summary>
        //[Display(Name = "用户创建的 CardBox")]
        //[HasMany(ColumnKey = "CreatorId")]
        //public IList<CardBox> CreateCardBoxList { get; set; }

        /////// <summary>
        /////// 用户阅读的 CardBox 列表    (不包括他创建的，列表中是其它用户创建的 CardBox)
        ///////     多对多
        ///////         一个用户可以阅读多个 CardBox，一个 CardBox 也可以被多个用户阅读(共享)
        /////// </summary>
        ////[Display(Name = "用户阅读的 CardBox")]
        ////[HasAndBelongsToMany(Table = "User_CardBox_ForRead", ColumnKey = "UserId", ColumnRef = "CardBoxId")]
        ////public IList<CardBox> ReadCardBoxList { get; set; }


        //[HasMany(ColumnKey = "ReaderId")]
        //public IList<CardBoxTable> CardBoxTableList { get; set; }

        #endregion

    }
}