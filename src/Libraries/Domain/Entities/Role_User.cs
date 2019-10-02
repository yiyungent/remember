namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Role_User : BaseEntity
    {
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// 授权时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        #region Relationships

        /// <summary>
        /// 授权人/操作人
        /// </summary>
        [ForeignKey("Operator")]
        public int OperatorId { get; set; }
        [ForeignKey("OperatorId")]
        public virtual UserInfo Operator { get; set; }

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual UserInfo UserInfo { get; set; }

        public int RoleId { get; set; }
        [ForeignKey("RoleId")]
        public virtual RoleInfo RoleInfo { get; set; }

        #endregion




    }
}
