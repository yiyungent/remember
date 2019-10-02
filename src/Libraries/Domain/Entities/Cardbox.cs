namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// 实体类：卡片盒，内有很多卡片
    /// </summary>
    public partial class CardBox : BaseEntity
    {
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// 盒名
        /// </summary>
        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [Column(TypeName = "text")]
        [StringLength(30)]
        public string Description { get; set; }

        /// <summary>
        /// 封面图
        /// </summary>
        [Column(TypeName = "text")]
        [StringLength(30)]
        public string PicUrl { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 最近更新时间
        /// </summary>
        public DateTime? LastUpdateTime { get; set; }

        /// <summary>
        /// 是否公开
        /// </summary>
        public bool? IsOpen { get; set; }

        #region Relationships

        /// <summary>
        /// 卡片盒的创建者
        ///     多对一
        /// </summary>
        [ForeignKey("Creator")]
        public int? CreatorId { get; set; }
        /// <summary>
        /// 卡片盒的创建者
        ///     多对一
        /// </summary>
        [ForeignKey("CreatorId")]
        public virtual UserInfo Creator { get; set; }

        #endregion
    }
}
