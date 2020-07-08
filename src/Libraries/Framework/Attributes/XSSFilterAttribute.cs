using Framework.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Framework.Attributes
{
    /// <summary>
    /// XSS 过滤器
    /// 来源: https://www.cnblogs.com/sagecheng/p/9462239.html
    /// </summary>
    public class XSSFilterAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// OnActionExecuting
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //获取参数集合
            //var ps = context.ActionDescriptor.Parameters; // .NET Core
            var ps = context.ActionDescriptor.GetParameters();
            //遍历参数集合
            foreach (var p in ps)
            {
                //if (context.ActionArguments[p.Name] != null) // .NET Core
                if (context.ActionParameters[p.ParameterName] != null)
                {
                    //当参数是str
                    if (p.ParameterType.Equals(typeof(string)))
                    {
                        //context.ActionArguments[p.Name] = XSSHelper.XssFilter(context.ActionArguments[p.Name].ToString()); // .NET Core
                        context.ActionParameters[p.ParameterName] = XSSHelper.XssFilter(context.ActionParameters[p.ParameterName].ToString());
                    }
                    else if (p.ParameterType.IsClass)//当参数是一个实体
                    {
                        //PostModelFieldFilter(p.ParameterType, context.ActionArguments[p.Name]); // .NET Core
                        PostModelFieldFilter(p.ParameterType, context.ActionParameters[p.ParameterName]);
                    }
                }

            }
        }
        /// <summary>
        /// 遍历实体的字符串属性
        /// </summary>
        /// <param name="type">数据类型</param>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        private object PostModelFieldFilter(Type type, object obj)
        {
            if (obj != null)
            {
                foreach (var item in type.GetProperties())
                {
                    if (item.GetValue(obj) != null)
                    {
                        //当参数是str
                        if (item.PropertyType.Equals(typeof(string)))
                        {
                            string value = item.GetValue(obj).ToString();
                            item.SetValue(obj, XSSHelper.XssFilter(value));
                        }
                        else if (item.PropertyType.IsClass)//当参数是一个实体
                        {
                            item.SetValue(obj, PostModelFieldFilter(item.PropertyType, item.GetValue(obj)));
                        }
                    }

                }
            }
            return obj;
        }
    }
}
