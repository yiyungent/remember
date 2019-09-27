using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Models.Common
{
    public class ResponseData
    {
        public int code { get; set; }

        public string message { get; set; }

        public object data { get; set; }
    }
}