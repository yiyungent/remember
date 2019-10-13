using System.Collections.Generic;
using System.Web.Mvc;
using System.Text;
public static class CheckBoxListHelper
{
    public static MvcHtmlString CheckBoxList(this HtmlHelper helper, string name, IEnumerable<SelectListItem> selectList)
    {
        return CheckBoxList(helper, name, selectList, new { });
    }

    public static MvcHtmlString CheckBoxList(this HtmlHelper helper, string name, IEnumerable<SelectListItem> selectList, object htmlAttributes)
    {
        int index = 0;
        StringBuilder stringBuilder = new StringBuilder();
        foreach (SelectListItem selectItem in selectList)
        {
            IDictionary<string, object> newHtmlAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            index++;
            newHtmlAttributes.Add("id", name + "_" + index.ToString());
            //newHtmlAttributes.Add("name", name + "[" + index + "]");
            newHtmlAttributes.Add("name", name);
            newHtmlAttributes.Add("type", "checkbox");
            newHtmlAttributes.Add("value", selectItem.Value);
            if (selectItem.Selected)
            {
                newHtmlAttributes.Add("checked", "checked");
            }
            TagBuilder tagBuilder = new TagBuilder("input");
            tagBuilder.MergeAttributes<string, object>(newHtmlAttributes);
            string inputAllHtml = tagBuilder.ToString(TagRenderMode.SelfClosing);
            stringBuilder.AppendFormat(@"<div class='checkbox'><label>{0}{1}</label></div>", inputAllHtml, selectItem.Text);

        }
        return MvcHtmlString.Create(stringBuilder.ToString());
    }
}