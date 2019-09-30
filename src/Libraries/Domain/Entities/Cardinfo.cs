namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("r_moeci_com.cardinfo")]
    public partial class Cardinfo
    {
        public int ID { get; set; }

        public int? Status { get; set; }

        [Column(TypeName = "text")]
        [Required]
        [StringLength(65535)]
        public string Content { get; set; }

        public int? CardBoxId { get; set; }
    }
}
