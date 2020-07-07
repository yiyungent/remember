using Core;
using Domain;
using Domain.Entities;
using Services;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Framework.Extensions
{
    public static class DomainExt
    {
        /// <summary>
        /// 相对url转换成绝对http url
        /// </summary>
        /// <param name="dbRelativeUrl"></param>
        /// <returns></returns>
        public static string ToHttpAbsoluteUrl(this string dbRelativeUrl)
        {
            string rtnStr = "";
            if (!string.IsNullOrEmpty(dbRelativeUrl))
            {
                // 从 HttpContext 中获取当前请求域名
                var request = HttpContext.Current.Request;
                // http://localhost:4483/HelloWorld.html -> http://localhost:4483
                string prefixHttpHost = request.Url.AbsoluteUri.TrimEnd('/');
                if (request.Url.AbsolutePath != "/" && request.Url.AbsolutePath != "//")
                {
                    prefixHttpHost = request.Url.AbsoluteUri.Replace(request.Url.AbsolutePath, "");
                }

                // http://localhost:4483/assets/images/1.png
                rtnStr = prefixHttpHost + (dbRelativeUrl.StartsWith("/") ? dbRelativeUrl : "/" + dbRelativeUrl);
            }

            return rtnStr;
        }
    }
}
