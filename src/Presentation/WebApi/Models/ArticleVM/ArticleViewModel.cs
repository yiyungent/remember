using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.ArticleVM
{
    public class ArticleViewModel
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        public AuthorViewModel Author { get; set; }

        /// <summary>
        /// 封面图
        /// </summary>
        public string PicUrl { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public long CreateTime { get; set; }

        /// <summary>
        /// 最近更新时间
        /// </summary>
        public long LastUpdateTime { get; set; }

        /// <summary>
        /// 是否公开
        /// </summary>
        public bool IsOpen { get; set; }

        /// <summary>
        /// 统计信息
        /// </summary>
        public StatViewModel Stat { get; set; }

        public sealed class StatViewModel
        {
            public int LikeNum { get; set; }
            public int DislikeNum { get; set; }
            public int Coin { get; set; }
            public int FavNum { get; set; }
            public int ShareNum { get; set; }
            public int CommentNum { get; set; }
            public int ViewNum { get; set; }
        }

        public sealed class AuthorViewModel
        {
            public int ID { get; set; }

            public string UserName { get; set; }

            public string Desc { get; set; }

            public string Avatar { get; set; }

            public int FansNum { get; set; }
        }
    }
}