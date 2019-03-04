using Castle.ActiveRecord;
using System;
using System.ComponentModel.DataAnnotations;

namespace Remember.Domain
{
    /// <summary>
    /// 实体类的基类
    /// </summary>
    [Serializable]
    public class BaseEntity<T> : ActiveRecordBase
        where T : class
    {
        /// <summary>
        /// 主键
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Native)]
        [Display(Name = "编号", AutoGenerateField = false)]
        public int ID { get; set; }
    }
}
