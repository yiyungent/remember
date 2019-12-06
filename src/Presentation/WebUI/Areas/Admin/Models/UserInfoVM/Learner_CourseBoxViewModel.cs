using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.Admin.Models.UserInfoVM
{
    public class User_BookInfoViewModel
    {
        public int ID { get; set; }

        public CourseBoxModel CourseBox { get; set; }

        public string JoinTime { get; set; }

        public VideoInfoModel LastPlayVideoInfo { get; set; }

        /// <summary>
        /// 得分
        /// </summary>
        public int Score { get; set; }



        public sealed class VideoInfoModel
        {
            public int Page { get; set; }

            public string Title { get; set; }
        }

        public sealed class CourseBoxModel
        {
            public string Name { get; set; }
        }
    }
}