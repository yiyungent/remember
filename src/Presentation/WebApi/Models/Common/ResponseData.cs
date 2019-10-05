using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.Common
{
    public class ResponseData
    {
        public int Code { get; set; }

        public string Message { get; set; }

        public object Data { get; set; }
    }
}