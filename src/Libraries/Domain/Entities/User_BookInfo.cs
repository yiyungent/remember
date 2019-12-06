namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// 实体类：用户-文库
    /// </summary>
    public partial class User_BookInfo : BaseEntity
    {
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// 创建时间-用户开始阅读此文库的时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 此用户在此文库总阅读时间: 花费时间
        /// 秒
        /// </summary>
        public long SpendTime { get; set; }

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
        /// 最近阅读章节
        /// </summary>
        [ForeignKey("LastViewSection")]
        public int? LastViewSectionId { get; set; }
        [ForeignKey("LastViewSectionId")]
        public virtual BookSection LastViewSection { get; set; }

        /// <summary>
        /// 用户-阅读者
        /// </summary>
        [ForeignKey("Reader")]
        public int ReaderId { get; set; }
        [ForeignKey("Reader")]
        public virtual UserInfo Reader { get; set; }

        /// <summary>
        /// 文库
        /// </summary>
        [ForeignKey("BookInfo")]
        public int BookInfoId { get; set; }
        [ForeignKey("BookInfoId")]
        public virtual BookInfo BookInfo { get; set; }

        #endregion
    }
}
