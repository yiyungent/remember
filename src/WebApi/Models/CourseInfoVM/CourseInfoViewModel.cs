using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.CourseInfoVM
{
    public class CourseInfoViewModel
    {
        public int ID { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public int CourseInfoType { get; set; }

        public int CourseBoxId { get; set; }
    }
}