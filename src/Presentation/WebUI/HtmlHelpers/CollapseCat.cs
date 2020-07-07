using Core;
using Domain;
using Domain.Entities;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace WebUI.HtmlHelpers
{
    public static class CollapseCat
    {
        private static IList<CatInfo> _allList;

        private const string _btnGroupFormat = "<div class='layui-btn-group' style='float: right;'><button class='layui-btn layui-btn-sm' onclick='onEdit({1})'>编辑</button></div>";

        #region 产生分区列表首页
        public static MvcHtmlString GenerateCollapseCat(this HtmlHelper value)
        {
            IList<CatInfo> allList = ContainerManager.Resolve<ICatInfoService>().Filter(m => !m.IsDeleted).ToList();
            _allList = allList;
            IList<CatInfo> firstList = (from m in allList
                                        where m.ParentId == null || m.ParentId == 0
                                        orderby m.SortCode ascending
                                        select m).ToList();

            StringBuilder sbHtml = new StringBuilder();
            // 折叠分区 layui-collapse
            sbHtml.Append("<div class=\"layui-collapse\" lay-accordion=\"\">");

            // 当前同等级别(一级分区项) 的  分区项(许多)
            foreach (var firstItem in firstList)
            {
                // 折叠分区项 layui-colla-item
                sbHtml.Append("<div class=\"layui-colla-item\">");
                // 此分区项标题 layui-colla-title
                sbHtml.AppendFormat("<h2 class=\"layui-colla-title\">{0}" + _btnGroupFormat + "</h2>", firstItem.Name, firstItem.ID);

                // 此分区项内容:  1. 无子项---<p></p>   2. 有子项---折叠分区
                sbHtml.Append("<div class=\"layui-colla-content\">");
                // 里面又有许多项----此处开始进入 首次递归----------直到最后某项不再是任何项的父亲，则为 <p></p>
                // 深度优先--- 利用循环拿到每个分区项,再进递归拿取其子分区
                // 注意：如果当前分区项已经无子项，则为 <p></p>，否则  继续向里递归寻找
                if (firstItem.Children == null || firstItem.Children.Count == 0)
                {
                    sbHtml.AppendFormat("<p>{0} </p>", firstItem.Name);
                }
                else
                {
                    // 否则又是一个折叠分区
                    SubList(ref sbHtml, firstItem);
                }

                sbHtml.Append("</div>");

                sbHtml.Append("</div>");
            }

            sbHtml.Append("</div>");

            return MvcHtmlString.Create(sbHtml.ToString());
        }
        #endregion

        #region 递归查找子分区项
        private static void SubList(ref StringBuilder sbHtml, CatInfo current)
        {
            IList<CatInfo> subList = (from m in _allList
                                      where m.ParentId != null && m.ParentId == current.ID
                                      orderby m.SortCode ascending
                                      select m).ToList();
            // 此分区项下又嵌套一个折叠分区
            sbHtml.Append("<div class=\"layui-collapse\" lay-accordion=\"\">");
            foreach (var Item in subList)
            {
                sbHtml.Append("<div class=\"layui-colla-item\">");
                sbHtml.AppendFormat("<h2 class=\"layui-colla-title\">{0}" + _btnGroupFormat + "</h2>", Item.Name, Item.ID);

                sbHtml.Append("<div class=\"layui-colla-content\">");
                // 注意：如果当前分区项已经无子项，则为 <p></p>，否则  继续向里递归寻找
                if (Item.Children == null || Item.Children.Count == 0)
                {
                    sbHtml.AppendFormat("<p>{0} </p>", Item.Name);
                }
                else
                {
                    SubList(ref sbHtml, Item);
                }
                sbHtml.Append("</div>");

                sbHtml.Append("</div>");
            }
            sbHtml.Append("</div>");
        }
        #endregion
    }
}