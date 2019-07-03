using PluginHub.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.Admin.Models.PluginVM
{
    public class PluginListViewModel
    {
        public IList<PluginItemModel> List { get; set; }

        #region Ctor
        public PluginListViewModel()
        {
            this.List = new List<PluginItemModel>();
        }
        #endregion
    }

    public class PluginItemModel
    {
        public string SystemName { get; set; }

        public string FriendlyName { get; set; }

        public string Author { get; set; }

        public string Version { get; set; }

        public IList<string> SupportedVersions { get; set; }

        public bool Installed { get; set; }

        public bool HaveConfigure { get; set; }

        public bool HaveRoute { get; set; }

        public string RouteUrl { get; set; }
    }
}