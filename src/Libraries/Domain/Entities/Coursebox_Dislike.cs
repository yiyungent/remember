namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// 课程-踩的人
    /// </summary>
    public partial class CourseBox_Dislike : BaseEntity
    {
        [Key]
        public int ID { get; set; }

        public DateTime? CreateTime { get; set; }

        #region Relationships

        /// <summary>
        /// 课程
        /// </summary>
        [ForeignKey("CourseBox")]
        public int? CourseBoxId { get; set; }
        [ForeignKey("CourseBoxId")]
        public virtual CourseBox CourseBox { get; set; }

        /// <summary>
        /// 踩此门课程的人
        /// </summary>
        [ForeignKey("UserInfo")]
        public int? UserInfoId { get; set; }
        [ForeignKey("UserInfoId")]
        public virtual UserInfo UserInfo { get; set; }

        #endregion
    }
}
