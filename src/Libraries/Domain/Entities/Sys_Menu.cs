namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("r_moeci_com.sys_menu")]
    public partial class Sys_Menu
    {
        public int ID { get; set; }

        public int? Status { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(100)]
        public string ControllerName { get; set; }

        [StringLength(100)]
        public string ActionName { get; set; }

        [StringLength(100)]
        public string AreaName { get; set; }

        public int? SortCode { get; set; }

        public int? ParentId { get; set; }
    }
}
