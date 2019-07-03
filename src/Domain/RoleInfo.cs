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
       
    }
}
