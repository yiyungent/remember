using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.Admin.Models.CourseBoxVM
{
    public class CourseBoxCreateViewModel
    {
        public string Name { get; set; }

        public string Description { get; set; }


        public string PicUrl { get; set; }

        /// <summary>
        /// 有效日期 - 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 有效日期 - 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
    }
}