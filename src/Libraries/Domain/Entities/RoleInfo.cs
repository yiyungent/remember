namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("r_moeci_com.roleinfo")]
    public partial class RoleInfo
    {
        public int ID { get; set; }

        public int? Status { get; set; }

        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        public bool? IsLog { get; set; }

        [StringLength(255)]
        public string Remark { get; set; }
    }
}
