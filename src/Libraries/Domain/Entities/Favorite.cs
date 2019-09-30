namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Favorite : BaseEntity
    {
        public int ID { get; set; }

        [Column(TypeName = "text")]
        [StringLength(65535)]
        public string Name { get; set; }

        [Column(TypeName = "text")]
        [StringLength(65535)]
        public string Description { get; set; }

        public bool? IsOpen { get; set; }

        public DateTime? CreateTime { get; set; }

        #region Relationships

        [ForeignKey("Creator")]
        public int? CreatorId { get; set; }
        [ForeignKey("CreatorId")]
        public UserInfo Creator { get; set; } 

        #endregion
    }
}
