namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// 收藏夹
    /// </summary>
    public partial class Favorite : BaseEntity
    {
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// 收藏夹名
        /// </summary>
        [StringLength(30)]
        public string Name { get; set; }

        /// <summary>
        /// 收藏夹描述
        /// </summary>
        [Column(TypeName = "text")]
        [StringLength(500)]
        public string Description { get; set; }

        /// <summary>
        /// 是否公开
        /// </summary>
        public bool? IsOpen { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        #region Relationships

        /// <summary>
        /// 收藏夹的创建者
        /// </summary>
        [ForeignKey("Creator")]
        public int? CreatorId { get; set; }
        [ForeignKey("CreatorId")]
        public virtual UserInfo Creator { get; set; }

        /// <summary>
        /// 收藏的课程列表
        /// </summary>
        public virtual ICollection<Favorite_CourseBox> Favorite_CourseBoxes { get; set; }

        /// <summary>
        /// 收藏的卡片盒列表
        /// </summary>
        public virtual ICollection<Favorite_CardBox> Favorite_CardBoxes { get; set; }

        #endregion
    }
}
