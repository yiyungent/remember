namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("r_moeci_com.favorite_coursebox")]
    public partial class Favorite_CourseBox
    {
        public int ID { get; set; }

        public int? Status { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? FavoriteId { get; set; }

        public int? CourseBoxId { get; set; }
    }
}
