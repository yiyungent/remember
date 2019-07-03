using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Extensions
{
    public static class DateTimeExt
    {
        public static string ToRegTime(this DateTime value)
        {
            return value.ToString("MM.yyyy");
        }
    }
}