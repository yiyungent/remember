using Castle.ActiveRecord;
using Domain.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    /// <summary>
    /// 实体类: 系统设置
    /// </summary>
    public partial class Setting
    {
        /// <summary>
        /// 键
        /// </summary>
        [Property(Length = 100, NotNull = true)]
        public string SetKey { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        [Property(Length = 500, NotNull = false)]
        public string SetValue { get; set; }
    }
}
