namespace MiniCrawler.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("rememberdb.searchtotal")]
    public partial class searchtotal
    {
        public Guid ID { get; set; }

        [StringLength(50)]
        public string KeyWord { get; set; }

        public int SearchCount { get; set; }
    }
}
