using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Extensions
{
    public static class RequestExt
    {
        public static bool IsPjaxRequest(this HttpRequestBase value)
        {
            if (!string.IsNullOrEmpty(value.Headers["X-PJAX"]) && Convert.ToBoolean(value.Headers["X-PJAX"]))
            {
                // pjax -- 部分视图
                return true;
            }
            else
            {
                //  非 pjax -- 完整视图
                return false;
            }
        }
    }
}