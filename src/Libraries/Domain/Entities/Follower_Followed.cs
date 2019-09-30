namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Follower_Followed : BaseEntity
    {
        public int ID { get; set; }

        public DateTime? CreateTime { get; set; }

        #region Relationships

        [ForeignKey("Follower")]
        public int? FollowerId { get; set; }
        [ForeignKey("FollowerId")]
        public virtual UserInfo Follower { get; set; }

        [ForeignKey("Followed")]
        public int? FollowedId { get; set; } 
        [ForeignKey("FollowedId")]
        public virtual UserInfo Followed { get; set; }

        #endregion
    }
}
