using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Models.UserInfoVM;

namespace WebApi.Models.CourseBoxVM
{
    public class CommentViewModel
    {
        public int ID { get; set; }

        public string Content { get; set; }

        public string CreateTime { get; set; }

        public UserInfoViewModel Author { get; set; }
    }
}