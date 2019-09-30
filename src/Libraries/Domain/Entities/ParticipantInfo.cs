namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("r_moeci_com.participantinfo")]
    public partial class ParticipantInfo : BaseEntity
    {
        public int ID { get; set; }

        public int? Status { get; set; }

        [Column(TypeName = "text")]
        [StringLength(65535)]
        public string RoleNames { get; set; }

        [Column(TypeName = "text")]
        [StringLength(65535)]
        public string Description { get; set; }
    }
}
