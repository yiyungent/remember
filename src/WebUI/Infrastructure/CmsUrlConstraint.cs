using Core;
using NHibernate.Criterion;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace WebUI.Infrastructure
{
    public class CmsUrlConstraint : IRouteConstraint
    {
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (values[parameterName] != null)
            {
                //此为请求的 自定义url参数
                var customUrl = values[parameterName].ToString();

                var articleService = Container.Instance.Resolve<ArticleService>();
                // 根据自定义url 参数值 查找数据库
                var page = articleService.Query(new List<ICriterion>
                {
                    Expression.Eq("CustomUrl", customUrl)
                }).FirstOrDefault();
                if (page != null)
                {
                    //匹配成功进行数据传值
                    HttpContext.Current.Items["cmspage"] = page;
                    return true;
                }
                return false;
            }
            return false;
        }
    }
}