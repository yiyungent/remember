namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Favorite_CourseBox : BaseEntity
    {
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
