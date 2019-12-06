using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.BookInfoVM
{
    public class CommentLikeInputModel
    {
        public int CommentId { get; set; }

        /// <summary>
        /// 1: 点赞
        /// 2: 踩
        /// </summary>
        public int DoType { get; set; }
    }
}