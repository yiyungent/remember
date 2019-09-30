namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("r_moeci_com.searchtotal")]
    public partial class SearchTotal
    {
        public Guid ID { get; set; }

        [StringLength(50)]
        public string KeyWord { get; set; }

        public int SearchCount { get; set; }
    }
}
