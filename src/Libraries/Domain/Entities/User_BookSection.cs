namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// 实体类：学习者-视频课件
    /// </summary>
    public partial class User_BookSection : BaseEntity
    {
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// 最后访问IP
        /// </summary>
        [StringLength(20)]
        public string LastAccessIp { get; set; }

        /// <summary>
        /// 最近阅读位置
        /// 阅读在第几个字符
        /// </summary>
        public long LastViewAt { get; set; }

        /// <summary>
        /// 最近阅读时间
        /// </summary>
        public DateTime LastViewTime { get; set; }

        /// <summary>
        /// 阅读进度-最远的阅读位置
        /// 不会因为重读，而丢失进度
        /// </summary>
        public long ProgressAt { get; set; }

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
        /// 阅读者
        /// </summary>
        [ForeignKey("Reader")]
        public int ReaderId { get; set; }
        [ForeignKey("ReaderId")]
        public virtual UserInfo Reader { get; set; }

        /// <summary>
        /// 文库章节
        /// </summary>
        [ForeignKey("BookSection")]
        public int BookSectionId { get; set; }
        [ForeignKey("BookSectionId")]
        public virtual BookSection BookSection { get; set; }

        #endregion

        #region Helpers

        /// <summary>
        /// 进度百分比
        /// 为1则已完成
        /// </summary>
        [NotMapped]
        public float Percent
        {
            get
            {
                float percent = 0;
                if (this.BookSection != null && this.BookSection.Content.Length != 0)
                {
                    percent = (float)ProgressAt / (float)this.BookSection.Content.Length;
                }

                return percent;
            }
        }

        #endregion
    }
}
