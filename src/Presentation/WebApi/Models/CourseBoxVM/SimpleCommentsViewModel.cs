using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.CourseBoxVM
{
    public class SimpleCommentsViewModel
    {

        public SimpleCommentsViewModel()
        {
            this.Comments = new List<CommentModel>();
        }

        public int CourseBoxId { get; set; }

        public IList<CommentModel> Comments { get; set; }

        public sealed class CommentModel
        {
            public int ID { get; set; }

            public AuthorModel Author { get; set; }

            public string Content { get; set; }

            /// <summary>
            /// 13位时间戳
            /// </summary>
            public long CreateTime { get; set; }

            public int LikeNum { get; set; }
        }

        public sealed class AuthorModel
        {
            public int ID { get; set; }

            public string UserName { get; set; }

            public string Avatar { get; set; }
        }
    }
}