namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Favorite_CardBox : BaseEntity
    {
        public int ID { get; set; }

        public DateTime? CreateTime { get; set; }

        [ForeignKey("Cardbox")]
        public int CardBoxId { get; set; }
        [ForeignKey("CardBoxId")]
        public virtual CardBox Cardbox { get; set; }

        [ForeignKey("Favorite")]
        public int FavoriteId { get; set; }
        [ForeignKey("FavoriteId")]
        public virtual Favorite Favorite { get; set; }
    }
}
