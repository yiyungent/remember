namespace MiniCrawler.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("rememberdb.themetemplate")]
    public partial class themetemplate
    {
        public int ID { get; set; }

        [Required]
        [StringLength(100)]
        public string TemplateName { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        public int? Status { get; set; }
    }
}
