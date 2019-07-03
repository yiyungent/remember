using Castle.ActiveRecord;
using Domain.Base;
using System;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    /// <summary>
    /// 实体类： 操作
    /// </summary>
    [ActiveRecord]
    [Serializable]
    public partial class FunctionInfo : BaseEntity<FunctionInfo>
    {
        
    }
}
