namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// 追随者A-被追随/关注者B
    /// <para>A关注B -> A成为了B的粉丝</para>
    /// </summary>
    public partial class Follower_Followed : BaseEntity
    {
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// 创建/关注时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 删除时间：为null，则未删除
        /// </summary>
        public DateTime? DeletedAt { get; set; }

        /// <summary>
        /// 是否被删除
        /// </summary>
        public bool IsDeleted { get; set; }

        #region Relationships

        /// <summary>
        /// 追随者A
        /// </summary>
        [ForeignKey("Follower")]
        public int FollowerId { get; set; }
        [ForeignKey("FollowerId")]
        public virtual UserInfo Follower { get; set; }

        /// <summary>
        /// 被关注/追随的人B
        /// </summary>
        [ForeignKey("Followed")]
        public int FollowedId { get; set; } 
        [ForeignKey("FollowedId")]
        public virtual UserInfo Followed { get; set; }

        #endregion
    }
}
