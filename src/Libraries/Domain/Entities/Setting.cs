namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("r_moeci_com.setting")]
    public partial class Setting
    {
        public int ID { get; set; }

        public int? Status { get; set; }

        [Required]
        [StringLength(100)]
        public string SetKey { get; set; }

        [Column(TypeName = "text")]
        [StringLength(65535)]
        public string SetValue { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

        [Column(TypeName = "text")]
        [StringLength(65535)]
        public string Remark { get; set; }
    }
}
