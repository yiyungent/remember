namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CardBox : BaseEntity
    {
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        [Column(TypeName = "text")]
        [StringLength(65535)]
        public string Description { get; set; }

        [StringLength(100)]
        public string PicUrl { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? LastUpdateTime { get; set; }

        public bool? IsOpen { get; set; }

        #region Relationships

        [ForeignKey("Creator")]
        public int? CreatorId { get; set; }
        [ForeignKey("CreatorId")]
        public virtual UserInfo Creator { get; set; }

        #endregion
    }
}
