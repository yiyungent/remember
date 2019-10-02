namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SearchTotal : BaseEntity
    {
        [Key]
        public Guid ID { get; set; }

        [StringLength(30)]
        public string KeyWord { get; set; }

        public int SearchCount { get; set; }
    }
}
