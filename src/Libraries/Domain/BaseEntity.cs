using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    [Serializable]
    public class BaseEntity
    {
        /// <summary>
        /// 删除时间：为null，则未删除
        /// </summary>
        public DateTime? DeletedAt { get; set; }

        /// <summary>
        /// 是否被删除
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}
