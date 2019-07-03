using Core;
using Domain;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace WebUI.HtmlHelpers
{
    public static class CollapseMenu
    {
        private static IList<Sys_Menu> _allMenuList;

        private const string _btnGroupFormat = "<div class='layui-btn-group' style='float: right;'><button class='layui-btn layui-btn-sm' onclick='onEdit({1})'>修改</button></div>";

        #region 产生菜单列表首页
        public static MvcHtmlString GenerateCollapseMenu(this HtmlHelper value)
        {
            IList<Sys_Menu> allMenuList = Container.Instance.Resolve<Sys_MenuService>().GetAll();
            _allMenuList = allMenuList;
            IList<Sys_Menu> firstMenuList = (from m in allMenuList
                                             where m.ParentMenu == null
                                             orderby m.SortCode ascending
                                             select m).ToList();

            StringBuilder sbMenuHtml = new StringBuilder();
            // 折叠菜单 layui-collapse
            sbMenuHtml.Append("<div class=\"layui-collapse\" lay-accordion=\"\">");

            // 当前同等级别(一级菜单项) 的  菜单项(许多)
            foreach (var firstMenuItem in firstMenuList)
            {
                // 折叠菜单项 layui-colla-item
                sbMenuHtml.Append("<div class=\"layui-colla-item\">");
                // 此菜单项标题 layui-colla-title
                sbMenuHtml.AppendFormat("<h2 class=\"layui-colla-title\">{0}" + _btnGroupFormat + "</h2>", firstMenuItem.Name, firstMenuItem.ID);

                // 此菜单项内容:  1. 无子项---<p></p>   2. 有子项---折叠菜单
                sbMenuHtml.Append("<div class=\"layui-colla-content\">");
                // 里面又有许多项----此处开始进入 首次递归----------直到最后某项不再是任何项的父亲，则为 <p></p>
                // 深度优先--- 利用循环拿到每个菜单项,再进递归拿取其子菜单
                // 注意：如果当前菜单项已经无子项，则为 <p></p>，否则  继续向里递归寻找
                if (firstMenuItem.Children == null || firstMenuItem.Children.Count == 0)
                {
                    sbMenuHtml.AppendFormat("<p>{0} </p>", firstMenuItem.Name);
                }
                else
                {
                    // 否则又是一个折叠菜单
                    SubMenuList(ref sbMenuHtml, firstMenuItem);
                }

                sbMenuHtml.Append("</div>");

                sbMenuHtml.Append("</div>");
            }

            sbMenuHtml.Append("</div>");

            return MvcHtmlString.Create(sbMenuHtml.ToString());
        }
        #endregion

        #region 递归查找子菜单项
        private static void SubMenuList(ref StringBuilder sbMenuHtml, Sys_Menu currentMenu)
        {
            IList<Sys_Menu> subMenuList = (from m in _allMenuList
                                           where m.ParentMenu != null && m.ParentMenu.ID == currentMenu.ID
                                           orderby m.SortCode ascending
                                           select m).ToList();
            // 此菜单项下又嵌套一个折叠菜单
            sbMenuHtml.Append("<div class=\"layui-collapse\" lay-accordion=\"\">");
            foreach (var menuItem in subMenuList)
            {
                sbMenuHtml.Append("<div class=\"layui-colla-item\">");
                sbMenuHtml.AppendFormat("<h2 class=\"layui-colla-title\">{0}" + _btnGroupFormat + "</h2>", menuItem.Name, menuItem.ID);

                sbMenuHtml.Append("<div class=\"layui-colla-content\">");
                // 注意：如果当前菜单项已经无子项，则为 <p></p>，否则  继续向里递归寻找
                if (menuItem.Children == null || menuItem.Children.Count == 0)
                {
                    sbMenuHtml.AppendFormat("<p>{0} </p>", menuItem.Name);
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