using Castle.ActiveRecord;
using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Base
{
    /// <summary>
    /// 实体类的基类
    /// </summary>
    [Serializable] // 可序列化，用于之后分页
    public class BaseEntity<T> : ActiveRecordBase
        where T : class
    {
        /// <summary>
        /// 主键
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Native)]
        [Display(Name = "编号", AutoGenerateField = false)]
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [Property(Default = "0")]
        public StatusEnum Status { get; set; }
    }
}
