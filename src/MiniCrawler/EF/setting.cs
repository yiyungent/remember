namespace MiniCrawler.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("rememberdb.setting")]
    public partial class setting
    {
        public int ID { get; set; }

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
