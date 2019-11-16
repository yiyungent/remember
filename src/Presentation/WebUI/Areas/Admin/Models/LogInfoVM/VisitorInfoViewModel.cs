using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.Admin.Models.LogInfoVM
{
    public class VisitorInfoViewModel
    {
        public ScreenModel Screen { get; set; }

        public class ScreenModel
        {
            public int Width { get; set; }

            public int Height { get; set; }
        }
    }
}