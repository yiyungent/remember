namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("r_moeci_com.loginfo")]
    public partial class LogInfo
    {
        public int ID { get; set; }

        public int? Status { get; set; }

        public int? AccessUserId { get; set; }

        [StringLength(50)]
        public string AccessIp { get; set; }

        [StringLength(100)]
        public string Browser { get; set; }

        [StringLength(100)]
        public string BrowserEngine { get; set; }

        [StringLength(100)]
        public string OS { get; set; }

        [StringLength(100)]
        public string Device { get; set; }

        [StringLength(100)]
        public string Cpu { get; set; }

        public DateTime? AccessTime { get; set; }

        public DateTime? JumpTime { get; set; }

        public long? Duration { get; set; }

        [Column(TypeName = "text")]
        [StringLength(65535)]
        public string AccessUrl { get; set; }

        [Column(TypeName = "text")]
        [StringLength(65535)]
        public string RefererUrl { get; set; }

        public DateTime? CreateTime { get; set; }
    }
}
