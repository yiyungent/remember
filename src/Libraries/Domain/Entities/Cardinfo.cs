namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CardInfo : BaseEntity
    {
        public int ID { get; set; }

        [Column(TypeName = "text")]
        [Required]
        [StringLength(65535)]
        public string Content { get; set; }

        #region Relationships

        [ForeignKey("CardBox")]
        public int? CardBoxId { get; set; }
        [ForeignKey("CardBoxId")]
        public virtual CardBox CardBox { get; set; }

        #endregion
    }
}
