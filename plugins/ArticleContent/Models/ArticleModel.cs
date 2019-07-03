using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArticleContent.Models
{
    public class ArticleModel
    {
        public int ID { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string LastUpdateTime { get; set; }

        public int AuthorId { get; set; }

        public UserInfo Author { get; set; }
    }
}