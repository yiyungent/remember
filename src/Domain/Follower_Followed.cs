using Castle.ActiveRecord;
using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    /// <summary>
    /// 追随者A-被追随/关注者B
    /// <para>A关注B -> A成为了B的粉丝</para>
    /// </summary>
    [ActiveRecord]
    public class Follower_Followed : BaseEntity<Follower_Followed>
    {
        #region Properties

        /// <summary>
        /// 创建/关注时间
        /// </summary>
        [Property]
        public DateTime CreateTime { get; set; }

        #endregion

        #region Relationships

        /// <summary>
        /// 追随者A
        /// </summary>
        [BelongsTo(Column = "FollowerId")]
        public UserInfo Follower { get; set; }

        /// <summary>
        /// 被关注/追随的人B
        /// </summary>
        [BelongsTo(Column = "FollowedId")]
        public UserInfo Followed { get; set; }

        #endregion
    }
}
