using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.Admin.Models.Common
{
    public class OptionModel
    {
        public virtual int ID { get; set; }

        public virtual string Text { get; set; }

        public virtual bool IsSelected { get; set; }
    }
}