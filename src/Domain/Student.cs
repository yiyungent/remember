using Castle.ActiveRecord;
using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    [ActiveRecord]
    public class Student : BaseEntity<Student>
    {
        /// <summary>
        /// 标题
        /// </summary>
        [Display(Name = "姓名")]
        [Property(Length = 30, NotNull = false)]
        public string Name { get; set; }

        /// <summary>
        /// 学号
        /// </summary>
        [Display(Name = "学号")]
        [Property(Length = 2200, NotNull = false)]
        public string StudyNumber { get; set; }

        /// <summary>
        /// 出生日期
        /// </summary>
        [Display(Name = "创建时间")]
        [Property]
        public DateTime CreateTime { get; set; }
    }
}
