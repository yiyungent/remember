using Castle.ActiveRecord;
using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Domain
{
    /// <summary>
    /// 实体类：参与者信息
    /// </summary>
    [ActiveRecord]
    [Serializable]
    public class ParticipantInfo : BaseEntity<ParticipantInfo>
    {
        #region Properties

        /// <summary>
        /// 担任角色数组
        /// eg: ["作词", "作曲", "后期"]
        /// </summary>
        [Property(Length = 500, NotNull = false)]
        public string RoleNames { get; set; }

        /// <summary>
        /// 参与描述
        /// <para>在此创作中做了什么</para>
        /// </summary>
        [Property(Length = 500, NotNull = false)]
        public string Description { get; set; } 

        #endregion
    }
}
