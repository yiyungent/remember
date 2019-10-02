namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// 实体类：参与者信息
    /// </summary>
    public partial class ParticipantInfo : BaseEntity
    {
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// 担任角色数组
        /// eg: ["作词", "作曲", "后期"]
        /// </summary>
        [Column(TypeName = "text")]
        [StringLength(30)]
        public string RoleNames { get; set; }

        /// <summary>
        /// 参与描述
        /// <para>在此创作中做了什么</para>
        /// </summary>
        [Column(TypeName = "text")]
        [StringLength(30)]
        public string Description { get; set; }
    }
}
