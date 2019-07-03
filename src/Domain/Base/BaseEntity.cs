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
        public int ID { get; set; }
    }
}
