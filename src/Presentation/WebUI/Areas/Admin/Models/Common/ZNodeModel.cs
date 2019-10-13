using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.Admin.Models.Common
{
    public class ZNodeModel
    {
        /// <summary>
        /// Sys_Menu.ID
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// Sys_Menu.ParentMenu.ID
        /// </summary>
        public int pId { get; set; }

        /// <summary>
        /// FunctionInfo.ID
        /// </summary>
        public int? fId { get; set; }

        public string name { get; set; }

        public bool isParent { get; set; }

        public bool open { get; set; }

        /// <summary>
        /// 是否选中
        /// </summary>
        public bool @checked { get; set; }
    }
}