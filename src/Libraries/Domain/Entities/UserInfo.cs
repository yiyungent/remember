namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("r_moeci_com.userinfo")]
    public partial class UserInfo
    {
        public int ID { get; set; }

        public int? Status { get; set; }

        [Required]
        [StringLength(30)]
        public string UserName { get; set; }

        [Required]
        [StringLength(64)]
        public string Password { get; set; }

        [StringLength(255)]
        public string RefreshToken { get; set; }

        public DateTime LastLoginTime { get; set; }

        [StringLength(20)]
        public string TemplateName { get; set; }

        [StringLength(50)]
        public string Avatar { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(50)]
        public string Phone { get; set; }

        [Column(TypeName = "text")]
        [StringLength(65535)]
        public string Description { get; set; }

        public long? Coin { get; set; }

        public DateTime RegTime { get; set; }

        [Column(TypeName = "text")]
        [StringLength(65535)]
        public string Remark { get; set; }
    }
}
