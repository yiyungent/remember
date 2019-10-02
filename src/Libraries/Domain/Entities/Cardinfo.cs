namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// 实体类：知识卡片
    /// </summary>
    public partial class CardInfo : BaseEntity
    {
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [Column(TypeName = "text")]
        [Required]
        [StringLength(30)]
        public string Content { get; set; }

        #region Relationships

        /// <summary>
        /// 所在 CardBox
        ///     多对一
        /// </summary>
        [ForeignKey("CardBox")]
        public int? CardBoxId { get; set; }
        /// <summary>
        /// 所在 CardBox
        ///     多对一
        /// </summary>
        [ForeignKey("CardBoxId")]
        public virtual CardBox CardBox { get; set; }

        #endregion
    }
}
