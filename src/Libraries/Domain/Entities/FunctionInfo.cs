namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("r_moeci_com.functioninfo")]
    public partial class FunctionInfo
    {
        public int ID { get; set; }

        public int? Status { get; set; }

        [Required]
        [StringLength(50)]
        public string AuthKey { get; set; }

        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        [Column(TypeName = "text")]
        [StringLength(65535)]
        public string Remark { get; set; }

        public int? MenuId { get; set; }
    }
}
