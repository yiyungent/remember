namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// ÊÕ²Ø¼Ð-¿Î³Ì
    /// </summary>
    public partial class Favorite_CourseBox : BaseEntity
    {
        [Key]
        public int ID { get; set; }

        public DateTime? CreateTime { get; set; }

        #region Relationships

        [ForeignKey("Favorite")]
        public int? FavoriteId { get; set; }
        [ForeignKey("FavoriteId")]
        public virtual Favorite Favorite { get; set; }

        [ForeignKey("CourseBox")]
        public int? CourseBoxId { get; set; }
        [ForeignKey("CourseBoxId")]
        public virtual CourseBox CourseBox { get; set; }

        #endregion
    }
}
