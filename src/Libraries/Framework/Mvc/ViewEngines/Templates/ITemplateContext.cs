using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Mvc.ViewEngines.Templates
{
    /// <summary>
    /// Work context
    /// </summary>
    public interface ITemplateContext
    {
        /// <summary>
        /// Get or set current template system name
        /// </summary>
        string WorkingTemplateName { get; set; }
    }
}
