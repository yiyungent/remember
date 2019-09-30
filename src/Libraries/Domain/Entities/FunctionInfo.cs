namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class FunctionInfo : BaseEntity
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string AuthKey { get; set; }

        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        [Column(TypeName = "text")]
        [StringLength(65535)]
        public string Remark { get; set; }

        #region Relationships

        [ForeignKey("Sys_Menu")]
        public int? MenuId { get; set; }
        [ForeignKey("MenuId")]
        public virtual Sys_Menu Sys_Menu { get; set; } 

        #endregion
    }
}
