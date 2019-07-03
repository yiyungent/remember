using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Framework.Mvc.ViewEngines.Templates
{
    public abstract class TemplateBuildManagerViewEngine : TemplateVirtualPathProviderViewEngine
    {
        #region Methods 

        #region Protected Methods 

        // mod 改用实现
        protected override bool FileExists(ControllerContext controllerContext, string virtualPath)
        {
            return System.Web.Compilation.BuildManager.GetObjectFactory(virtualPath, false) != null;
        }

        #endregion Protected Methods 

        #endregion Methods 
    }
}
