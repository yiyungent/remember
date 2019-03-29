using Remember.Core;
using Remember.Domain;
using Remember.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Remember.Web.HtmlHelpExt
{
    public static class CollapseMenu
    {
        private static IList<SysMenu> _allMenuList;

        #region 用于折叠菜单HTML的 HTML标签
        private const string _collapse_HtmlTag = "<div class=\"layui-collapse\" lay-accordion=\"\">";
        private const string _collaItem_HtmlTag = "<div class=\"layui-colla-item\">";
        private const string _collaTitle_HtmlTag = "<h2 class=\"layui-colla-title\">{0} <button class=\"layui-btn layui-btn-sm\" style=\"float: right;margin-top: 6px;\" onclick=\"onEdit()\">修改</button></h2>";
        private const string _collaContent_HtmlTag = "<div class=\"layui-colla-content\">";
        private const string _collaContent_P_HtmlTag = "<p>{0} 描述</p>";
        #endregion

        #region 产生菜单列表首页
        public static MvcHtmlString GenerateCollapseMenu(this HtmlHelper value)
        {
            IList<SysMenu> allMenuList = Container.Instance.Resolve<SysMenuService>().GetAll();
            _allMenuList = allMenuList;
            IList<SysMenu> firstMenuList = (from m in allMenuList
                                            where m.ParentMenu == null
                                            orderby m.SortCode ascending
                                            select m).ToList();

            StringBuilder sbMenuHtml = new StringBuilder();
            // start 折叠菜单 layui-collapse
            sbMenuHtml.Append(_collapse_HtmlTag);

            // 当前同等级别(一级菜单项) 的  菜单项(许多)
            foreach (var firstMenuItem in firstMenuList)
            {
                // start 折叠菜单项 layui-colla-item
                sbMenuHtml.Append(_collaItem_HtmlTag);
                // 此菜单项标题 layui-colla-title
                sbMenuHtml.AppendFormat(_collaTitle_HtmlTag, firstMenuItem.Name);

                // start 此菜单项内容:  1. 无子项---<p></p>   2. 有子项---折叠菜单
                sbMenuHtml.Append(_collaContent_HtmlTag);
                // 里面又有许多项----此处开始进入 首次递归----------直到最后某项不再是任何项的父亲，则为 <p></p>
                // 深度优先--- 利用循环拿到每个菜单项,再进递归拿取其子菜单
                // 注意：如果当前菜单项已经无子项，则为 <p></p>，否则  继续向里递归寻找
                if (firstMenuItem.Children == null || firstMenuItem.Children.Count == 0)
                {
                    sbMenuHtml.AppendFormat(_collaContent_P_HtmlTag, firstMenuItem.Name);
                }
                else
                {
                    // 否则又是一个折叠菜单
                    SubMenuList(ref sbMenuHtml, firstMenuItem);
                }

                sbMenuHtml.Append("</div>");    // end  layui-colla-content

                sbMenuHtml.Append("</div>");    // end layui-colla-item
            }

            sbMenuHtml.Append("</div>"); // end layui-collapse

            return MvcHtmlString.Create(sbMenuHtml.ToString());
        }
        #endregion

        #region 递归查找子菜单项
        private static void SubMenuList(ref StringBuilder sbMenuHtml, SysMenu currentMenu)
        {
            IList<SysMenu> subMenuList = (from m in _allMenuList
                                          where m.ParentMenu != null && m.ParentMenu.ID == currentMenu.ID
                                          orderby m.SortCode ascending
                                          select m).ToList();
            // 此菜单项下又嵌套一个折叠菜单
            sbMenuHtml.Append(_collapse_HtmlTag);
            foreach (var menuItem in subMenuList)
            {
                sbMenuHtml.Append(_collaItem_HtmlTag);
                sbMenuHtml.AppendFormat(_collaTitle_HtmlTag, menuItem.Name);

                sbMenuHtml.Append(_collaContent_HtmlTag);
                // 注意：如果当前菜单项已经无子项，则为 <p></p>，否则  继续向里递归寻找
                if (menuItem.Children == null || menuItem.Children.Count == 0)
                {
                    sbMenuHtml.AppendFormat(_collaContent_P_HtmlTag, menuItem.Name);
                }
                else
                {
                    SubMenuList(ref sbMenuHtml, menuItem);
                }
                sbMenuHtml.Append("</div>");

                sbMenuHtml.Append("</div>");
            }
            sbMenuHtml.Append("</div>");
        }
        #endregion
    }
}