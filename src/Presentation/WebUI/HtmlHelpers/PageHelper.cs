using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace WebUI.HtmlHelpers
{
    public static class PageHelper
    {
        public static MvcHtmlString PageLinks(this HtmlHelper value, PageInfo pageInfo, Func<int, string> pageUrl)
        {
            StringBuilder sbResult = new StringBuilder();

            #region 上一页
            if (pageInfo.PageIndex > 1)
            {
                TagBuilder prevLi = new TagBuilder("li");

                TagBuilder prevA = new TagBuilder("a");
                prevA.MergeAttribute("href", pageUrl(pageInfo.PageIndex - 1));
                prevA.InnerHtml = "&laquo;";

                prevLi.InnerHtml = prevA.ToString();

                sbResult.Append(prevLi.ToString());
            }
            #endregion

            #region 首页按钮
            TagBuilder homeLi = new TagBuilder("li");

            TagBuilder homeA = new TagBuilder("a");
            homeA.MergeAttribute("href", pageUrl(1));
            homeA.InnerHtml = "首页";

            homeLi.InnerHtml = homeA.ToString();

            sbResult.Append(homeLi.ToString());
            #endregion
            #region 首页后省略号
            if (pageInfo.PageIndex >= pageInfo.MaxLinkCount)
            {
                TagBuilder tagLi = new TagBuilder("li");
                tagLi.AddCssClass("disabled");
                tagLi.MergeAttribute("disabled", "disabled");

                TagBuilder tagA = new TagBuilder("a");
                tagA.MergeAttribute("href", "#");
                tagA.InnerHtml = "…";

                tagLi.InnerHtml = tagA.ToString();

                sbResult.Append(tagLi.ToString());
            }
            #endregion

            #region 普通页面按钮--页码条

            // 显示分页工具条中普通页码
            // 显示第一个页码
            int begin;
            // 显示最后一个页码
            int end;
            // 第一页码
            begin = pageInfo.PageIndex - (pageInfo.MaxLinkCount / 2);
            if (begin < 1)
            {
                // 第一个页码 不能小于1
                begin = 1;
            }

            // 最大页码
            end = begin + (pageInfo.MaxLinkCount - 1);
            if (end > pageInfo.TotalPages)
            {
                // 最后一个页码不能大于总页数
                end = pageInfo.TotalPages;
                // 修正begin 的值, 若页码条容量为10, 理论上 begin 是 end - 9
                begin = end - (pageInfo.MaxLinkCount - 1);
                if (begin < 1)
                {
                    // 第一个页码 不能小于1
                    begin = 1;
                }
            }
            for (int i = begin; i <= end; i++)
            {
                TagBuilder pageLi = new TagBuilder("li");
                if (i == pageInfo.PageIndex)
                {
                    pageLi.AddCssClass("active");
                }
                else
                {
                }

                TagBuilder pageA = new TagBuilder("a");
                pageA.MergeAttribute("href", pageUrl(i));
                pageA.InnerHtml = i.ToString();

                pageLi.InnerHtml = pageA.ToString();

                sbResult.Append(pageLi.ToString());
            }
            #endregion

            #region 尾页前省略号
            if (pageInfo.PageIndex <= pageInfo.TotalPages - pageInfo.MaxLinkCount + 1)
            {
                TagBuilder tagLi = new TagBuilder("li");
                tagLi.AddCssClass("disabled");
                tagLi.MergeAttribute("disabled", "disabled");

                TagBuilder tagA = new TagBuilder("a");
                tagA.MergeAttribute("href", "#");
                tagA.InnerHtml = "…";

                tagLi.InnerHtml = tagA.ToString();

                sbResult.Append(tagLi.ToString());
            }
            #endregion
            #region 尾页按钮
            TagBuilder endLi = new TagBuilder("li");

            TagBuilder endA = new TagBuilder("a");
            endA.MergeAttribute("href", pageUrl(pageInfo.TotalPages));
            endA.InnerHtml = "尾页";

            endLi.InnerHtml = endA.ToString();

            sbResult.Append(endLi.ToString());
            #endregion

            #region 下一页
            if (pageInfo.PageIndex < pageInfo.TotalPages)
            {
                TagBuilder nextLi = new TagBuilder("li");

                TagBuilder nextA = new TagBuilder("a");
                nextA.MergeAttribute("href", pageUrl(pageInfo.PageIndex + 1));
                nextA.InnerHtml = "&raquo;";

                nextLi.InnerHtml = nextA.ToString();

                sbResult.Append(nextLi.ToString());
            }
            #endregion

            return MvcHtmlString.Create(sbResult.ToString());
        }
    }
}