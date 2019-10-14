using Framework.Common;
using Framework.Factories;
using Framework.Infrastructure;
using Framework.Infrastructure.Abstract;
using Framework.Infrastructure.Concrete;
using System.Web.Mvc;

namespace Framework.HtmlHelpers
{
    public static class AuthTagHelper
    {
        public static IAuthManager AuthManager
        {
            get
            {
                return HttpOneRequestFactory.Get<IAuthManager>();
            }
        }

        static AuthTagHelper()
        {
        }

        #region 权限标签-不支持标签内容为另一标签（即标签嵌套）
        /// <summary>
        /// 权限标签-不支持标签内容为另一标签（即标签嵌套）
        /// </summary>
        /// <param name="authKey">权限操作键</param>
        public static MvcHtmlString AuthTag(this HtmlHelper html, string authKey, string tagName, string innerHtml, object htmlAttributes = null, TagRenderMode tagRenderMode = TagRenderMode.Normal)
        {
            if (!AuthManager.HasAuth(authKey))
            {
                return MvcHtmlString.Create("");
            }

            TagBuilder tag = new TagBuilder(tagName);
            tag.InnerHtml = innerHtml;
            if (htmlAttributes != null)
            {
                tag.MergeAttributes(Tools.ObjectToDictionary(htmlAttributes));
            }

            return MvcHtmlString.Create(tag.ToString(tagRenderMode));
        }

        public static MvcHtmlString AuthTag(this HtmlHelper html, string areaName, string controllerName, string actionName, string tagName, string innerHtml, object htmlAttributes = null, TagRenderMode tagRenderMode = TagRenderMode.Normal)
        {
            string authKey = AuthManager.GetAuthKey(areaName, controllerName, actionName);

            return AuthTag(html, authKey, tagName, innerHtml, htmlAttributes, tagRenderMode);
        }

        public static MvcHtmlString AuthTag(this HtmlHelper html, string controllerName, string actionName, string tagName, string innerHtml, object htmlAttributes = null, TagRenderMode tagRenderMode = TagRenderMode.Normal)
        {
            return AuthTag(html, areaName: null, controllerName: controllerName, actionName: actionName, tagName: tagName, innerHtml: innerHtml, htmlAttributes: htmlAttributes, tagRenderMode: tagRenderMode);
        }
        #endregion

        #region 按钮标签
        public static MvcHtmlString AuthButton(this HtmlHelper html, string authKey, object htmlAttributes = null)
        {
            // 查询此操作权限对应的名字  作为按钮值
            string innerHtml = string.Empty;
            innerHtml = AuthManager.GetAuthNameByKey(authKey);

            return AuthTag(html, authKey, "button", innerHtml: innerHtml, htmlAttributes: htmlAttributes, tagRenderMode: TagRenderMode.Normal);
        }
        #endregion

        #region 双标签（拥有开始结束的标签）--支持标签嵌套
        /// <summary>
        /// 双标签（拥有开始结束的标签）--支持标签嵌套
        /// </summary>
        public static MvcHtmlString AuthDoubleTag(this HtmlHelper html)
        {
            return MvcHtmlString.Create("");
        }
        #endregion

        #region 单标签（自闭合标签）
        /// <summary>
        /// 单标签（自闭合标签）
        /// </summary>
        public static MvcHtmlString AuthSingleTag(this HtmlHelper html)
        {
            return MvcHtmlString.Create("");
        }
        #endregion

        #region 当前登录用户是否有此权限
        public static bool HasAuth(this HtmlHelper html, string authKey)
        {
            return AuthManager.HasAuth(authKey);
        }

        public static bool HasAuth(this HtmlHelper html, string areaName, string controllerName, string actionName)
        {
            return AuthManager.HasAuth(areaName, controllerName, actionName);
        }

        public static bool HasAuth(this HtmlHelper html, string controllerName, string actionName)
        {
            return AuthManager.HasAuth(controllerName, actionName);
        }
        #endregion
    }
}
