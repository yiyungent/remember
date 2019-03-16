using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Reflection;
using Remember.Web.Attributes;
using System.ComponentModel;

namespace Remember.Web.Extensions
{
    public static class EnumExt
    {
        #region 获取枚举备注 Remark
        public static string GetRemark(this Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            if (fi == null)
            {
                return string.Empty;
            }
            object[] attributes = fi.GetCustomAttributes(typeof(RemarkAttribute), false);
            if (attributes.Length > 0)
            {
                return ((RemarkAttribute)attributes[0]).Remark;
            }
            return string.Empty;
        } 
        #endregion

        #region 获取枚举描述 Description
        public static string GetDescription(this Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            if (fi == null)
            {
                return string.Empty;
            }
            DescriptionAttribute[] attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];
            if (attributes.Length > 0)
            {
                return attributes[0].Description;
            }
            return string.Empty;
        } 
        #endregion
    }
}