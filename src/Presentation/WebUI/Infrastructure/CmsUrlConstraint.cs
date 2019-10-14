using Core;
using Domain.Entities;
using Services.Interface;
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
                string customUrl = values[parameterName].ToString();

                //var articleService = Container.Instance.Resolve<ArticleService>();
                // 根据自定义url 参数值 查找数据库, 将文章实体拿到
                Article page = ContainerManager.Resolve<IArticleService>().Find(m => m.CustomUrl == customUrl && !m.IsDeleted);
                if (page != null)
                {
                    // 有则说明有此自定义文章url，将其暂时保存
                    HttpContext.Current.Items["CmsPage"] = page;
                    return true;
                }

                return false;
            }
            return false;
        }
    }
}