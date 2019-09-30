namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("r_moeci_com.favorite")]
    public partial class Favorite
    {
        public int ID { get; set; }

        public int? Status { get; set; }

        [Column(TypeName = "text")]
        [StringLength(65535)]
        public string Name { get; set; }

        [Column(TypeName = "text")]
        [StringLength(65535)]
        public string Description { get; set; }

        public bool? IsOpen { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? CreatorId { get; set; }
    }
}
