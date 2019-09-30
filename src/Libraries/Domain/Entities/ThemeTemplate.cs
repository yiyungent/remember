namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("r_moeci_com.themetemplate")]
    public partial class ThemeTemplate
    {
        public int ID { get; set; }

        public int? Status { get; set; }

        [Required]
        [StringLength(100)]
        public string TemplateName { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        public int? IsOpen { get; set; }
    }
}
