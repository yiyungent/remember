using Castle.ActiveRecord;
using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    [ActiveRecord]
    [Serializable]
    public class ThemeTemplate : BaseEntity<ThemeTemplate>
    {
        [Display(Name = "模板名")]
        [Property(Unique = true, Length = 100, NotNull = true)]
        public string TemplateName { get; set; }

        [Display(Name = "模板标题")]
        [Property(Length = 100, NotNull = true)]
        public string Title { get; set; }

        /// <summary>
        /// 状态
        ///     0: 禁用
        ///     1: 开启
        /// </summary>
        [Display(Name = "状态")]
        [Property]
        public int IsOpen { get; set; }
    }
}
