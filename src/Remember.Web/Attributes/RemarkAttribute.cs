using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Remember.Web.Attributes
{
    /// <summary>
    /// 备注特性
    /// </summary>
    public class RemarkAttribute : Attribute
    {
        public string Remark { get; set; }

        public RemarkAttribute(string remark)
        {
            this.Remark = remark;
        }
    }
}