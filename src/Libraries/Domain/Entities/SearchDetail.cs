namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SearchDetail : BaseEntity
    {
        [Key]
        public Guid ID { get; set; }

        [StringLength(30)]
        public string KeyWord { get; set; }

        public DateTime? SearchTime { get; set; }
    }
}
