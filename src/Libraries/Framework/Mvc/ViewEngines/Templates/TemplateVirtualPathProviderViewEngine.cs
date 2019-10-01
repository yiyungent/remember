using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Framework.Mvc.ViewEngines.Templates
{
    public abstract class TemplateVirtualPathProviderViewEngine : VirtualPathProviderViewEngine
    {
        public const string TemplateSessionKey = "Template";

        // add 使之在当前类可见，在 VirtualPathProviderViewEngine 有此，但为 私有
        internal Func<string, string> GetExtensionThunk;

        protected TemplateVirtualPathProviderViewEngine()
        {
            // add 初始化，原直接在属性处 完成 new 初始化
            GetExtensionThunk = new Func<string, string>(VirtualPathUtility.GetExtension);
        }

        // add 使之在当前类可见，在 VirtualPathProviderViewEngine 有此, 
        // 注意，在 VirtualPathProviderViewEngine 中 private static readonly string[] _emptyLocations = new string[0];
        // 但在此处始终为 null
        //private readonly string[] _emptyLocations = null;
        // 解决: 214行 searchedLocations2 为 null 时报错
        private readonly string[] _emptyLocations = new string[0];

        // mod
        protected virtual string GetPath(ControllerContext controllerContext, string[] locations, string[] areaLocations, string locationsPropertyName, string name, string controllerName, string cacheKeyPrefix, bool useCache, out string[] searchedLocations)
        {
            // add 主题模板
            string templateName = GetCurrentTemplate(controllerContext);
            // mod 自己 字段 的 此
            searchedLocations = _emptyLocations;
            if (string.IsNullOrEmpty(name))
                return string.Empty;
            // mod 自己 实现的 GetAreaName()
            string areaName = GetAreaName(controllerContext.RouteData);
            // update 判断 区域名 是否为空   原直接写在实参列表中的，实则一致
            bool isArea = !string.IsNullOrEmpty(areaName);
            // mod 改用 自己的 TemplateViewLocation, GetViewLocations
            // 如果 为 Area ，则只到 eg. ~/Templates/Red/Areas/Admin/Views 找视图
            List<TemplateViewLocation> viewLocations = GetViewLocations(isArea ? null : locations, isArea ? areaLocations : null);
            if (viewLocations.Count == 0)
            {
                // mod 由于 MvcResources 不可访问，改用自己写错误信息
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Properties cannot be null or empty.", new object[] { locationsPropertyName }));
            }
            // mod 改用自己实现 VirtualPathProviderViewEngine.IsSpecificPath(name);
            bool isSpecificPath = IsSpecificPath(name);
            // mod 调用自己 带 templateName 的 this.CreateCacheKey()
            string key = this.CreateCacheKey(cacheKeyPrefix, name, isSpecificPath ? string.Empty : controllerName, areaName, templateName);
            if (useCache)
            {
                // mod
                var cached = this.ViewLocationCache.GetViewLocation(controllerContext.HttpContext, key);
                if (cached != null)
                {
                    return cached;
                }
            }
            if (!isSpecificPath)
            {
                // mod 改用自己 带 templateName 的
                return this.GetPathFromGeneralName(controllerContext, viewLocations, name, controllerName, areaName, templateName, key, ref searchedLocations);
            }
            return this.GetPathFromSpecificName(controllerContext, name, key, ref searchedLocations);
        }

        // add 使之可见
        protected virtual bool FilePathIsSupported(string virtualPath)
        {
            if (this.FileExtensions == null)
            {
                return true;
            }
            string str = this.GetExtensionThunk(virtualPath).TrimStart(new char[] { '.' });
            return this.FileExtensions.Contains<string>(str, StringComparer.OrdinalIgnoreCase);
        }

        // add 使之可见
        protected virtual string GetPathFromSpecificName(ControllerContext controllerContext, string name, string cacheKey, ref string[] searchedLocations)
        {
            string virtualPath = name;
            if (!this.FilePathIsSupported(name) || !this.FileExists(controllerContext, name))
            {
                virtualPath = string.Empty;
                searchedLocations = new string[] { name };
            }
            this.ViewLocationCache.InsertViewLocation(controllerContext.HttpContext, cacheKey, virtualPath);
            return virtualPath;
        }

        // mod 增加 templateName 参数 
        protected virtual string GetPathFromGeneralName(ControllerContext controllerContext, List<TemplateViewLocation> locations, string name, string controllerName, string areaName, string templateName, string cacheKey, ref string[] searchedLocations)
        {
            string virtualPath = string.Empty;
            searchedLocations = new string[locations.Count];
            for (int i = 0; i < locations.Count; i++)
            {
                string str2 = locations[i].Format(name, controllerName, areaName, templateName);
                if (this.FileExists(controllerContext, str2))
                {
                    searchedLocations = _emptyLocations;
                    virtualPath = str2;
                    this.ViewLocationCache.InsertViewLocation(controllerContext.HttpContext, cacheKey, virtualPath);
                    return virtualPath;
                }
                searchedLocations[i] = str2;
            }
            return virtualPath;
        }

        // add 增加 templateName 参数
        protected virtual string CreateCacheKey(string prefix, string name, string controllerName, string areaName, string templateName)
        {
            return string.Format(CultureInfo.InvariantCulture, ":ViewCacheEntry:{0}:{1}:{2}:{3}:{4}:{5}", new object[] { base.GetType().AssemblyQualifiedName, prefix, name, controllerName, areaName, templateName });
        }

        #region add 替换原 AreaHelpers.GetAreaName(), 而是使用它

        protected virtual string GetAreaName(RouteData routeData)
        {
            object obj2;
            if (routeData.DataTokens.TryGetValue("area", out obj2))
            {
                return (obj2 as string);
            }
            return GetAreaName(routeData.Route);
        }

        protected virtual string GetAreaName(RouteBase route)
        {
            var area = route as IRouteWithArea;
            if (area != null)
            {
                return area.Area;
            }
            var route2 = route as Route;
            if ((route2 != null) && (route2.DataTokens != null))
            {
                return (route2.DataTokens["area"] as string);
            }
            return null;
        }

        #endregion

        // mod 改用自己 的 TemplateViewLocation 类型
        protected virtual List<TemplateViewLocation> GetViewLocations(string[] viewLocationFormats, string[] areaViewLocationFormats)
        {
            var list = new List<TemplateViewLocation>();
            if (areaViewLocationFormats != null)
            {
                list.AddRange(areaViewLocationFormats.Select(s => new TemplateAreaAwareViewLocation(s)).Cast<TemplateViewLocation>());
            }
            if (viewLocationFormats != null)
            {
                list.AddRange(viewLocationFormats.Select(s => new TemplateViewLocation(s)));
            }
            return list;
        }

        // add 使之在当前类可见，与 原 VirtualPathProviderViewEngine 提供的功能一致
        protected virtual bool IsSpecificPath(string name)
        {
            char ch = name[0];
            if (ch != '~')
            {
                return (ch == '/');
            }
            return true;
        }

        // add 查找主题模板
        protected virtual string GetCurrentTemplate(ControllerContext controllerContext)
        {
            //var templateName = controllerContext.RequestContext.HttpContext.Request["Template"];
            object templateNameSession = controllerContext.RequestContext.HttpContext.Session[TemplateSessionKey];
            string templateName = null;
            if (templateNameSession != null)
            {
                templateName = templateNameSession.ToString();
            }

            return templateName;
        }

        // add 但原本就为 public, 重写后，功能一致啊，为什么要重写??????,难道是因为只有这样重写后，this才能指向当前，使用当前类的 GetPath()。原因应该是 GetPath()在原处为 private,无法重写只能覆盖, 如果不在这里再写一次此方法，就会指向原处的 GetPath()
        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            // hack templateName 未设置时, 放弃->找不到了，让下一个视图引擎搜索
            string templateName = GetCurrentTemplate(controllerContext);
            if (string.IsNullOrEmpty(templateName))
            {
                return new ViewEngineResult((IEnumerable<string>)new string[] { "" });
            }

            if (controllerContext == null)
                throw new ArgumentNullException("controllerContext");
            if (string.IsNullOrEmpty(viewName))
                throw new ArgumentException("viewName");
            // controllerName
            string requiredString = controllerContext.RouteData.GetRequiredString("controller");
            string[] searchedLocations1;
            // 视图路径 eg. ~/Templates/Red/Views/Home/Index.cshtml
            string path1 = this.GetPath(controllerContext, this.ViewLocationFormats, this.AreaViewLocationFormats, "ViewLocationFormats", viewName, requiredString, "View", useCache, out searchedLocations1);
            string[] searchedLocations2;
            // Master视图路径,未使用 MasterName ，则为空
            string path2 = this.GetPath(controllerContext, this.MasterLocationFormats, this.AreaMasterLocationFormats, "MasterLocationFormats", masterName, requiredString, "Master", useCache, out searchedLocations2);
            if (string.IsNullOrEmpty(path1) || string.IsNullOrEmpty(path2) && !string.IsNullOrEmpty(masterName))
                return new ViewEngineResult(Enumerable.Union<string>((IEnumerable<string>)searchedLocations1, (IEnumerable<string>)searchedLocations2));
            else
                return new ViewEngineResult(this.CreateView(controllerContext, path1, path2), (IViewEngine)this);
        }

        // add 重写，但代码一致，只为调用本类新 GetPath()
        public override ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            // hack templateName 未设置时, 放弃->找不到了，让下一个视图引擎搜索
            string templateName = GetCurrentTemplate(controllerContext);
            if (string.IsNullOrEmpty(templateName))
            {
                return new ViewEngineResult((IEnumerable<string>)new string[] { "" });
            }

            if (controllerContext == null)
                throw new ArgumentNullException("controllerContext");
            if (string.IsNullOrEmpty(partialViewName))
                throw new ArgumentException("partialViewName");

            string requiredString = controllerContext.RouteData.GetRequiredString("controller");
            string[] searchedLocations;
            string path = this.GetPath(controllerContext, this.PartialViewLocationFormats, this.AreaPartialViewLocationFormats, "PartialViewLocationFormats", partialViewName, requiredString, "Partial", useCache, out searchedLocations);
            if (string.IsNullOrEmpty(path))
                return new ViewEngineResult((IEnumerable<string>)searchedLocations);
            else
                return new ViewEngineResult(this.CreatePartialView(controllerContext, path), (IViewEngine)this);
        }


        #region mod VirtualPathProviderViewEngine---AreaAwareViewLocation, ViewLocation
        // add templateName 参数

        public class TemplateAreaAwareViewLocation : TemplateViewLocation
        {
            public TemplateAreaAwareViewLocation(string virtualPathFormatString)
                : base(virtualPathFormatString)
            {
            }

            public override string Format(string viewName, string controllerName, string areaName, string templateName)
            {
                return string.Format(CultureInfo.InvariantCulture, _virtualPathFormatString, viewName, controllerName, areaName, templateName);
            }
        }

        public class TemplateViewLocation
        {
            protected readonly string _virtualPathFormatString;

            public TemplateViewLocation(string virtualPathFormatString)
            {
                _virtualPathFormatString = virtualPathFormatString;
            }

            public virtual string Format(string viewName, string controllerName, string areaName, string templateName)
            {
                return string.Format(CultureInfo.InvariantCulture, _virtualPathFormatString, viewName, controllerName, templateName);
            }
        }

        #endregion
    }
}
