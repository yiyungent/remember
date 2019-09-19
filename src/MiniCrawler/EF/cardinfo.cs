namespace MiniCrawler.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("rememberdb.cardinfo")]
    public partial class cardinfo
    {
        public int ID { get; set; }

        [Column(TypeName = "text")]
        [Required]
        [StringLength(65535)]
        public string Content { get; set; }

        public int? CardBoxId { get; set; }

        public virtual cardbox cardbox { get; set; }
    }
}
