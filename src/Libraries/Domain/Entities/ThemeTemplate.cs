namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ThemeTemplate : BaseEntity
    {
        public int ID { get; set; }

        [Required]
        [StringLength(100)]
        public string TemplateName { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        public int? IsOpen { get; set; }
    }
}
