using Castle.ActiveRecord;
using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    /// <summary>
    /// 实体类: 角色
    /// </summary>
    [ActiveRecord]
    [Serializable]
    public partial class RoleInfo : BaseEntity<RoleInfo>
    {
        /// <summary>
        /// 是否记录管理操作
        /// 0 否，1 是
        /// </summary>
        [Property(NotNull = false)]
        public bool IsLog { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Property(NotNull = false)]
        public string Remark { get; set; }
    }
}
