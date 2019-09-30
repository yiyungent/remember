namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ParticipantInfo : BaseEntity
    {
        public int ID { get; set; }

        [Column(TypeName = "text")]
        [StringLength(65535)]
        public string RoleNames { get; set; }

        [Column(TypeName = "text")]
        [StringLength(65535)]
        public string Description { get; set; }
    }
}
