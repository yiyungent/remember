using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.ArticleVM
{
    public class CommentInputModel
    {
        public int ArticleId { get; set; }

        public string Content { get; set; }
    }
}